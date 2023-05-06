-- Store all user's - talent settings 
CREATE TABLE tbl_Settings
(
	UId numeric (8,0) not null identity (1,1),
	Newsletters bit default 0,
	JobAlert bit default 0,
	JobAlertFromFollowedComp bit default 0,
	SiteChangeAlert bit default 1,
	ArticleNotify bit default 1,
	DateUpdated smalldatetime default getdate(), 
    CONSTRAINT [PK_tbl_Settings] PRIMARY KEY ([UId]),
)