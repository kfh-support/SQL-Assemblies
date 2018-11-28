/*
   Test Script for manually testing the LonRes  Clr functions:

   /*
      DCSQL01 -- Live test:

	  	use [Development]
	go
	select * from sys.assembly_modules
	select name,* from sys.syslogins where name like '%KFH%'


   */


   -- CLR Stub:

   ALTER PROCEDURE [dbo].[CreateCLEAFileLonRes]
	@nUploadType [int],
	@strDirectoryName [nvarchar](max)
    WITH EXECUTE AS CALLER
   AS
    EXTERNAL NAME [StoredProcedureLonRes].[lonres.StoredProcedures].[CreateCLEAFileLonRes]
   GO


   Execute:


   Step 4: Step 4: Create text file

	EXEC [dbo].[CreateCLEAFileLonRes] 20, 'E:\KFH\Webuploads\CLEA\'

*/

/*
    SQL Side  Wrapper CLR procedure:

	ALTER PROCEDURE [dbo].[CreateCLEAFileLonRes]
	@nUploadType [int],
	@Destination [nvarchar](max)
	WITH EXECUTE AS CALLER
	AS
	EXTERNAL NAME [StoredProcedureLonRes].[lonres.StoredProcedures].[CreateCLEAFileLonRes]
	GO

*/
/*
    Configuration:
	
	USE Master

   -- 1.
    CREATE ASYMMETRIC KEY KFHExternalAssemblyKeyLonRes
	FROM FILE = 'c:\kfh\assemblies\KFHExternalAccessLonRes.snk';
	GO

   -- 2
	CREATE LOGIN KFHExternalAssemblyLoginLonRes
	FROM ASYMMETRIC KEY KFHExternalAssemblyKeyLonRes;
	Go
	
   -- 3.
   -- GRANT EXTERNAL ACCESS
   
      GRANT EXTERNAL ACCESS ASSEMBLY TO KFHExternalAssemblyLoginLResLocal;

   
    -- Set the Database Settings:

	use [Development]
	go 
	sp_configure 'clr enabled', 1
	GO
	alter database [OpenHousePortals] SET TRUSTWORTHY ON;
	go
	RECONFIGURE 
	GO


   -- Check CLR configrations: 

	use [Development]
	go
	select * from sys.assembly_modules
	select name,* from sys.syslogins where name like '%KFH%'

*/