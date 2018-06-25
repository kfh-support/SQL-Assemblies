using System;
using System.Text;
using System.Runtime.InteropServices;

// Change namespace below to your project name
namespace AFDDemo
{
  /// <summary>
  /// AFD API Class
  /// </summary>
  public class afdAPI
  {

    /// <summary>
    /// Structure for setting and retrieving data fields
    /// <summary>
    public struct afdAddressData
    {

      public string Lookup;
      public string Name;
      public string Organisation;
      public string Prop;
      public string Street;
      public string Locality;
      public string Town;
      public string PostalCounty;
      public string AbbreviatedPostalCounty;
      public string OptionalCounty;
      public string AbbreviatedOptionalCounty;
      public string TraditionalCounty;
      public string AdministrativeCounty;
      public string Postcode;
      public string DPS;
      public string PostcodeFrom;
      public string PostcodeType;
      public string MailsortCode;
      public string Phone;
      public string GridE;
      public string GridN;
      public string Miles;
      public string Km;
      public string Latitude;
      public string Longitude;
      public string JustBuilt;
      public string UrbanRuralCode;
      public string UrbanRuralName;
      public string WardCode;
      public string WardName;
      public string ConstituencyCode;
      public string Constituency;
      public string EERCode;
      public string EERName;
      public string AuthorityCode;
      public string Authority;
      public string LEACode;
      public string LEAName;
      public string TVRegion;
      public string Occupancy;
      public string OccupancyDescription;
      public string AddressType;
      public string AddressTypeDescription;
      public string UDPRN;
      public string NHSCode;
      public string NHSName;
      public string PCTCode;
      public string PCTName;
      public string CensationCode;
      public string Affluence;
      public string Lifestage;
      public string AdditionalCensusInfo;
      public string SOALower;
      public string SOAMiddle;
      public string Residency;
      public string HouseholdComposition;
      public string Business;
      public string Size;
      public string SICCode;
      public string OnEditedRoll;
      public string Gender;
      public string Forename;
      public string MiddleInitial;
      public string Surname;
      public string DateOfBirth;
      public string PhoneMatchType;
      public string DataSet;
      public string CouncilTaxBand;
      public string Product;
      public string Key;
      public string List;
      public string Country;
      public string CountryISO;
    }

    // Internal Function to lookup Address data
    [DllImport("afddata.dll",EntryPoint="AFDData")]
    private static extern int InternalAFDData32 (IntPtr dataName, int operation, IntPtr tData);
    [DllImport("afddata64.dll",EntryPoint="AFDData")]
    private static extern int InternalAFDData64 (IntPtr dataName, int operation, IntPtr tData);

    // String to let the DLL know the fields required
    public const string afdFieldSpec = "Address@{::}@Lookup:255@Name:120@Organisation:120@Property:120@Street:120@Locality:70@Town:30@PostalCounty:30@AbbreviatedPostalCounty:30@OptionalCounty:30@AbbreviatedOptionalCounty:30@TraditionalCounty:30@AdministrativeCounty:30@Postcode:10@DPS:2@PostcodeFrom:8@PostcodeType:6@MailsortCode:5@Phone:20@GridE:10@GridN:10@Miles:6@Km:6@Latitude:10@Longitude:10@JustBuilt:10@UrbanRuralCode:2@UrbanRuralName:60@WardCode:9@WardName:50@ConstituencyCode:9@Constituency:50@EERCode:9@EERName:40@AuthorityCode:9@Authority:50@LEA Code:9@LEA Name:50@TVRegion:30@Occupancy:6@OccupancyDescription:30@AddressType:6@AddressTypeDescription:55@UDPRN:8@NHSCode:9@NHSName:50@PCTCode:9@PCTName:50@CensationCode:10@Affluence:30@Lifestage:100@AdditionalCensusInfo:200@SOALower:10@SOAMiddle:10@Residency:6@HouseholdComposition:106@Business:100@Size:6@SICCode:10@OnEditedRoll:6@Gender:6@Forename:30@MiddleInitial:6@Surname:30@DateOfBirth:10@PhoneMatchType:6@DataSet:16@CouncilTaxBand:6@Product:40@Key:255@List:512@Country:30@CountryISO:3";

    // Function Type Constants
    public const int AFD_POSTCODE_LOOKUP = 0;
    public const int AFD_POSTCODE_PROPERTY_LOOKUP = 1;
    public const int AFD_FASTFIND_LOOKUP = 2;
    public const int AFD_SEARCH = 3;
    public const int AFD_RETRIEVE_RECORD = 4;
    public const int AFD_CLEAN = 7;
    public const int AFD_GET_NEXT = 32;
    public const int AFD_LIST_BOX = 64;
    public const int AFD_SHOW_ERROR = 128;

    // Sector Skip Constants
    public const int AFD_NO_SKIP = 0;
    public const int AFD_ADDRESS_SKIP = 512;
    public const int AFD_POSTCODE_SKIP = 1024;
    public const int AFD_SECTOR_SKIP = 1536;
    public const int AFD_OUTCODE_SKIP = 2048;
    public const int AFD_POST_TOWN_SKIP = 2560;
    public const int AFD_POSTCODE_AREA_SKIP = 3072;

    // Success Code Constants
    public const int AFD_RECORD_BREAK = 0;
    public const int AFD_SUCCESS = 1;

    // Error Code Constants
    public const int AFD_ERROR_INVALID_FIELDSPEC = -1;
    public const int AFD_ERROR_NO_RESULTS_FOUND = -2;
    public const int AFD_ERROR_INVALID_RECORD_NUMBER = -3;
    public const int AFD_ERROR_OPENING_FILES = -4;
    public const int AFD_ERROR_FILE_READ = -5;
    public const int AFD_ERROR_END_OF_SEARCH = -6;
    public const int AFD_ERROR_DATA_LICENSE_ERROR = -7;
    public const int AFD_ERROR_CONFLICTING_SEARCH_PARAMETERS = -8;
    public const int AFD_USER_CANCELLED = -99;

    // Refiner Cleaning Code Constants
    public const int AFD_REFINER_PAF_MATCH = 100;
    public const int AFD_REFINER_POSTCODE_MATCH = 200;
    public const int AFD_REFINER_CHANGED_POSTCODE = 201;
    public const int AFD_REFINER_ASSUME_POSTCODE_CORRECT = 202;
    public const int AFD_REFINER_ASSUME_CHANGED_POSTCODE_CORRECT = 203;
    public const int AFD_REFINER_ASSUME_POSTCODE_ADDED_PROPERTY = 204;
    public const int AFD_REFINER_ASSUME_CHANGED_POSTCODE_ADDED_PROPERTY = 205;
    public const int AFD_REFINER_FULL_DPS_MATCH = 300;
    public const int AFD_REFINER_FULL_DPS_MATCH_NO_ORG = 301;
    public const int AFD_REFINER_FULL_DPS_MATCH_LIMITED = 302;
    public const int AFD_REFINER_STREET_MATCH = 400;
    public const int AFD_REFINER_NO_MATCH_FOUND = -101;
    public const int AFD_REFINER_AMBIGUOUS_POSTCODE = -102;
    public const int AFD_REFINER_SUGGEST_RECORD = -103;
    public const int AFD_REFINER_AMBIGUOUS_MATCH = -104;
    public const int AFD_REFINER_INTERNATIONAL_ADDRESS = -105;
    public const int AFD_REFINER_NO_RECORD_DATA = -106;

    /// <summary>
    /// Declarations for String Utilities (not required for the core API)
    /// </summary>
    public struct afdStringData
    {
      public string Lookup;
      public string Outcode;
      public string Incode;
      public string Search;
      public string Replace;
    }
    public const string afdStringFieldSpec = "String@@Lookup:255@Outcode:4@Incode:3@Search:255@Replace:255";
    public const int AFD_STRING_SEARCH_REPLACE = 0;
    public const int AFD_STRING_SEARCH_REPLACE_CASE = 1;
    public const int AFD_STRING_CAPITALISE = 2;
    public const int AFD_STRING_CLEAN_LINE = 3;
    public const int AFD_STRING_CHECK_POSTCODE = 4;
    public const int AFD_STRING_CLEAN_POSTCODE = 5;
    public const int AFD_STRING_ABBREVIATE_COUNTY = 6;

    /// <summary>
    /// Declarations for Grid Utilities (not required for the core API)
    /// </summary>
    public struct afdGridData
    {
      public string Lookup;
      public string GBGridE;
      public string GBGridN;
      public string NIGridE;
      public string NIGridN;
      public string Latitude;
      public string Longitude;
      public string TextualLatitude;
      public string TextualLongitude;
      public string Km;
      public string Miles;
      public string GBGridEFrom;
      public string GBGridNFrom;
      public string NIGridEFrom;
      public string NIGridNFrom;
      public string LatitudeFrom;
      public string LongitudeFrom;
      public string TextualLatitudeFrom;
      public string TextualLongitudeFrom;
    }
    public const string afdGridFieldSpec = "Grid@@Lookup:255@GBGridE:10@GBGridN:10@NIGridE:10@NIGridN:10@Latitude:10@Longitude:10@TextualLatitude:15@TextualLongitude:15@Km:6@Miles:6@GBGridEFrom:10@GBGridNFrom:10@NIGridEFrom:10@NIGridNFrom:10@LatitudeFrom:10@LongitudeFrom:10@TextualLatitudeFrom:15@TextualLongitudeFrom:15";
    public const int AFD_GRID_CONVERT = 512;
    public const int AFD_GRID_LOOKUP_LOCATION = 513;
    public const int AFD_GRID_DISTANCE = 514;

    /// <summary>
    /// Declarations for Map Utilities (not required for the core API)
    /// <summary>
    public struct afdMapData
    {
      public string GBGridE;
      public string GBGridN;
      public string NIGridE;
      public string NIGridN;
      public string Latitude;
      public string Longitude;
      public string TextualLatitude;
      public string TextualLongitude;
      public string ImageWidth;
      public string ImageHeight;
      public string ImageScale;
      public string OutputType;
      public string Filename;
    }
    public const string afdMapFieldSpec = "Map@@GBGridE:10@GBGridN:10@NIGridE:10@NIGridN:10@Latitude:10@Longitude:10@TextualLatitude:15@TextualLongitude:15@ImageWidth:4@ImageHeight:4@ImageScale:1@OutputType:1@Filename:255";
    public const string AFD_MAP_BMP = "0";
    public const string AFD_MAP_PNG = "1";
    public const int AFD_MAP_MINI = 0;
    public const int AFD_MAP_FULL = 1;

    /// <summary>
    /// Declarations for Email Utilities (not required for the core API)
    /// <summary>
    public struct afdEmailData
    {
      public string Email;
    }
    public const string afdEmailFieldSpec = "Email@@Email:255";
    public const int AFD_EMAIL_FULL = 0;
    public const int AFD_EMAIL_FORMAT = 2;
    public const int AFD_EMAIL_TLD = 3;
    public const int AFD_EMAIL_LOCAL = 4;

    /// <summary>
    /// Function to lookup Address data
    /// </summary>
    /// <param name="dataName">Field Specification String</param>
    /// <param name="operation">Function Type Constants</param>
    /// <param name="details">Object containing search/lookup values. Becomes populated with results</param>
    /// <returns>The function returns a long which is the result code. This will be &gt;= 0 if the function is successful, or &lt; 0 in the case of an error</returns>
    public long AFDData(string dataName, long operation, ref afdAddressData details)
    {
      
      StringBuilder passDetails = new StringBuilder();
      long retVal;
      
      /// Compose the DLL string from the supplied structure
      passDetails.Append(details.Lookup.PadRight(255, ' '));
      passDetails.Append(details.Name.PadRight(120, ' '));
      passDetails.Append(details.Organisation.PadRight(120, ' '));
      passDetails.Append(details.Prop.PadRight(120, ' '));
      passDetails.Append(details.Street.PadRight(120, ' '));
      passDetails.Append(details.Locality.PadRight(70, ' '));
      passDetails.Append(details.Town.PadRight(30, ' '));
      passDetails.Append(details.PostalCounty.PadRight(30, ' '));
      passDetails.Append(details.AbbreviatedPostalCounty.PadRight(30, ' '));
      passDetails.Append(details.OptionalCounty.PadRight(30, ' '));
      passDetails.Append(details.AbbreviatedOptionalCounty.PadRight(30, ' '));
      passDetails.Append(details.TraditionalCounty.PadRight(30, ' '));
      passDetails.Append(details.AdministrativeCounty.PadRight(30, ' '));
      passDetails.Append(details.Postcode.PadRight(10, ' '));
      passDetails.Append(details.DPS.PadRight(2, ' '));
      passDetails.Append(details.PostcodeFrom.PadRight(8, ' '));
      passDetails.Append(details.PostcodeType.PadRight(6, ' '));
      passDetails.Append(details.MailsortCode.PadRight(5, ' '));
      passDetails.Append(details.Phone.PadRight(20, ' '));
      passDetails.Append(details.GridE.PadRight(10, ' '));
      passDetails.Append(details.GridN.PadRight(10, ' '));
      passDetails.Append(details.Miles.PadRight(6, ' '));
      passDetails.Append(details.Km.PadRight(6, ' '));
      passDetails.Append(details.Latitude.PadRight(10, ' '));
      passDetails.Append(details.Longitude.PadRight(10, ' '));
      passDetails.Append(details.JustBuilt.PadRight(10, ' '));
      passDetails.Append(details.UrbanRuralCode.PadRight(2, ' '));
      passDetails.Append(details.UrbanRuralName.PadRight(60, ' '));
      passDetails.Append(details.WardCode.PadRight(9, ' '));
      passDetails.Append(details.WardName.PadRight(50, ' '));
      passDetails.Append(details.ConstituencyCode.PadRight(9, ' '));
      passDetails.Append(details.Constituency.PadRight(50, ' '));
      passDetails.Append(details.EERCode.PadRight(9, ' '));
      passDetails.Append(details.EERName.PadRight(40, ' '));
      passDetails.Append(details.AuthorityCode.PadRight(9, ' '));
      passDetails.Append(details.Authority.PadRight(50, ' '));
      passDetails.Append(details.LEACode.PadRight(9, ' '));
      passDetails.Append(details.LEAName.PadRight(50, ' '));
      passDetails.Append(details.TVRegion.PadRight(30, ' '));
      passDetails.Append(details.Occupancy.PadRight(6, ' '));
      passDetails.Append(details.OccupancyDescription.PadRight(30, ' '));
      passDetails.Append(details.AddressType.PadRight(6, ' '));
      passDetails.Append(details.AddressTypeDescription.PadRight(55, ' '));
      passDetails.Append(details.UDPRN.PadRight(8, ' '));
      passDetails.Append(details.NHSCode.PadRight(9, ' '));
      passDetails.Append(details.NHSName.PadRight(50, ' '));
      passDetails.Append(details.PCTCode.PadRight(9, ' '));
      passDetails.Append(details.PCTName.PadRight(50, ' '));
      passDetails.Append(details.CensationCode.PadRight(10, ' '));
      passDetails.Append(details.Affluence.PadRight(30, ' '));
      passDetails.Append(details.Lifestage.PadRight(100, ' '));
      passDetails.Append(details.AdditionalCensusInfo.PadRight(200, ' '));
      passDetails.Append(details.SOALower.PadRight(10, ' '));
      passDetails.Append(details.SOAMiddle.PadRight(10, ' '));
      passDetails.Append(details.Residency.PadRight(6, ' '));
      passDetails.Append(details.HouseholdComposition.PadRight(106, ' '));
      passDetails.Append(details.Business.PadRight(100, ' '));
      passDetails.Append(details.Size.PadRight(6, ' '));
      passDetails.Append(details.SICCode.PadRight(10, ' '));
      passDetails.Append(details.OnEditedRoll.PadRight(6, ' '));
      passDetails.Append(details.Gender.PadRight(6, ' '));
      passDetails.Append(details.Forename.PadRight(30, ' '));
      passDetails.Append(details.MiddleInitial.PadRight(6, ' '));
      passDetails.Append(details.Surname.PadRight(30, ' '));
      passDetails.Append(details.DateOfBirth.PadRight(10, ' '));
      passDetails.Append(details.PhoneMatchType.PadRight(6, ' '));
      passDetails.Append(details.DataSet.PadRight(16, ' '));
      passDetails.Append(details.CouncilTaxBand.PadRight(6, ' '));
      passDetails.Append(details.Product.PadRight(40, ' '));
      passDetails.Append(details.Key.PadRight(255, ' '));
      passDetails.Append(details.List.PadRight(512, ' '));
      passDetails.Append(details.Country.PadRight(30, ' '));
      passDetails.Append(details.CountryISO.PadRight(3, ' '));

      // Call the DLL function
      string returnDetails = CallDllFunction(dataName, operation, passDetails, out retVal);
      // Retrieve the data structure from the string returned by the DLL

      details.Lookup = returnDetails.Substring(0, 255).Trim();
      details.Name = returnDetails.Substring(255, 120).Trim();
      details.Organisation = returnDetails.Substring(375, 120).Trim();
      details.Prop = returnDetails.Substring(495, 120).Trim();
      details.Street = returnDetails.Substring(615, 120).Trim();
      details.Locality = returnDetails.Substring(735, 70).Trim();
      details.Town = returnDetails.Substring(805, 30).Trim();
      details.PostalCounty = returnDetails.Substring(835, 30).Trim();
      details.AbbreviatedPostalCounty = returnDetails.Substring(865, 30).Trim();
      details.OptionalCounty = returnDetails.Substring(895, 30).Trim();
      details.AbbreviatedOptionalCounty = returnDetails.Substring(925, 30).Trim();
      details.TraditionalCounty = returnDetails.Substring(955, 30).Trim();
      details.AdministrativeCounty = returnDetails.Substring(985, 30).Trim();
      details.Postcode = returnDetails.Substring(1015, 10).Trim();
      details.DPS = returnDetails.Substring(1025, 2).Trim();
      details.PostcodeFrom = returnDetails.Substring(1027, 8).Trim();
      details.PostcodeType = returnDetails.Substring(1035, 6).Trim();
      details.MailsortCode = returnDetails.Substring(1041, 5).Trim();
      details.Phone = returnDetails.Substring(1046, 20).Trim();
      details.GridE = returnDetails.Substring(1066, 10).Trim();
      details.GridN = returnDetails.Substring(1076, 10).Trim();
      details.Miles = returnDetails.Substring(1086, 6).Trim();
      details.Km = returnDetails.Substring(1092, 6).Trim();
      details.Latitude = returnDetails.Substring(1098, 10).Trim();
      details.Longitude = returnDetails.Substring(1108, 10).Trim();
      details.JustBuilt = returnDetails.Substring(1118, 10).Trim();
      details.UrbanRuralCode = returnDetails.Substring(1128, 2).Trim();
      details.UrbanRuralName = returnDetails.Substring(1130, 60).Trim();
      details.WardCode = returnDetails.Substring(1190, 9).Trim();
      details.WardName = returnDetails.Substring(1199, 50).Trim();
      details.ConstituencyCode = returnDetails.Substring(1249, 9).Trim();
      details.Constituency = returnDetails.Substring(1258, 50).Trim();
      details.EERCode = returnDetails.Substring(1308, 9).Trim();
      details.EERName = returnDetails.Substring(1317, 40).Trim();
      details.AuthorityCode = returnDetails.Substring(1357, 9).Trim();
      details.Authority = returnDetails.Substring(1366, 50).Trim();
      details.LEACode = returnDetails.Substring(1416, 9).Trim();
      details.LEAName = returnDetails.Substring(1425, 50).Trim();
      details.TVRegion = returnDetails.Substring(1475, 30).Trim();
      details.Occupancy = returnDetails.Substring(1505, 6).Trim();
      details.OccupancyDescription = returnDetails.Substring(1511, 30).Trim();
      details.AddressType = returnDetails.Substring(1541, 6).Trim();
      details.AddressTypeDescription = returnDetails.Substring(1547, 55).Trim();
      details.UDPRN = returnDetails.Substring(1602, 8).Trim();
      details.NHSCode = returnDetails.Substring(1610, 9).Trim();
      details.NHSName = returnDetails.Substring(1619, 50).Trim();
      details.PCTCode = returnDetails.Substring(1669, 9).Trim();
      details.PCTName = returnDetails.Substring(1678, 50).Trim();
      details.CensationCode = returnDetails.Substring(1728, 10).Trim();
      details.Affluence = returnDetails.Substring(1738, 30).Trim();
      details.Lifestage = returnDetails.Substring(1768, 100).Trim();
      details.AdditionalCensusInfo = returnDetails.Substring(1868, 200).Trim();
      details.SOALower = returnDetails.Substring(2068, 10).Trim();
      details.SOAMiddle = returnDetails.Substring(2078, 10).Trim();
      details.Residency = returnDetails.Substring(2088, 6).Trim();
      details.HouseholdComposition = returnDetails.Substring(2094, 106).Trim();
      details.Business = returnDetails.Substring(2200, 100).Trim();
      details.Size = returnDetails.Substring(2300, 6).Trim();
      details.SICCode = returnDetails.Substring(2306, 10).Trim();
      details.OnEditedRoll = returnDetails.Substring(2316, 6).Trim();
      details.Gender = returnDetails.Substring(2322, 6).Trim();
      details.Forename = returnDetails.Substring(2328, 30).Trim();
      details.MiddleInitial = returnDetails.Substring(2358, 6).Trim();
      details.Surname = returnDetails.Substring(2364, 30).Trim();
      details.DateOfBirth = returnDetails.Substring(2394, 10).Trim();
      details.PhoneMatchType = returnDetails.Substring(2404, 6).Trim();
      details.DataSet = returnDetails.Substring(2410, 16).Trim();
      details.CouncilTaxBand = returnDetails.Substring(2426, 6).Trim();
      details.Product = returnDetails.Substring(2432, 40).Trim();
      details.Key = returnDetails.Substring(2472, 255).Trim();
      details.List = returnDetails.Substring(2727, 512).Trim();
      details.Country = returnDetails.Substring(3239, 30).Trim();
      details.CountryISO = returnDetails.Substring(3269, 3).Trim();
      // Return with the function return value
      return retVal;

    }

    /// <summary>
    /// Function to lookup String data - used for string utility functions only
    /// </summary>
    /// <param name="dataName">Field Specification String</param>
    /// <param name="operation">Function Type Constants</param>
    /// <param name="details">Object containing search/lookup values. Becomes populated with results</param>
    /// <returns>The function returns a long which is the result code. This will be &gt;= 0 if the function is successful, or &lt; 0 in the case of an error</returns>
    public long AFDData(string dataName, long operation, ref afdStringData details)
    {
      StringBuilder passDetails = new StringBuilder();
      long retVal;

      
      // Compose the DLL string from the supplied structure
      passDetails.Append(details.Lookup.PadRight(255, ' '));
      passDetails.Append(details.Outcode.PadRight(4, ' '));
      passDetails.Append(details.Incode.PadRight(3, ' '));
      passDetails.Append(details.Search.PadRight(255, ' '));
      passDetails.Append(details.Replace.PadRight(255, ' '));

      // Call the DLL function
      string returnDetails = CallDllFunction(dataName, operation, passDetails, out retVal);

      // Retrieve the data structure from the string returned by the DLL

      details.Lookup = returnDetails.Substring(0,255).Trim();
      details.Outcode = returnDetails.Substring(255,4).Trim();
      details.Incode = returnDetails.Substring(259,3).Trim();
      details.Search = returnDetails.Substring(262,255).Trim();
      details.Replace = returnDetails.Substring(517,255).Trim();

      // Return with the function return value
      return retVal;

    }

    /// <summary>
    /// Function to lookup Grid data - used for grid utility functions only
    /// </summary>
    public long AFDData(string dataName, long operation, ref afdGridData details)
    {
      StringBuilder passDetails = new StringBuilder();
      long retVal;

      // Compose the DLL string from the supplied structure
      passDetails.Append(details.Lookup .PadRight(255, ' '));
      passDetails.Append(details.GBGridE .PadRight(10, ' '));
      passDetails.Append(details.GBGridN .PadRight(10, ' '));
      passDetails.Append(details.NIGridE .PadRight(10, ' '));
      passDetails.Append(details.NIGridN .PadRight(10, ' '));
      passDetails.Append(details.Latitude .PadRight(10, ' '));
      passDetails.Append(details.Longitude .PadRight(10, ' '));
      passDetails.Append(details.TextualLatitude .PadRight(15, ' '));
      passDetails.Append(details.TextualLongitude .PadRight(15, ' '));
      passDetails.Append(details.Km .PadRight(6, ' '));
      passDetails.Append(details.Miles .PadRight(6, ' '));
      passDetails.Append(details.GBGridEFrom .PadRight(10, ' '));
      passDetails.Append(details.GBGridNFrom .PadRight(10, ' '));
      passDetails.Append(details.NIGridEFrom .PadRight(10, ' '));
      passDetails.Append(details.NIGridNFrom .PadRight(10, ' '));
      passDetails.Append(details.LatitudeFrom .PadRight(10, ' '));
      passDetails.Append(details.LongitudeFrom .PadRight(10, ' '));
      passDetails.Append(details.TextualLatitudeFrom .PadRight(15, ' '));
      passDetails.Append(details.TextualLongitudeFrom .PadRight(15, ' '));

      // Call the DLL function
      string returnDetails = CallDllFunction(dataName, operation, passDetails, out retVal);

      // Retrieve the data structure from the string returned by the DLL

      details.Lookup = returnDetails.Substring(0,255).Trim();
      details.GBGridE = returnDetails.Substring(255,10).Trim();
      details.GBGridN = returnDetails.Substring(265,10).Trim();
      details.NIGridE = returnDetails.Substring(275,10).Trim();
      details.NIGridN = returnDetails.Substring(285,10).Trim();
      details.Latitude = returnDetails.Substring(295,10).Trim();
      details.Longitude = returnDetails.Substring(305,10).Trim();
      details.TextualLatitude = returnDetails.Substring(315,15).Trim();
      details.TextualLongitude = returnDetails.Substring(330,15).Trim();
      details.Km = returnDetails.Substring(345,6).Trim();
      details.Miles = returnDetails.Substring(351,6).Trim();
      details.GBGridEFrom = returnDetails.Substring(357,10).Trim();
      details.GBGridNFrom = returnDetails.Substring(367,10).Trim();
      details.NIGridEFrom = returnDetails.Substring(377,10).Trim();
      details.NIGridNFrom = returnDetails.Substring(387,10).Trim();
      details.LatitudeFrom = returnDetails.Substring(397,10).Trim();
      details.LongitudeFrom = returnDetails.Substring(407,10).Trim();
      details.TextualLatitudeFrom = returnDetails.Substring(417,15).Trim();
      details.TextualLongitudeFrom = returnDetails.Substring(432,15).Trim();

      // Return with the function return value
      return retVal;

    }

    /// <summary>
    /// Function to lookup Map data - used for map utility functions only
    /// </summary>
    /// <param name="dataName">Field Specification String</param>
    /// <param name="operation">Function Type Constants</param>
    /// <param name="details">Object containing search/lookup values. Becomes populated with results</param>
    /// <returns>The function returns a long which is the result code. This will be &gt;= 0 if the function is successful, or &lt; 0 in the case of an error</returns>
    public long AFDData(string dataName, long operation, ref afdMapData details)
    {
      StringBuilder passDetails = new StringBuilder();
      long retVal;

      // Compose the DLL string from the supplied structure
      passDetails.Append(details.GBGridE.PadRight(10, ' '));
      passDetails.Append(details.GBGridN.PadRight(10, ' '));
      passDetails.Append(details.NIGridE.PadRight(10, ' '));
      passDetails.Append(details.NIGridN.PadRight(10, ' '));
      passDetails.Append(details.Latitude.PadRight(10, ' '));
      passDetails.Append(details.Longitude.PadRight(10, ' '));
      passDetails.Append(details.TextualLatitude.PadRight(15, ' '));
      passDetails.Append(details.TextualLongitude.PadRight(15, ' '));
      passDetails.Append(details.ImageWidth.PadRight(4, ' '));
      passDetails.Append(details.ImageHeight.PadRight(4, ' '));
      passDetails.Append(details.ImageScale.PadRight(1, ' '));
      passDetails.Append(details.OutputType.PadRight(1, ' '));
      passDetails.Append(details.Filename.PadRight(255, ' '));

      // Call the DLL function
      string returnDetails = CallDllFunction(dataName, operation, passDetails, out retVal);

      // Retrieve the data structure from the string returned by the DLL

      details.GBGridE = returnDetails.Substring(0,10).Trim();
      details.GBGridN = returnDetails.Substring(10,10).Trim();
      details.NIGridE = returnDetails.Substring(20,10).Trim();
      details.NIGridN = returnDetails.Substring(30,10).Trim();
      details.Latitude = returnDetails.Substring(40,10).Trim();
      details.Longitude = returnDetails.Substring(50,10).Trim();
      details.TextualLatitude = returnDetails.Substring(60,15).Trim();
      details.TextualLongitude = returnDetails.Substring(75,15).Trim();
      details.ImageWidth = returnDetails.Substring(90,4).Trim();
      details.ImageHeight = returnDetails.Substring(94,4).Trim();
      details.ImageScale = returnDetails.Substring(98,1).Trim();
      details.OutputType = returnDetails.Substring(99,1).Trim();
      details.Filename = returnDetails.Substring(100,255).Trim();

      // Return with the function return value
      return retVal;

    }

    /// <summary>
    /// Function to lookup Email data - used for email utility functions only
    /// </summary>
    /// <param name="dataName">Field Specification String</param>
    /// <param name="operation">Function Type Constants</param>
    /// <param name="details">Object containing search/lookup values. Becomes populated with results</param>
    /// <returns>The function returns a long which is the result code. This will be &gt;= 0 if the function is successful, or &lt; 0 in the case of an error</returns>
    public long AFDData(string dataName, long operation, ref afdEmailData details)
    {
      StringBuilder passDetails = new StringBuilder();
      long retVal;

      
      // Compose the DLL string from the supplied structure
      passDetails.Append(details.Email.PadRight(255, ' '));

      // Call the DLL function
      string returnDetails = CallDllFunction(dataName, operation, passDetails, out retVal);

      // Retrieve the data structure from the string returned by the DLL

      details.Email = returnDetails.Substring(0,255).Trim();

      // Return with the function return value
      return retVal;

    }

    /// <summary>
    /// Function to return text corresponding to the error code supplied, e.g. for display in a Message Box
    /// </summary>
    /// <param name="errorCode">Error code returned by AFDdata method</param>
    /// <returns>The function return the relevent error message</returns>
    public string AFDErrorText(long errorCode) 
    {
      switch (errorCode)
      {
        case AFD_ERROR_INVALID_FIELDSPEC:
          return "Application Internal Error"; // The fieldSpec string passed to the AFD API was invalid
        case AFD_ERROR_NO_RESULTS_FOUND:
          return "No Results Found";
        case AFD_ERROR_INVALID_RECORD_NUMBER:
          return "Invalid Record Number"; // Only possible if an invalid key is passed when re-looking up a result
        case AFD_ERROR_OPENING_FILES:
          return "Error Opening AFD Data Files";
        case AFD_ERROR_FILE_READ:
          return "AFD Data File Read Error";
        case AFD_ERROR_END_OF_SEARCH:
          return "End Of Search"; // Shouldn't normally be displayed to the user
        case AFD_ERROR_DATA_LICENSE_ERROR:
          return "Data License Error - Check software is registered";
        case AFD_ERROR_CONFLICTING_SEARCH_PARAMETERS:
          return "Conflicting Search Parameters"; // The Name and Organisation fields cannot be searched at the same time
                                                    // The UDPRN field must be searched independently of any other fields
        case AFD_USER_CANCELLED:
          return "User Cancelled"; // User Cancelled DLL Internal ListBox
        default:
          if (errorCode >= 0)
            return ""; // No Error - Success!
          else
            return "Unknown Error " + errorCode.ToString();
      }
    }

    /// <summary>
    /// Function to return text corresponding to the Refiner Cleaning Result
    /// <param name="returnCode"></param>
    public string AFDRefinerCleaningText   (long returnCode) 
    {
      switch (returnCode)
      {
        case AFD_REFINER_PAF_MATCH:
          return "Record identically matches PAF Record";
        case AFD_REFINER_POSTCODE_MATCH:
          return "Postcode Match";
        case AFD_REFINER_CHANGED_POSTCODE:
          return "Postcode Match with Changed Postcode";
        case AFD_REFINER_ASSUME_POSTCODE_CORRECT:
          return "Postcode, Property Match Assuming The Postcode Is Correct";
        case AFD_REFINER_ASSUME_CHANGED_POSTCODE_CORRECT:
          return "Postcode, Property Match with Changed Postcode Assuming The Postcode Is Correct";
        case AFD_REFINER_ASSUME_POSTCODE_ADDED_PROPERTY:
          return "Postcode, Property Match with Non-Matched Property Added In";
        case AFD_REFINER_ASSUME_CHANGED_POSTCODE_ADDED_PROPERTY:
          return "Postcode, Property Match with Changed Postcode and Non-Matched Property Added In";
        case AFD_REFINER_FULL_DPS_MATCH:
          return "Full DPS Match";
        case AFD_REFINER_FULL_DPS_MATCH_NO_ORG:
          return "Full DPS Match (Ambiguous Organisation Not Retained)";
        case AFD_REFINER_FULL_DPS_MATCH_LIMITED:
          return "Limited DPS Match (Locality or Town information not present)";
        case AFD_REFINER_STREET_MATCH:
          return "Street (Non DPS) Match";
        case AFD_REFINER_NO_MATCH_FOUND:
          return "No Match Found";
        case AFD_REFINER_AMBIGUOUS_POSTCODE:
          return "Ambiguous Postcode (Non DPS Match)";
        case AFD_REFINER_SUGGEST_RECORD:
          return "Suggested Record";
        case AFD_REFINER_AMBIGUOUS_MATCH:
          return "Ambiguous Match";
        case AFD_REFINER_INTERNATIONAL_ADDRESS:
          return "International Addres";
        case AFD_REFINER_NO_RECORD_DATA:
          return "No Record Data Supplied";
        default:
          return AFDErrorText(returnCode);
      }
    }

    /// <summary>
    /// Procedure to clear AFDAddressData Structure
    /// </summary>
    /// <param name="details">AFDData to be cleared</param>
    public void ClearAFDAddressData(ref afdAddressData details) 
    {
      details.Lookup = string.Empty;
      details.Name = string.Empty;
      details.Organisation = string.Empty;
      details.Prop = string.Empty;
      details.Street = string.Empty;
      details.Locality = string.Empty;
      details.Town = string.Empty;
      details.PostalCounty = string.Empty;
      details.AbbreviatedPostalCounty = string.Empty;
      details.OptionalCounty = string.Empty;
      details.AbbreviatedOptionalCounty = string.Empty;
      details.TraditionalCounty = string.Empty;
      details.AdministrativeCounty = string.Empty;
      details.Postcode = string.Empty;
      details.DPS = string.Empty;
      details.PostcodeFrom = string.Empty;
      details.PostcodeType = string.Empty;
      details.MailsortCode = string.Empty;
      details.Phone = string.Empty;
      details.GridE = string.Empty;
      details.GridN = string.Empty;
      details.Miles = string.Empty;
      details.Km = string.Empty;
      details.Latitude = string.Empty;
      details.Longitude = string.Empty;
      details.JustBuilt = string.Empty;
      details.UrbanRuralCode = string.Empty;
      details.UrbanRuralName = string.Empty;
      details.WardCode = string.Empty;
      details.WardName = string.Empty;
      details.ConstituencyCode = string.Empty;
      details.Constituency = string.Empty;
      details.EERCode = string.Empty;
      details.EERName = string.Empty;
      details.AuthorityCode = string.Empty;
      details.Authority = string.Empty;
      details.LEACode = string.Empty;
      details.LEAName = string.Empty;
      details.TVRegion = string.Empty;
      details.Occupancy = string.Empty;
      details.OccupancyDescription = string.Empty;
      details.AddressType = string.Empty;
      details.AddressTypeDescription = string.Empty;
      details.UDPRN = string.Empty;
      details.NHSCode = string.Empty;
      details.NHSName = string.Empty;
      details.PCTCode = string.Empty;
      details.PCTName = string.Empty;
      details.CensationCode = string.Empty;
      details.Affluence = string.Empty;
      details.Lifestage = string.Empty;
      details.AdditionalCensusInfo = string.Empty;
      details.SOALower = string.Empty;
      details.SOAMiddle = string.Empty;
      details.Residency = string.Empty;
      details.HouseholdComposition = string.Empty;
      details.Business = string.Empty;
      details.Size = string.Empty;
      details.SICCode = string.Empty;
      details.OnEditedRoll = string.Empty;
      details.Gender = string.Empty;
      details.Forename = string.Empty;
      details.MiddleInitial = string.Empty;
      details.Surname = string.Empty;
      details.DateOfBirth = string.Empty;
      details.PhoneMatchType = string.Empty;
      details.DataSet = string.Empty;
      details.CouncilTaxBand = string.Empty;
      details.Product = string.Empty;
      details.Key = string.Empty;
      details.List = string.Empty;
      details.Country = string.Empty;
      details.CountryISO = string.Empty;
    }

    /// Procedure to clear AFDGridData Structure
    /// </summary>
    /// <param name="details">AFDData to be cleared</param>
    public void ClearAFDGridData(ref afdGridData details) 
    {
      details.Lookup = string.Empty;
      details.GBGridE = string.Empty;
      details.GBGridN = string.Empty;
      details.NIGridE = string.Empty;
      details.NIGridN = string.Empty;
      details.Latitude = string.Empty;
      details.Longitude = string.Empty;
      details.TextualLatitude = string.Empty;
      details.TextualLongitude = string.Empty;
      details.Km = string.Empty;
      details.Miles = string.Empty;
      details.GBGridEFrom = string.Empty;
      details.GBGridNFrom = string.Empty;
      details.NIGridEFrom = string.Empty;
      details.NIGridNFrom = string.Empty;
      details.LatitudeFrom = string.Empty;
      details.LongitudeFrom = string.Empty;
      details.TextualLatitudeFrom = string.Empty;
      details.TextualLongitudeFrom = string.Empty;
    }

    /// Procedure to clear AFDMapData Structure
    /// </summary>
    /// <param name="details">AFDData to be cleared</param>
    public void ClearAFDMapData(ref afdMapData details) 
    {
      details.GBGridE = string.Empty;
      details.GBGridN = string.Empty;
      details.NIGridE = string.Empty;
      details.NIGridN = string.Empty;
      details.Latitude = string.Empty;
      details.Longitude = string.Empty;
      details.TextualLatitude = string.Empty;
      details.TextualLongitude = string.Empty;
      details.ImageWidth = string.Empty;
      details.ImageHeight = string.Empty;
      details.ImageScale = string.Empty;
      details.OutputType = string.Empty;
      details.Filename = string.Empty;
    }

    /// Procedure to clear AFDStringData Structure
    /// </summary>
    /// <param name="details">AFDData to be cleared</param>
    public void ClearAFDStringData(ref afdStringData details) 
    {
      details.Lookup = string.Empty;
      details.Outcode = string.Empty;
      details.Incode = string.Empty;
      details.Search = string.Empty;
      details.Replace = string.Empty;
    }

    /// Procedure to clear AFDEmailData Structure
    /// </summary>
    /// <param name="details">AFDData to be cleared</param>
    public void ClearAFDEmailData(ref afdEmailData details) 
    {
      details.Email = string.Empty;
    }

    private string CallDllFunction(string dataName, long operation, StringBuilder passDetails, out long retVal)
    {
      IntPtr tDataName;
      IntPtr tPassDetails;
      
      // Call the DLL function
      tDataName = Marshal.StringToHGlobalAnsi(dataName);
      tPassDetails = Marshal.StringToHGlobalAnsi(passDetails.ToString());
      if (IntPtr.Size == 8) 
          retVal = InternalAFDData64(tDataName, (int)operation, tPassDetails);
      else
          retVal = InternalAFDData32(tDataName, (int)operation, tPassDetails);
      return Marshal.PtrToStringAnsi(tPassDetails);
    }

  }

}
