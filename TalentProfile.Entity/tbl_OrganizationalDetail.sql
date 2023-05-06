/*	Contains detail of an organization. 
	Organization either listed as a firm/consultancy/institute or similar.
	OR Organization may have a posted job.
*/

CREATE TABLE [dbo].[tbl_OrganizationalDetail](
	[SrNo] [numeric](9, 0) NOT NULL,
	[UserId] [numeric](9, 0) NOT NULL,
	[OrganizationName] [varchar](70) NOT NULL,
	[FromDate] [smalldatetime] NULL,
	[ToDate] [smalldatetime] NULL,
	[City] [varchar](50) NOT NULL,
	[Country] [int] NOT NULL,
	[Designation] [varchar](50) NOT NULL,
	[UpdatedOn] [smalldatetime] NULL,
	[Flag] [varchar](2) NULL,
 CONSTRAINT [PK_tbl_OrganizationalDetail] PRIMARY KEY CLUSTERED 
(
	[SrNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
