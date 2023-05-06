/* Contains registered employee's detail, who has signedup on behalf of an organization.*/

CREATE TABLE [dbo].[tbl_EmployerDetail](
	[Id] [numeric](9, 0) NOT NULL,
	[OrganizationName] [varchar](60) NULL,
	[AuthPerson] [varchar](50) NULL,
	[AuthPerPhone] [varchar](11) NULL,
	[AuthPerDesignation] [varchar](30) NULL,
	[IndustryType] [varchar](30) NULL,
	[Website] [varchar](60) NULL,
	[Address] [varchar](300) NULL,
	[City] [varchar](40) NULL,
	[Country] [int] NULL,
	[CompLogo] [varchar](50) NULL,
	[CompProfile] [text] NULL,
	[Strength] [varchar](50) NULL,
	[EmpUserId] [numeric](9, 0) NULL,
	[ActiveDate] [smalldatetime] NULL,
	[DomainId] [int] NULL,
 CONSTRAINT [PK_tbl_EmployerDetail_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tbl_EmployerDetail]  WITH CHECK ADD  CONSTRAINT [FK_Id_LookupIndustryDomains] FOREIGN KEY([DomainId])
REFERENCES [dbo].[tbl_LookupIndustryDomains] ([DomainId])
GO

ALTER TABLE [dbo].[tbl_EmployerDetail] CHECK CONSTRAINT [FK_Id_LookupIndustryDomains]
GO