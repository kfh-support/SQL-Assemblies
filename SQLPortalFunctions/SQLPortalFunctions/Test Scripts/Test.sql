DECLARE @PropertyReference INT = (SELECT TOP 1 PropertyReference FROM Development.dbo.Property WHERE PropertyStatus = 2 AND OnWebsite = 1 AND 
NOT EXISTS (SELECT 1 FROM Development.dbo.PropertyCoordinates WHERE PropertyReference = Property.PropertyReference))

INSERT INTO Development.dbo.PropertyCoordinates VALUES(@PropertyReference, '', '')

DELETE FROM OpenHousePortals.dbo.PropertyInclude

UPDATE Development.dbo.Property SET EditDate = GETDATE() WHERE PropertyReference = @PropertyReference

EXEC OpenHousePortals.dbo.uspWebUpdateProperties

DELETE FROM Development.dbo.PropertyCoordinates WHERE PropertyReference = @PropertyReference