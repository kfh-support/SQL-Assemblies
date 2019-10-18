using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Collections;
using Microsoft.SqlServer.Server;

public partial class FileFunctions
{
    public static class STATUS_CODES
    {
        public const string CERTIFICATE_MISSING = "CERTIFICATE_MISSING";
        public const string CERTIFICATE_ERROR = "CERTIFICATE_ERROR";
        public const string XML_EXPECTED = "XML_EXPECTED";
        public const string XML_ERROR = "XML_ERROR";
        public const string XML_SCHEMA_MISSING = "XML_SCHEMA_MISSING";
        public const string XML_INVALID_AGAINST_SCHEMA = "XML_INVALID_AGAINST_SCHEMA";
        public const string SQL_ERROR = "SQL_ERROR";
        public const string GET_REQUEST_STREAM = "GET_REQUEST_STREAM";
        public const string GET_RESPONSE_STREAM = "GET_RESPONSE_STREAM";
        public const string SUCCESS = "SUCCESS";
        public const string FAILURE = "FAILURE";
        public const string XML_XPATH_RESULT_NODE_MISSING = "XML_XPATH_RESULT_NODE_MISSING";
        public const string FAILED_VALIDATION = "FAILED_VALIDATION";
        public const string FILE_EXISTS_SKIPPED = "FILE_EXISTS_SKIPPED";
        public const string FILE_EXISTS = "FILE_EXISTS";
        public const string FILE_DOES_NOT_EXIST = "FILE_DOES_NOT_EXIST";
    } 

    // provides the class for encapsulating the response
    private class FileResult
    {
        // properties
        private SqlBoolean _Success; public SqlBoolean Success { get { return _Success; } set { _Success = value; } }
        private SqlString _Message; public SqlString Message { get { return _Message; } set { _Message = value; } }
        private SqlString _Code; public SqlString Code { get { return _Code; } set { _Code = value; } }
        private SqlDecimal _Size; public SqlDecimal Size { get { return _Size; } set { _Size = value; } }
        private SqlDateTime _Started; public SqlDateTime Started { get { return _Started; } set { _Started = value; } }
        private SqlDateTime _Finished; public SqlDateTime Finished { get { return _Finished; } set { _Finished = value; } }
        private SqlString _FileName; public SqlString FileName { get { return _FileName; } set { _FileName = value; } }
        private SqlBinary _Binary; public SqlBinary Binary { get { return _Binary; } set { _Binary = value; } }

        // new constructor
        public FileResult(
            SqlBoolean success,
            SqlString message,
            SqlString code,
            SqlDecimal? size = null,
            SqlDateTime? started = null,
            SqlDateTime? finished = null,
            SqlString? filename = null,
            SqlBinary? binary = null
            )
        {
            this.Success = success;
            this.Message = message;
            this.Code = code;
            this.Size = size ?? 0;
            this.Started = started ?? DateTime.Now;
            this.Finished = finished ?? DateTime.Now;
            this.FileName = filename ?? null;
            this.Binary = binary ?? null;
            //if (finished.HasValue == false) { this.Finished = DateTime.Now; } else { this.Finished = (SqlDateTime)Finished; }
        }
    }


    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow2", DataAccess = DataAccessKind.Read,
        TableDefinition = @"
            Success bit,
            Message nvarchar(max),
            Started datetime,
            Finished datetime,
            Code nvarchar(100),
            Size numeric,
            FileName nvarchar(max),
            Binary varbinary(max)")]
    public static IEnumerable fnManageFile(
        SqlString Operation, // new param
        SqlBinary InFileBinary,
        SqlString InSourceFile,
        SqlString InSourceText, // new param
        SqlString OutDestFolder,
        SqlString OutDestFileName,
        SqlBoolean Overwrite
        )
    {
        // store the results in a list
        ArrayList resultCollection = new ArrayList();

        // logs the time the row had begun processing
        SqlDateTime _StartedTS = DateTime.Now;

        String _Operation = Operation.IsNull ? null : Operation.Value.ToUpper();
        string[] _ValidOperations = new string[] { "C", "D", "M", "N", "E", "RF", "RB", "RD", "RDS" };

        // VALIDATION

        // check to see if a valid operation has been specified
        if (_Operation == null || Array.IndexOf(_ValidOperations, _Operation) == -1)
        {
            resultCollection.Add(new FileResult(
                false, "No valid operation was specified. Valid values are 'C' (Copy), 'D' (Delete), 'M' (Move), 'N' (New), 'E' (Exists), 'RF' (Read File), 'RB' (Read Binary), 'RD' (Read Top Directory Files) and 'RDS' (Read All Directory + Sub Directory Files)).", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
            return resultCollection;
        }

        int ParamCheck = 0;
        if (_Operation == "RDS" || _Operation == "RD")
        {
            if (!InFileBinary.IsNull)
            {
                resultCollection.Add(new FileResult(
                    false, "You cannot use this input on this particular operation.", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
            }
            else { ParamCheck = 1; }
        }
        else { if (!InFileBinary.IsNull) { ParamCheck += 1; } if (!InSourceText.IsNull) { ParamCheck += 1; } if (!InSourceFile.IsNull) { ParamCheck += 1; } }

        // check to see if at least one of the required inputs are specified
        if (ParamCheck == 0)
        {
            resultCollection.Add(new FileResult(
                false, "No binary, source text or file was provided as input. Cancelled.", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
            return resultCollection;
        }

        // check to see if we are using more than one input for the call
        if (ParamCheck > 1)
        {
            resultCollection.Add(new FileResult(
                false, "Too many input arguments were specified. You can only choose one at a time. Cancelled.", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
            return resultCollection;
        }

        if (
            (_Operation == "C" && InSourceFile.IsNull && OutDestFolder.IsNull) |
            ((_Operation == "D" || _Operation == "E" || _Operation == "RF" || _Operation == "RB" || _Operation == "RD" || _Operation == "RDS") && InSourceFile.IsNull) |
            (_Operation == "M" && InSourceFile.IsNull && OutDestFolder.IsNull) |
            (_Operation == "N" && (InSourceText.IsNull || InFileBinary.IsNull) && (OutDestFolder.IsNull || OutDestFileName.IsNull))
           )
        {
            resultCollection.Add(new FileResult(
                false, "One or more provided values were invalid or missing for the type of operation. Cancelled.", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
            return resultCollection;
        }

        // use default overwrite method (if not specified)
        Boolean _Overwrite = false;
        if (!Overwrite.IsNull) { _Overwrite = Overwrite.Value; }

        System.IO.FileInfo fi = null;
        System.IO.FileInfo dfi = null;

        // check source and destination
        try
        {
            // otherwise check source file / directory exists (if specified)
            if (!InSourceFile.IsNull)
            {

                // return from code if we are reading a file
                if (_Operation == "RF" || _Operation == "RB" || _Operation == "RD" || _Operation == "RDS")
                {
                    try
                    {
                        String fileData = null;
                        byte[] fileBinary = null;
                        String filter = "*";

                        if (_Operation == "RF")
                        {
                            if (System.IO.File.Exists(InSourceFile.Value) == true)
                            {
                                fileData = System.IO.File.ReadAllText(InSourceFile.Value);
                                resultCollection.Add(new FileResult(true, fileData, STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now, InSourceFile.Value));
                                return resultCollection;
                            }
                            else
                            {
                                resultCollection.Add(new FileResult(false, "File does not exist.", STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now, InSourceFile.Value));
                                return resultCollection;
                            }
                        }
                        else if (_Operation == "RB")
                        {
                            if (System.IO.File.Exists(InSourceFile.Value) == true)
                            {
                                fileBinary = System.IO.File.ReadAllBytes(InSourceFile.Value);
                                resultCollection.Add(new FileResult(true, null, STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now, InSourceFile.Value, fileBinary));
                                return resultCollection;
                            }
                            else
                            {
                                resultCollection.Add(new FileResult(false, "File does not exist.", STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now, InSourceFile.Value));
                                return resultCollection;
                            }
                        }
                        else if (_Operation == "RD" || _Operation == "RDS")
                        {
                            if (System.IO.Directory.Exists(InSourceFile.Value) == true)
                            {
                                if (_Operation == "RD")
                                {
                                    if (!InSourceText.IsNull) { filter = InSourceText.Value; }
                                    string[] files = System.IO.Directory.GetFiles(InSourceFile.Value, filter, SearchOption.TopDirectoryOnly);
                                    foreach (string f in files)
                                    {
                                        fileData = System.IO.File.ReadAllText(f);
                                        resultCollection.Add(new FileResult(true, fileData, STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now, f));
                                    }
                                    return resultCollection;
                                }
                                else if (_Operation == "RDS")
                                {
                                    if (!InSourceText.IsNull) { filter = InSourceText.Value; }
                                    string[] files = System.IO.Directory.GetFiles(InSourceFile.Value, filter, SearchOption.AllDirectories);
                                    foreach (string f in files)
                                    {
                                        fileData = System.IO.File.ReadAllText(f);
                                        resultCollection.Add(new FileResult(true, fileData, STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now, f));
                                    }
                                    return resultCollection;
                                }
                            }
                            else
                            {
                                resultCollection.Add(new FileResult(false, "Directory does not exist.", STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now, InSourceFile.Value));
                                return resultCollection;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        resultCollection.Add(new FileResult(
                            false, "Error occurred trying to read file / directory: " + ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now, InSourceFile.Value));
                        return resultCollection;
                    }
                }

                if (System.IO.File.Exists(InSourceFile.Value) == true)
                {
                    fi = new System.IO.FileInfo(InSourceFile.Value);
                    if (fi.IsReadOnly == false || fi.IsReadOnly == true) { }

                    // return from code if we are only checking that the file exists
                    if (_Operation == "E")
                    {
                        resultCollection.Add(new FileResult(
                            true, "Source file found.", STATUS_CODES.FILE_EXISTS, fi.Length / 1024, _StartedTS, DateTime.Now));
                        return resultCollection;
                    }

                    // return from code if we are deleting a file
                    else if (_Operation == "D")
                    {
                        try
                        {
                            System.IO.File.Delete(InSourceFile.Value);
                            resultCollection.Add(new FileResult(
                                true, "File deleted successfully.", STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now));
                            return resultCollection;
                        }
                        catch (Exception ex)
                        {
                            resultCollection.Add(new FileResult(
                                false, "Error occurred deleting file: " + ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                            return resultCollection;
                        }
                    }
                }
                else
                {
                    resultCollection.Add(new FileResult(
                        false, "Source file cannot be found.", STATUS_CODES.FILE_DOES_NOT_EXIST, null, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }

            // finally check destination folder exists and destination file can be created
            if (!OutDestFolder.IsNull)
            {
                if (System.IO.Directory.Exists(OutDestFolder.Value) == true)
                {
                    // are we specifying a destination filename?
                    if (!OutDestFileName.IsNull)
                    {
                        // check to see if the destination filename is valid
                        if (System.IO.Path.GetFullPath(OutDestFolder.Value + OutDestFileName.Value) != null)
                        {
                            // if the destination filename already exists and we cannot overwrite the destination file then exit the code
                            if (System.IO.File.Exists(OutDestFolder.Value + OutDestFileName.Value) == true && _Overwrite == false)
                            {
                                resultCollection.Add(new FileResult(
                                    true, "The destination file already exists and you have chosen not to overwrite. Skipping file.", STATUS_CODES.FILE_EXISTS_SKIPPED, null, _StartedTS, DateTime.Now));
                                return resultCollection;
                            }
                            else
                            {
                                dfi = new System.IO.FileInfo(OutDestFolder.Value + OutDestFileName.Value);
                                if (dfi.IsReadOnly == false || dfi.IsReadOnly == true) { } // extra permission checking here
                            }
                        }
                    }
                    else
                    {
                        if (fi != null)
                        {
                            // if not then use the source filename instead and check to see if the destination file can be created
                            if (System.IO.Path.GetFullPath(OutDestFolder.Value + fi.Name) != null)
                            {
                                // does the file already exist in the destination folder?
                                if (System.IO.File.Exists(OutDestFolder.Value + fi.Name) == true && _Overwrite == false)
                                {
                                    resultCollection.Add(new FileResult(
                                        true, "The destination file already exists and you have chosen not to overwrite. Skipping file.", STATUS_CODES.FILE_EXISTS_SKIPPED, null, _StartedTS, DateTime.Now));
                                    return resultCollection;
                                }
                            }
                        }
                        else
                        {
                            resultCollection.Add(new FileResult(
                                true, "There was a problem parsing the input file. Skipping file.", STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                            return resultCollection;
                        }
                    }
                }
                else
                {
                    resultCollection.Add(new FileResult(
                        false, "The destination folder cannot be reached. Please check permissions or the path is correct and contains a trailing slash (\\). Cancelling.", STATUS_CODES.FAILED_VALIDATION, null, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }
        }
        catch (Exception ex)
        {
            resultCollection.Add(new FileResult(false, ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
            return resultCollection;
        }

        // new file
        if (_Operation == "N")
        {
            try
            {
                // binary to file operation
                if (!InFileBinary.IsNull)
                {
                    long fsSize = 0;
                    // otherwise convert the stream of bytes to a file
                    FileStream fs = System.IO.File.Create(OutDestFolder.Value + OutDestFileName.Value);
                    fs.Write(InFileBinary.Value, 0, InFileBinary.Value.Length);
                    fsSize = fs.Length;
                    fs.Close();
                    resultCollection.Add(new FileResult(true, "Binary to file created successfully.", STATUS_CODES.SUCCESS, fsSize / 1024, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }
            catch (Exception ex)
            {
                resultCollection.Add(new FileResult(
                    false, "Error occurred converting binary to file: " + ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                return resultCollection;
            }

            try
            {
                // text to file operation
                if (!InSourceText.IsNull)
                {
                    long fsSize = 0;
                    FileStream fs = System.IO.File.Create(OutDestFolder.Value + OutDestFileName.Value);
                    byte[] bytes = new byte[InSourceText.Value.Length * sizeof(char)];
                    System.Buffer.BlockCopy(InSourceText.Value.ToCharArray(), 0, bytes, 0, bytes.Length);
                    fs.Write(bytes, 0, bytes.Length);
                    fsSize = fs.Length;
                    fs.Close();
                    resultCollection.Add(new FileResult(true, "Text to file created successfully.", STATUS_CODES.SUCCESS, fsSize / 1024, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }
            catch (Exception ex)
            {
                resultCollection.Add(new FileResult(
                    false, "Error occurred storing text to file: " + ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                return resultCollection;
            }
        }
        // copy file or move file
        else if (_Operation == "C" || _Operation == "M")
        {

            try
            {
                // file to file operation
                if (!InSourceFile.IsNull)
                {
                    if (OutDestFileName.IsNull)
                    {
                        switch (_Operation)
                        {
                            case "C":
                                // copy the source file to the destination folder with the original filename
                                System.IO.File.Copy(InSourceFile.Value, OutDestFolder.Value + fi.Name, _Overwrite);
                                break;
                            case "M":
                                // copy the source file to the destination folder with the original filename
                                // might have to include an overwrite parameter for here at a later date, maybe a copy and then delete afterwards
                                System.IO.File.Move(InSourceFile.Value, OutDestFolder.Value + fi.Name);
                                break;
                        }
                    }
                    else
                    {
                        switch (_Operation)
                        {
                            case "C":
                                // copy the source file to the destination folder with the new filename
                                System.IO.File.Copy(InSourceFile.Value, OutDestFolder.Value + OutDestFileName.Value, _Overwrite);
                                break;
                            case "M":
                                // copy the source file to the destination folder with the original filename
                                // might have to include an overwrite parameter for here at a later date, maybe a copy and then delete afterwards
                                System.IO.File.Move(InSourceFile.Value, OutDestFolder.Value + OutDestFileName.Value);
                                break;
                        }
                    }
                    resultCollection.Add(new FileResult(
                        true, "File (moved / copied) successfully.", STATUS_CODES.SUCCESS, null, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }
            catch (Exception ex)
            {
                resultCollection.Add(new FileResult(
                    false, "Error occurred (copying / moving) file to file: " + ex.Message + "\r\n" + ex.StackTrace, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                return resultCollection;
            }
        }

        return resultCollection;
    }

    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow2", DataAccess = DataAccessKind.Read,
        TableDefinition = @"
            Success bit,
            Message nvarchar(max),
            Started datetime,
            Finished datetime,
            Code nvarchar(100),
            Size numeric,
            FileName nvarchar(max),
            Binary varbinary(max)")]
    public static IEnumerable fnSystemFileCreate
        (
            SqlBinary inFileBinary,
            SqlString inSourceDirectory,
            SqlString inSourceFileName,
            SqlBoolean inOverwrite
        )
    {
        string formatMessage = string.Empty;

        // store the results in a list
        ArrayList resultCollection = new ArrayList();

        // logs the time the row had begun processing
        SqlDateTime startTime = DateTime.Now;

        // Complete file path
        string completePath = (string)inSourceDirectory + (string)inSourceFileName;

        // Check if the destination directory exists
        if (System.IO.Directory.Exists((string)inSourceDirectory) == false)
        {
            // Directory does not exist
            formatMessage = String.Format("Output directory {0} does not exist.  Please create this and try again.", (string)inSourceDirectory);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }

        // Check if this file already exists
        if (System.IO.File.Exists(completePath) && inOverwrite == false)
        {
            // File already exists
            formatMessage = String.Format("Output file {0} already exists with no overwriting specified.  Please confirm overwrite and try again.", completePath);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }

        // Create the file
        try
        {
            // binary to file operation
            if (!inFileBinary.IsNull)
            {
                long fsSize = 0;

                // otherwise convert the stream of bytes to a file
                FileStream fs = System.IO.File.Create(completePath);
                fs.Write(inFileBinary.Value, 0, inFileBinary.Value.Length);
                fsSize = fs.Length;
                fs.Close();
                
                // Update the success status
                formatMessage = String.Format("Varbinary data to disk file {0} created successfully.", completePath);
                resultCollection.Add(new FileResult(true, formatMessage, STATUS_CODES.SUCCESS, fsSize / 1024, startTime, DateTime.Now));              
            }
        }
        catch (Exception ex)
        {
            formatMessage = String.Format("Unable to convert varbinary data to disk file {0}: {1}.\r\n{2}.", completePath, ex.Message, ex.StackTrace);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }    

        // All done
        return resultCollection;
    }


    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow2", DataAccess = DataAccessKind.Read,
        TableDefinition = @"
            Success bit,
            Message nvarchar(max),
            Started datetime,
            Finished datetime,
            Code nvarchar(100),
            Size numeric,
            FileName nvarchar(max),
            Binary varbinary(max)")]
    public static IEnumerable fnSystemFileDelete
        (
            SqlString inSourceDirectory,
            SqlString inSourceFileName
        )
    {
        string formatMessage = string.Empty;

        // store the results in a list
        ArrayList resultCollection = new ArrayList();

        // logs the time the row had begun processing
        SqlDateTime startTime = DateTime.Now;

        // Complete file path
        string completePath = (string)inSourceDirectory + (string)inSourceFileName;

        // Check if the destination directory exists
        if (System.IO.Directory.Exists((string)inSourceDirectory) == false)
        {
            // Directory does not exist
            formatMessage = String.Format("Output directory {0} does not exist.  Please create this and try again.", (string)inSourceDirectory);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }

        // Check if this file already exists
        if (System.IO.File.Exists(completePath) == false)
        {
            // File already exists
            formatMessage = String.Format("Physical file {0} does not exist.  Please ensure file exists and try again.", completePath);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }

        // Delete the file
        try
        {
            System.IO.File.Delete(completePath);

            // Update the success status
            formatMessage = String.Format("Disk file {0} successfully deleted.", completePath);
            resultCollection.Add(new FileResult(true, formatMessage, STATUS_CODES.SUCCESS, null, startTime, DateTime.Now));            
        }
        catch (Exception ex)
        {
            formatMessage = String.Format("Unable to delete disk file {0}: {1}.\r\n{2}.", completePath, ex.Message, ex.StackTrace);
            resultCollection.Add(new FileResult(false, formatMessage, STATUS_CODES.FAILURE, null, startTime, DateTime.Now));
            return resultCollection;
        }

        // All done
        return resultCollection;
    }


    // fill each individual row in sql using the sender object
    public static void FillRow2(object postResultObj,
        out SqlBoolean Success,
        out SqlString Message,
        out SqlDateTime Started,
        out SqlDateTime Finished,
        out SqlString Code,
        out SqlDecimal Size,
        out SqlString FileName,
        out SqlBinary Binary
        )
    {
        {
            FileResult pr = (FileResult)postResultObj;
            Success = pr.Success;
            Message = pr.Message;
            Started = pr.Started;
            Finished = pr.Finished;
            Code = pr.Code;
            Size = pr.Size;
            FileName = pr.FileName;
            Binary = pr.Binary;
        }
    }
};

