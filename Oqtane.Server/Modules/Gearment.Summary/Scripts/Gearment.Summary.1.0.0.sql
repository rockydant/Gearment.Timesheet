/*  
Create GearmentSummary table
*/

CREATE TABLE [dbo].[GearmentSummary](
	[SummaryId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentSummary] PRIMARY KEY CLUSTERED 
  (
	[SummaryId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentSummary]  WITH CHECK ADD  CONSTRAINT [FK_GearmentSummary_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO