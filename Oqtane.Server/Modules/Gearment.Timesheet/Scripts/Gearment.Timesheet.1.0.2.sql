DROP TABLE IF EXISTS GearmentTimesheet

CREATE TABLE [dbo].[GearmentTimesheet](
	[TimesheetId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[DayOfWeek] [nvarchar](256) NOT NULL,
	[PayRollID] [nvarchar](256) NOT NULL,
	[Date] [nvarchar](256) NOT NULL,
	[In] [nvarchar](256) NOT NULL,
	[Out] [nvarchar](256) NOT NULL,
	[Hours] [nvarchar](256) NOT NULL,
	[Type] [nvarchar](256) NOT NULL,

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