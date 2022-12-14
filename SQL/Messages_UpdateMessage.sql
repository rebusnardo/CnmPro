USE [CnmPro]
GO
/****** Object:  StoredProcedure [dbo].[Messages_Update]    Script Date: 9/1/2022 4:41:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROC [dbo].[Messages_Update]
	   
	  @Message   nvarchar(1000)
      ,@Subject nvarchar (100)
      ,@RecipientId  int
      ,@SenderId int
      ,@DateSent  datetime2(7)
      ,@DateRead datetime2(7)
	  ,@Id int
     

/*-------------TEST CODE---------------


DECLARE  @Id int = 2;

DECLARE
		@Message   nvarchar(1000) = 'Update2'
      ,@Subject nvarchar (100) ='Morning Message'
      ,@RecipientId  int = 2
      ,@SenderId int = 2
      ,@DateSent  datetime2 = '2022-07-31'
      ,@DateRead datetime2 ='2022-07-31'

SELECT * FROM [dbo].[Messages]

WHERE Id = @Id

EXECUTE [dbo].[Messages_Update]

			@Message
			,@Subject
			,@RecipientId
			,@SenderId
			,@DateSent
			,@DateRead
			,@Id 

SELECT * FROM [dbo].[Messages]

	WHERE Id = @Id
 
-------------TEST CODE---------------*/

AS

BEGIN

Declare @dateNow datetime2 = GETUTCDATE();

UPDATE [dbo].[Messages]

   SET [Message] = @Message
      ,[Subject] = @Subject
      ,[RecipientId] = @SenderId
      ,[SenderId] = @SenderId
      ,[DateSent] = @DateSent
      ,[DateRead] = @DateRead
      ,[DateModified] = @dateNow
     
 WHERE Id  = @Id

END