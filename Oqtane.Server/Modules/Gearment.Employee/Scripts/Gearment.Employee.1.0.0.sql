/*  
Create GearmentEmployee table
*/

CREATE TABLE [dbo].[GearmentEmployee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentEmployee] PRIMARY KEY CLUSTERED 
  (
	[EmployeeId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentEmployee]  WITH CHECK ADD  CONSTRAINT [FK_GearmentEmployee_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO