/*  
Create GearmentEmployeeSchedule table
*/

CREATE TABLE [dbo].[GearmentEmployeeSchedule](
	[EmployeeScheduleId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentEmployeeSchedule] PRIMARY KEY CLUSTERED 
  (
	[EmployeeScheduleId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentEmployeeSchedule]  WITH CHECK ADD  CONSTRAINT [FK_GearmentEmployeeSchedule_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO