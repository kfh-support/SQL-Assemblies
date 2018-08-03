using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Collections;
using System.ComponentModel;
using System.Text;
using AFDDemo;

// Everything to sit with a KFH namespace
namespace KFH.Database
{
    public partial class StoredProcedures
    {
        [Microsoft.SqlServer.Server.SqlProcedure]
        public static void SearchForPostCode(string strPostCodeIn, out string strPostCodeOut, out string strStreet, out string strTown, out string strCounty, out string strErrors)
        {
            long retVal;
            strErrors = "";
            strStreet = "";
            strTown = "";
            strCounty = "";
            strPostCodeOut = "";

            // Create an AFD post code instance
            afdAPI.afdAddressData details = new afdAPI.afdAddressData();

            // Make object instance
            afdAPI afdObj;
            afdObj = new afdAPI();

            // Clear Structure
            afdObj.ClearAFDAddressData(ref details);

            // Set the lookup postcode
            details.Lookup = strPostCodeIn; 

            // Carry out the lookup (no need to alter the line below, unless you want to add a sector skip option - see constants)
            retVal = afdObj.AFDData(afdAPI.afdFieldSpec, afdAPI.AFD_FASTFIND_LOOKUP, ref details);

            // Do we have an error?
            if (retVal < 0)
            {
                strErrors = afdObj.AFDErrorText(retVal);
                return;
            }

            // We must have an address, get this
            if (retVal >= 0)
            {
                strStreet = details.Street;
                strPostCodeOut = details.Postcode;

                // If the town is London or in some cases Kent make the user enter the town
                if (details.Town == "London")
                {
                    strTown = "";
                    strCounty = "London";
                }
                else if (details.Town == "Kent")
                {
                    strTown = "";
                    strCounty = "Kent";
                }
                else
                {
                    strTown = details.Town;
                    strCounty = details.OptionalCounty;
                }
            }
        }
    }
};

