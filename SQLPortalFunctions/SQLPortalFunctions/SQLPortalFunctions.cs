using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Security.Cryptography.X509Certificates;
using System.Collections;

public partial class Functions
{
    // provides a quick code to reference certain events
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

    /*
    * --------------------------------------------------------
    *  fnWebSendXMLData CLR
    * --------------------------------------------------------
    */

    // adds additional properties and methods to the XmlDocument class
    private class XmlDocument2 : XmlDocument
    {
        // stores the validation errors
        // private ERROR_CODES _ForcedCode = null; public ERROR_CODES ForcedCode { get { return _ForcedCode; } set { _ForcedCode = value; } }
        private Boolean _UniqueNodeValues = false; public Boolean UniqueNodeValues { get { return _UniqueNodeValues; } set { _UniqueNodeValues = value; } }
        private ArrayList _ValidationErrors = new ArrayList(); public ArrayList ValidationErrors { get { return _ValidationErrors; } set { _ValidationErrors = value; } }
    }

    // provides the class for encapsulating the response
    private class PostResult
    {
        // properties

        private SqlString _XMLPosted; public SqlString XMLPosted { get { return _XMLPosted; } set { _XMLPosted = value; } }
        private SqlXml _XMLResponse; public SqlXml XMLResponse { get { return _XMLResponse; } set { _XMLResponse = value; } }
        private SqlBoolean _Success; public SqlBoolean Success { get { return _Success; } set { _Success = value; } }
        private SqlBoolean _Failure; public SqlBoolean Failure { get { return _Failure; } set { _Failure = value; } }
        private SqlString _AdditionalInfo; public SqlString AdditionalInfo { get { return _AdditionalInfo; } set { _AdditionalInfo = value; } }
        private SqlString _Code; public SqlString Code { get { return _Code; } set { _Code = value; } }
        private SqlString _ReturnValue; public SqlString ReturnValue { get { return _ReturnValue; } set { _ReturnValue = value; } }
        private SqlDateTime _Started; public SqlDateTime Started { get { return _Started; } set { _Started = value; } }
        private SqlDateTime _Finished; public SqlDateTime Finished { get { return _Finished; } set { _Finished = value; } }

        // new constructor
        public PostResult(
            SqlString XMLPosted,
            SqlXml XMLResponse,
            SqlBoolean Success,
            SqlBoolean Failure,
            SqlString AdditionalInfo,
            SqlString Code,
            SqlString ReturnValue,
            SqlDateTime? Started = null,
            SqlDateTime? Finished = null
            )
        {
            this.XMLPosted = XMLPosted;
            this.XMLResponse = XMLResponse;
            this.Success = Success;
            this.Failure = Failure;
            this.AdditionalInfo = AdditionalInfo;
            this.Code = Code;
            this.ReturnValue = ReturnValue;
            if (Started.HasValue == false) { this.Started = DateTime.Now; } else { this.Started = (SqlDateTime)Started; }
            if (Finished.HasValue == false) { this.Finished = DateTime.Now; } else { this.Finished = (SqlDateTime)Finished; }
        }
    }

    // the handler for xml validation events
    static void ValidationEventHandler(object sender, System.Xml.Schema.ValidationEventArgs e)
    {
        string ex = null;
        if (e.Exception.GetType() == typeof(XmlSchemaValidationException))
        {
            XmlSchemaValidationException inex = (XmlSchemaValidationException)e.Exception;
            XmlElement el = (XmlElement)inex.SourceObject;
            if (e.Message.Contains("duplicate key sequence") && ((XmlDocument2)el.OwnerDocument).UniqueNodeValues == false) { return; }
            switch (e.Severity)
            {
                case System.Xml.Schema.XmlSeverityType.Error:
                    {
                        ex = String.Format("XML Schema validation - {0}: {1}", e.Severity, e.Message);
                        ((XmlDocument2)el.OwnerDocument).ValidationErrors.Add(new PostResult(null, null, false, true, ex, STATUS_CODES.XML_INVALID_AGAINST_SCHEMA, null));
                        break;
                    }
                case System.Xml.Schema.XmlSeverityType.Warning:
                    {
                        ex = String.Format("XML Schema validation - {0}: {1}", e.Severity, e.Message);
                        ((XmlDocument2)el.OwnerDocument).ValidationErrors.Add(new PostResult(null, null, false, true, ex, STATUS_CODES.XML_INVALID_AGAINST_SCHEMA, null));
                        break;
                    }
            }
        }
        else
        {
            return;
        }
    }

    // the sql table valued function entry point
    [Microsoft.SqlServer.Server.SqlFunction(FillRowMethodName = "FillRow", DataAccess = DataAccessKind.Read,
        TableDefinition = @"
            XMLPosted nvarchar(max),
            XMLResponse xml,
            Success bit,
            Failure bit,
            AdditionalInfo nvarchar(max),
            Started datetime,
            Finished datetime,
            Code nvarchar(100),
            ReturnValue nvarchar(max)")]
    public static IEnumerable fnWebSendXMLData(
        SqlString PostURL,
        SqlString Method,
        SqlString ContentType,
        SqlXml XMLData,
        SqlString P12CertPath,
        SqlString P12CertPass,
        SqlInt32 Timeout,
        SqlString SchemaURI,
        SqlString RootName,
        SqlString SuccessNode,
        SqlString SuccessValue,
        SqlBoolean UniqueNodeValues,
        SqlString XPathReturnValue
        )
    {
        // creates a collection to store the row values ( might have to make this a scalar row rather then multi-rowed )
        ArrayList resultCollection = new ArrayList();

        // logs the time the row had begun processing
        SqlDateTime _StartedTS = DateTime.Now;

        // stores the URL of the request and stores any xml input as bytes
        Uri URL = null;
        byte[] postBytes = null;

        // if the required fields post url and method are not provided then...
        if (PostURL.IsNull || PostURL.Value == "" || Method.IsNull)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [Post URL] was missing or the [Method] was missing.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // if the methods are not correctly set to the only valid values then...
        if (Method.Value != "POST" && Method.Value != "GET")
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [Method] provided was not either POST or GET.", STATUS_CODES.FAILED_VALIDATION, null)); 
            return resultCollection;
        }

        // check to see if the url is correctly formed otherwise...
        if (Uri.TryCreate(PostURL.Value, UriKind.RelativeOrAbsolute, out URL) == false)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [URL] provided was incorrectly formatted.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // if xml schema has been specified but no xml has been passed then...
        if (!SchemaURI.IsNull && XMLData.IsNull)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [Schema URI] was provided but [XML Data] was not.", STATUS_CODES.FAILED_VALIDATION, null)); return resultCollection;
        }

        // check to see if we can access the schema .xsd file otherwise...
        if (!SchemaURI.IsNull && System.IO.File.Exists(SchemaURI.Value) == false)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [Schema URI] was provided but could not be loaded because it could not be found.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // if xml specific fields are available but no xml has been passed then...
        if ((!RootName.IsNull || !SuccessNode.IsNull || !SuccessValue.IsNull) && XMLData.IsNull)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [Root Name] or [Success Node] or [Success Value] was provided but the [XML Data] was missing.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // if certificate pass is specified but no certificate file has then...
        if (!P12CertPass.IsNull && P12CertPath.IsNull)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [P12 Cert Pass] was provided but the [P12 Cert Path] is missing.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // check to see if we can access the certificate file otherwise...
        if (!P12CertPath.IsNull && System.IO.File.Exists(P12CertPath.Value) == false)
        {
            resultCollection.Add(new PostResult(null, null, false, true, "The [P12 Cert Path] was provided but could not be loaded because it could not be found.", STATUS_CODES.FAILED_VALIDATION, null));
            return resultCollection;
        }

        // check that the certificate provided can be opened before performing major code...
        if (!P12CertPath.IsNull)
        {
            try
            {
                if (!P12CertPass.IsNull) 
                { 
                    new X509Certificate2(P12CertPath.Value, P12CertPass.Value); 
                }
                else 
                { 
                    new X509Certificate2(P12CertPath.Value); 
                }
            }
            catch (Exception ex)
            {
                resultCollection.Add(new PostResult(null, null, false, true, String.Format("Certificate exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace), STATUS_CODES.FAILED_VALIDATION, null));
                return resultCollection;
            }
        }

        // check if any xml data has been passed to the clr
        using (var stringWriter = new StringWriter())
        {
            if (!XMLData.IsNull)
            {
                // prepare xml variable
                XmlDocument2 xDoc = new XmlDocument2();

                try
                {
                    // rename root node if applicable
                    if (!RootName.IsNull)
                    {
                        XmlDocument2 _xDoc = new XmlDocument2();
                        _xDoc.LoadXml(XMLData.Value);
                        XmlElement _xDocNewRoot = xDoc.CreateElement(RootName.Value);
                        xDoc.AppendChild(_xDocNewRoot);
                        _xDocNewRoot.InnerXml = _xDoc.DocumentElement.InnerXml;
                    }
                    else
                    {
                        // otherwise just load the xml exactly as provided
                        xDoc.LoadXml(XMLData.Value);
                    }

                    // add the xml declaration if it doesn't exist
                    if (xDoc.FirstChild.NodeType != XmlNodeType.XmlDeclaration)
                    {
                        XmlDeclaration xmlDec = xDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                        XmlElement root = xDoc.DocumentElement;
                        xDoc.InsertBefore(xmlDec, root);
                    }

                    // if the schema file has been specified then validate otherwise skip
                    if (!SchemaURI.IsNull)
                    {
                        System.Xml.Schema.XmlSchemaSet schemas = new System.Xml.Schema.XmlSchemaSet();
                        schemas.Add("", SchemaURI.Value);
                        xDoc.Schemas.Add(schemas);
                        if (!UniqueNodeValues.IsNull) 
                        { 
                            xDoc.UniqueNodeValues = UniqueNodeValues.Value; 
                        }
                        xDoc.Validate(ValidationEventHandler);
                    }

                    // adds any validation errors to the output rows and stops processing if errors were found
                    if (xDoc.ValidationErrors.Count > 0)
                    {
                        resultCollection.AddRange(xDoc.ValidationErrors);
                        return resultCollection;
                    }

                    // Save the xml as a string
                    using (var xmlTextWriter = XmlWriter.Create(stringWriter))
                    {
                        xDoc.WriteTo(xmlTextWriter);
                        xmlTextWriter.Flush();
                    }

                    // get the bytes for the xml document
                    //postBytes = Encoding.UTF8.GetBytes(xDoc.OuterXml);
                    UTF8Encoding encoding = new UTF8Encoding();
                    postBytes = encoding.GetBytes(xDoc.InnerXml);                    
                }
                catch (Exception ex)
                {
                    string errorMsg = String.Format("XML Parse exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                    resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.XML_ERROR, null, _StartedTS));
                    return resultCollection;
                }
            }

            if (Timeout.IsNull) { Timeout = 20; }
            if (ContentType.IsNull) { ContentType = "*/*"; }

            // create the web request object after validation succeeds
            HttpWebRequest webRequest;
            try
            {
                // create the http request
                webRequest = (HttpWebRequest)HttpWebRequest.Create(URL);

                // setup the connection settings
                webRequest.Method = Method.Value;
                webRequest.ContentType = ContentType.Value;
                webRequest.Accept = ContentType.Value;
                webRequest.Timeout = (Timeout.Value * 1000);
                webRequest.SendChunked = true;
                webRequest.KeepAlive = false;
            }
            catch (Exception ex)
            {
                string errorMsg = String.Format("WebRequest Create exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.FAILURE, null, _StartedTS));
                return resultCollection;
            }

            try
            {
                // add the P12 certificate to the request if specified
                if (!P12CertPath.IsNull)
                {
                    // load with password if specified
                    if (!P12CertPass.IsNull) { webRequest.ClientCertificates.Add(new X509Certificate2(P12CertPath.Value, P12CertPass.Value)); }
                    else { webRequest.ClientCertificates.Add(new X509Certificate2(P12CertPath.Value)); }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = String.Format("Certificate exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.CERTIFICATE_ERROR, null, _StartedTS));
                return resultCollection;
            }

            // post the request
            if (Method.Value == "POST")
            {
                try
                {
                    using (StreamWriter postStream = new StreamWriter(webRequest.GetRequestStream(), Encoding.UTF8))
                    {
                        try
                        {
                            // post the data as bytes to the request stream
                            if (postBytes != null)
                            {
                                postStream.Write(System.Text.Encoding.UTF8.GetString(postBytes));
                                postStream.Flush();
                                //postStream.BaseStream.Write(postBytes, 0, postBytes.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMsg = String.Format("StreamWriter exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                            resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.GET_REQUEST_STREAM, null, _StartedTS, DateTime.Now));
                            return resultCollection;
                        }
                        finally
                        {
                            postStream.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    string errorMsg = String.Format("Web request exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                    resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.GET_REQUEST_STREAM, null, _StartedTS, DateTime.Now));
                    return resultCollection;
                }
            }

            string response = string.Empty;
            HttpWebResponse objResponse = null;

            // get the response (also used for GET)
            try
            {
                objResponse = (HttpWebResponse)webRequest.GetResponse();
                if (webRequest.HaveResponse == true)
                {
                    using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                    {
                        try
                        {
                            // get the result of the post
                            response = responseStream.ReadToEnd();

                            // see if the response is in xml format or not
                            XmlDocument doc = null;
                            if (response.IndexOf("<", 0) <= 2)
                            {
                                doc = new XmlDocument();
                                try { doc.LoadXml(response); }
                                catch (Exception) { doc = null; }
                            }

                            // if the response is expected in xml format
                            if (doc != null)
                            {
                                // check for a success value and node if specified...
                                if (!SuccessNode.IsNull && !SuccessValue.IsNull)
                                {
                                    try
                                    {
                                        // use the response xml and get the success node(s) specified in the xpath
                                        XmlNodeList nodeSuccess = doc.SelectNodes(SuccessNode.Value);

                                        using (XmlNodeReader xnr = new XmlNodeReader(doc))
                                        {
                                            if (nodeSuccess.Count == 0)
                                            {
                                                resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), new SqlXml(xnr), false, false, null, STATUS_CODES.XML_XPATH_RESULT_NODE_MISSING,
                                                    ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                                            }
                                            else
                                            {
                                                // loop through each node we need to validate and get the value
                                                IEnumerator EN = nodeSuccess.GetEnumerator();
                                                while (EN.MoveNext() == true)
                                                {
                                                    if (((XmlNode)EN.Current).InnerText == SuccessValue.Value)
                                                    {
                                                        resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), new SqlXml(xnr), true, false, null, STATUS_CODES.SUCCESS,
                                                            ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                                                    }
                                                    else
                                                    {
                                                        resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), new SqlXml(xnr), false, true, null, STATUS_CODES.FAILURE,
                                                            ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        string errorMsg = String.Format("XML reader exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                                        resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.FAILURE,
                                            ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                                        return resultCollection;
                                    }
                                }
                                else
                                {
                                    // if we're not checking for a success node then just return the xml response
                                    using (XmlNodeReader xnr = new XmlNodeReader(doc))
                                    {
                                        resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), new SqlXml(xnr), true, false, null, STATUS_CODES.SUCCESS,
                                            ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                                    }
                                }
                            }
                            else
                            {
                                // all other requests return in plain text in the additional info column
                                resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, true, false, response, STATUS_CODES.SUCCESS,
                                    ReturnStringFromXPath(XPathReturnValue, ref doc), _StartedTS, DateTime.Now));
                            }
                        }
                        catch (Exception ex)
                        {
                            string errorMsg = String.Format("StreamWriter exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                            resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                            return resultCollection;
                        }
                        finally
                        {
                            responseStream.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMsg = String.Format("Web response exception. {0}. {1}. {2}.", ex.Source, ex.Message, ex.StackTrace);
                resultCollection.Add(new PostResult(stringWriter.GetStringBuilder().ToString(), null, false, true, errorMsg, STATUS_CODES.FAILURE, null, _StartedTS, DateTime.Now));
                return resultCollection;
            }
            finally
            {
                if (objResponse != null) { objResponse.Close(); }
            }
        }

        return resultCollection;

    }

    private static String ReturnStringFromXPath(SqlString xp, ref XmlDocument xd)
    {
        if (!xp.IsNull)
        {
            try
            {
                XmlNodeList xnl = xd.SelectNodes(xp.Value);
                if (xnl.Count == 0) { return null; }
                else
                {
                    IEnumerator n = xnl.GetEnumerator(); String t = null;
                    while (n.MoveNext() == true) { t += ((XmlNode)n.Current).InnerText + "\r\n"; }
                    return t;
                }
            }
            catch (Exception) { return null; }
        }
        else { return null; }
    }

    // fill each individual row in sql using the sender object
    public static void FillRow(object postResultObj,
        out SqlString XMLPosted,
        out SqlXml XMLResponse,
        out SqlBoolean Success,
        out SqlBoolean Failure,
        out SqlString AdditionalInfo,
        out SqlDateTime Started,
        out SqlDateTime Finished,
        out SqlString Code,
        out SqlString ReturnValue
        )
    {
        {
            PostResult pr = (PostResult)postResultObj;
            XMLPosted = pr.XMLPosted;
            XMLResponse = pr.XMLResponse;
            Success = pr.Success;
            Failure = pr.Failure;
            AdditionalInfo = pr.AdditionalInfo;
            Started = pr.Started;
            Finished = pr.Finished;
            Code = pr.Code;
            ReturnValue = pr.ReturnValue;
        }
    }
}