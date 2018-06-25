DECLARE @PostCode AS VARCHAR(10)
DECLARE @Street AS VARCHAR(100)
DECLARE @Town	AS VARCHAR(100)
DECLARE @County AS VARCHAR(100)
DECLARE @Errors AS VARCHAR(200)

SET @PostCode = 'B18 5AP'
SET @Street = ''
SET @Town = ''
SET @County = ''
SET @Errors = ''

EXEC SearchForPostCode_Test
	@PostCode,
	@PostCode OUTPUT,
	@Street OUTPUT,
	@Town OUTPUT,	
	@County OUTPUT,
	@Errors OUTPUT 

SELECT 
	@PostCode,
	@Street,
	@Town,	
	@County,
	@Errors