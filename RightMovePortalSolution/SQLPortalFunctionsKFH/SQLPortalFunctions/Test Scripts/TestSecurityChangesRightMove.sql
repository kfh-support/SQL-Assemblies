/*
   Force Property HTTP Update Script for Single Records
*/

USE [OpenHousePortals]--OpenHousePortalsDevelopment

-- EXEC [uspWebUpdateProperties] 6

SET NOCOUNT ON
GO
--1. Find the Latest Records

		UPDATE PropertyInclude SET Processing = 1 WHERE WebPortalID = 6 AND IncludeDate < GETDATE()

--2. Manually Call the CLR Procedure:
-- New:
DROP TABLE #TempProperty

DECLARE @PropertyReference INT = 	(
		Select TOP 1 PropertyReference 
		from PropertyInclude
		where Processing = 1
		and WebPortalID = 6
	)
--SELECT @PropertyReference

	SELECT WebPortalID, PropertyReference, UpdateDate, InsertionDate, DeletionDate, OutputResults.Success, XMLToPost, OutputResults.XMLResponse, OutputResults.AdditionalInfo
	INTO #TempProperty
	FROM 
	(	
		SELECT 
			PIn.WebPortalID, 
			PIn.PropertyReference, 
			UpdateDate = GETDATE(), 
			InsertionDate = CASE WHEN PIn.UpdateType = 'U' AND P.InsertionDate IS NULL THEN GETDATE() ELSE P.InsertionDate END,
			DeletionDate = CASE WHEN PIn.UpdateType = 'D' AND P.DeletionDate IS NULL THEN GETDATE() WHEN PIn.UpdateType = 'U' THEN NULL ELSE P.DeletionDate END,
			PIn.XMLToPost,
			PIn.UpdateType,
			UpdatePropertyURL,
			RemovePropertyURL,
			UpdatePropertyXSDPath,
			RemovePropertyXSDPath,
			P12CertificateLocation,
			P12CertificatePassword
		FROM PropertyInclude PIn WITH (NOLOCK)
		INNER JOIN WebPortal WP WITH (NOLOCK) ON PIn.WebPortalID = WP.WebPortalID
		LEFT JOIN Property P WITH (NOLOCK) ON PIn.WebPortalID = P.WebPortalID AND PIn.PropertyReference = P.PropertyReference
		where P.PropertyReference = @PropertyReference
		and Processing = 1 AND WP.WebPortalID = 6

	--	and p.PropertyReference = 2160100
	) AS InputData
OUTER APPLY OpenHousePortals.dbo.fnWebSendXMLData
	(
		CASE WHEN UpdateType = 'U' THEN UpdatePropertyURL ELSE RemovePropertyURL END, 
		'POST', 'application/xml', XMLToPost, P12CertificateLocation, P12CertificatePassword, 30, 
		CASE WHEN UpdateType = 'U' THEN UpdatePropertyXSDPath ELSE RemovePropertyXSDPath END, 
		NULL, '/*/success[1]', 'true', 0, 
		CASE WHEN WebPortalID = 2 THEN '/*/message[1]' ELSE '/*/*/rightmove_id[1]' END 
	) OutputResults 
--WHERE PropertyReference = @PropertyReference


--4. Check Results:

--5. Update Record IN underlying source tables

UPDATE Property SET 
		UpdateDate = #TempProperty.UpdateDate, 
		InsertionDate = #TempProperty.InsertionDate, 
		DeletionDate = #TempProperty.DeletionDate, 
		Success = #TempProperty.Success, 
		LastPostXML = #TempProperty.XMLToPost, 
		ResponseXML = #TempProperty.XMLResponse, 
		ErrorMessage = #TempProperty.AdditionalInfo
	FROM Property
	INNER JOIN #TempProperty ON Property.WebPortalID = #TempProperty.WebPortalID 
	AND Property.PropertyReference = #TempProperty.PropertyReference


-- Step 6: Delete all 'Processing' properties
	DELETE FROM PropertyInclude WHERE Processing = 1 AND WebPortalID = 6 and PropertyReference = @PropertyReference
    

SELECT *
from #TempProperty

