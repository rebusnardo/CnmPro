USE [CnmPro]
GO
/****** Object:  StoredProcedure [dbo].[Messages_Select_ByRece]    Script Date: 9/1/2022 4:34:24 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROC [dbo].[Messages_Select_ByRece]
		@RecipientId int
		,@PageIndex int
		,@PageSize int


/*--------TEST CODE---------

SELECT * FROM [dbo].[Messages]

DECLARE				@RecipientId int = 152
					,@PageIndex int = 0
					,@PageSize int = 10


EXECUTE [dbo].[Messages_Select_ByRece] 
					@RecipientId
					,@PageIndex 
					,@PageSize 


--------TEST CODE---------*/

AS

BEGIN

DECLARE @offset int = @PageIndex * @PageSize

SELECT m.Id
      ,m.Message
      ,m.Subject
      ,m.RecipientId
      ,m.SenderId
      ,m.DateSent
      ,m.DateRead
      ,m.DateModified
      ,m.DateCreated
	  ,SenderData = 
		(
		SELECT TOP 1
			up.FirstName
			,up.LastName
			,up.Mi
			,up.AvatarUrl
		FROM dbo.userProfiles up
		WHERE up.UserId = m.RecipientId
		FOR JSON AUTO
		)

	  ,RecipientData = (
			SELECT TOP 1
				up.FirstName
				,up.LastName
				,up.Mi
				,up.AvatarUrl
				
			FROM dbo.userProfiles up
			WHERE up.UserId = m.SenderId
			FOR JSON AUTO
						)
	,TotalCount = COUNT(1) OVER()

FROM [dbo].[Messages] as m
inner join dbo.userProfiles as u on m.SenderId = u.UserId

Where m.RecipientId = @RecipientId

ORDER BY m.Id Desc
OFFSET @offSet Rows
Fetch Next @PageSize Rows ONLY

END