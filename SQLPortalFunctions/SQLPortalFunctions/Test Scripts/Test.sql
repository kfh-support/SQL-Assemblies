SELECT top 1 InputData.*, OutputResults.XMLPosted, OutputResults.XMLResponse, OutputResults.Success, OutputResults.AdditionalInfo FROM 
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
	FROM PropertyInclude PIn
	INNER JOIN WebPortal WP ON PIn.WebPortalID = WP.WebPortalID
	LEFT JOIN Property P ON PIn.WebPortalID = P.WebPortalID AND PIn.PropertyReference = P.PropertyReference
) AS InputData
OUTER APPLY OpenHousePortals.dbo.fnWebSendXMLData
(
	CASE WHEN UpdateType = 'U' THEN UpdatePropertyURL ELSE RemovePropertyURL END, 
	'POST', 'application/xml', XMLToPost, P12CertificateLocation, P12CertificatePassword, 30, 
	CASE WHEN UpdateType = 'U' THEN UpdatePropertyXSDPath ELSE RemovePropertyXSDPath END, 
	NULL, '/*/success[1]', 'true', 0, 
	CASE WHEN WebPortalID = 2 THEN '/*/message[1]' ELSE '/*/*/rightmove_id[1]' END 
) OutputResults 
WHERE WebPortalID = 1 AND PropertyReference = 2178221