using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Collections;
using Microsoft.SqlServer.Server;

public partial class UserDefinedFunctions
{
    public static class STATUS_CODES
    {
        public const string SUCCESS = "SUCCESS";
        public const string FAILURE = "FAILURE";    
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

