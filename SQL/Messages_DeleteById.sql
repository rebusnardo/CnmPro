USE [CnmPro]
GO
/****** Object:  StoredProcedure [dbo].[Messages_DeleteById]    Script Date: 9/1/2022 4:11:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROC [dbo].[Messages_DeleteById]
		@Id int

/*---------Start Test Code-------------

DECLARE @Id int = 5

SELECT * FROM [dbo].[Messages]

WHERE Id = @Id;

EXECUTE [dbo].[Messages_DeleteById] @Id

SELECT * FROM [dbo].[Messages]

WHERE Id = @Id;

---------End Test Code-------------*/

AS

BEGIN

DELETE FROM [dbo].[Messages]
      WHERE Id = @Id;

END