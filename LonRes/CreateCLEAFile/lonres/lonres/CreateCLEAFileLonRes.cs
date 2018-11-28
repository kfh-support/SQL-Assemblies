using System;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

namespace lonres
{


    public class StoredProcedures
    {
        // Fields
        private const int MAXNUMBEROFIMAGES = 9;
        private const int MAXNUMBEROFKEYFEATURES = 8;

        // Nested Types
        private enum DataField
        {
            RECORDCOUNT,
            AGENTREFERENCE,
            PROPERTYREFERENCE,
            ADDRESS_1,
            ADDRESS_2,
            TOWN,
            POSTCODE_A,
            POSTCODE_B,
            KEYFEATURES,
            SUMMARY,
            DESCRIPTION,
            BRANCHID,
            PROPERTYSTATUS,
            BEDROOMS,
            TENURETYPEID,
            PRICE,
            PRICETEXT,
            PROPERTYSTYLE,
            CREATEDATE,
            UPDATEDATA,
            DISPLAYADDRESS,
            PUBLISHED,
            TRANSACTIONTYPE,
            NEWHOMES,
            LONRES_FEE,
            SOLDPRICE,
            BATHROOMS,
            RECEPTIONS,
            FLOORLEVEL,
            PHOTOGRAPHS,
            EPCIMAGE,
            EPCTEXT,
            FLOORPLAN,
            MEDIADOCUMENT,
            LONES_SOLD,
            MEDIADOCUMENTEXT
        }

        // Methods
        [SqlProcedure]
        public static void CreateCLEAFileLonRes(int nUploadType, string strDirectoryName)
        {
        
            //DB Connection:

            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("WebUpload_CLEA_GetProperties", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add("@UploadID", SqlDbType.Int, 1).Value = nUploadType;

                SqlDataReader reader = command.ExecuteReader();
                StreamWriter writer = null;
                string str = "";
                string str2 = "";
                
                //Output File Generation:
                while (true)
                {
                    int year = DateTime.Now.Year;
                    //No Data Returned
                    if (!reader.Read())
                    {
                        if (writer != null)
                        {
                            writer.WriteLine("#END#");
                            writer.Close();
                            writer.Dispose();
                        }
                        break;
                    }
                    //Data has been Returned

                    // Create New File Name
                    StringBuilder builder = new StringBuilder();
                    str2 = reader.GetSqlValue(11).ToString();
                    builder.Append(strDirectoryName);
                    builder.Append(str2);
                    object[] args = new object[] {year.ToString()};
                    DateTime now = DateTime.Now;
                    year = now.Year;
                    DateTime time2 = DateTime.Today;
                    string fdate = (time2.Year.ToString() + time2.Month.ToString("00") + time2.Day.ToString("00") );
                    builder.Append(fdate);
                    builder.Append("01");
                    builder.Append(".BLM");
                    if (str != str2)
                    {
                        if (writer != null)
                        {
                            writer.WriteLine("");
                            writer.WriteLine("#END#");
                            writer.Close();
                            writer.Dispose();
                        }
                        writer = new StreamWriter(builder.ToString(), false, Encoding.ASCII, 0x800);
                        writer.Write("#HEADER#");
                        writer.WriteLine("");
                        writer.WriteLine("Version : 3");
                        writer.WriteLine("EOF : '^'");
                        writer.WriteLine("EOR : '~'");
                        writer.WriteLine("Property Count : " + reader.GetSqlValue(0).ToString());
                        writer.WriteLine("Generated Date : " + DateTime.Now);
                        writer.WriteLine("");
                        StringBuilder builder2 = new StringBuilder();
                        writer.WriteLine("#DEFINITION#");
                        builder2.Append("AGENT_REF^ADDRESS_1^ADDRESS_2^TOWN^POSTCODE1^POSTCODE2^");
                        builder2.Append("FEATURE1^FEATURE2^FEATURE3^FEATURE4^FEATURE5^FEATURE6^");
                        builder2.Append("FEATURE7^FEATURE8^SUMMARY^DESCRIPTION^BRANCH_ID^STATUS_ID^");
                        builder2.Append("BEDROOMS^TENURE_TYPE_ID^PRICE^PRICE_QUALIFIER^");
                        builder2.Append("PROP_SUB_ID^CREATE_DATE^UPDATE_DATE^DISPLAY_ADDRESS^PUBLISHED_FLAG^");
                        builder2.Append("TRANS_TYPE_ID^NEW_HOME_FLAG^LONRES_FEE^LONRES_SOLD_PRICE^");
                        builder2.Append("LONRES_BATHS^LONRES_RECEPTION^LONRES_FLOOR_LEVEL^MEDIA_IMAGE_00^");
                        builder2.Append("MEDIA_IMAGE_01^MEDIA_IMAGE_02^MEDIA_IMAGE_03^MEDIA_IMAGE_04^");
                        builder2.Append("MEDIA_IMAGE_05^MEDIA_IMAGE_06^MEDIA_IMAGE_07^MEDIA_IMAGE_08^");
                        builder2.Append("MEDIA_IMAGE_60^MEDIA_IMAGE_TEXT_60^MEDIA_FLOOR_PLAN_01^");
                        builder2.Append("MEDIA_DOCUMENT_00^LONRES_SOLD^");
                        builder2.Append("MEDIA_DOCUMENT_TEXT_00^~");
                        writer.WriteLine(builder2.ToString());
                        writer.WriteLine("");
                        writer.WriteLine("#DATA#");
                    }
                    StringBuilder builder3 = new StringBuilder();
                    int fieldCount = reader.FieldCount;
                    int ordinal = 2;
                    while (true)
                    {
                        if (ordinal >= fieldCount)
                        {
                            builder3.Append("Full Details^~");
                            writer.WriteLine(builder3.ToString());
                            str = str2;
                            break;
                        }
                        string str4 = "";
                        string str5 = "";
                        string str6 = "";
                        string str7 = "";
                        str7 = reader.GetSqlValue(2).ToString();
                        if (!reader.IsDBNull(ordinal))
                        {
                            string str8 = reader.GetSqlValue(ordinal).ToString();
                            if (8 != ordinal)
                            {
                                str8 = str8.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("~", " ");
                            }
                            if (2 == ordinal)
                            {
                                builder3.Append(reader.GetSqlValue(11).ToString() + "_" + str7);
                            }
                            else if (10 == ordinal)
                            {
                                builder3.Append(str8);
                            }
                            else if (8 == ordinal)
                            {
                                string[] strArray = str8.Split(new char[] { '\r' });
                                for (int i = 0; i < 8; i++)
                                {
                                    if ((i < strArray.Length) && (str8 != ""))
                                    {
                                        builder3.Append(strArray[i]);
                                    }
                                    if (i < 7)
                                    {
                                        builder3.Append("^");
                                    }
                                }
                            }
                            else if (0x1d == ordinal)
                            {
                                int num4 = 0;
                                string[] strArray2 = str8.Split(new char[] { '^' });
                                for (int i = 0; i < 9; i++)
                                {
                                    if ((i < strArray2.Length) && (str8 != ""))
                                    {
                                        str4 = strArray2[i].Substring(strArray2[i].Length - 3, 3);
                                        str5 = "$" + "{num4}";
                                        builder3.Append(reader.GetSqlValue(11).ToString() + "_" + str7 + "_IMG_" + ((str5.Length == 2) ? str5 : ("0" + str5)) + "." + str4);
                                    }
                                    if (i < 8)
                                    {
                                        builder3.Append("^");
                                    }
                                    num4++;
                                }
                            }
                            else if ((30 != ordinal) && (0x20 != ordinal))
                            {
                                builder3.Append(str8);
                            }
                            else if (str8.Length > 0)
                            {
                                str4 = str8.Substring(str8.Length - 3, 3);
                                str5 = str8.Substring(str8.Length - 7, 2);
                                if (30 == ordinal)
                                {
                                    string[] strArray4 = new string[] { reader.GetSqlValue(11).ToString(), "_", str7, "_IMG_60.", str4 };
                                    str6 = string.Concat(strArray4);
                                }
                                else
                                {
                                    string[] strArray5 = new string[] { reader.GetSqlValue(11).ToString(), "_", str7, "_FLP_00.", str4 };
                                    str6 = string.Concat(strArray5);
                                }
                                builder3.Append(str6);
                            }
                        }
                        builder3.Append("^");
                        ordinal++;
                    }

                }


            }

        }
    }

}

     
