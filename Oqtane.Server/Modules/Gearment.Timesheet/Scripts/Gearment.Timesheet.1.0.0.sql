/*  
Create GearmentTimesheet table
*/

CREATE TABLE [dbo].[GearmentTimesheet](
	[TimesheetId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentTimesheet] PRIMARY KEY CLUSTERED 
  (
	[TimesheetId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentTimesheet]  WITH CHECK ADD  CONSTRAINT [FK_GearmentTimesheet_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO