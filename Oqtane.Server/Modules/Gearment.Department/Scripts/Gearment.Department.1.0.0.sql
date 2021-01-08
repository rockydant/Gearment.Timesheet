/*  
Create GearmentDepartment table
*/

CREATE TABLE [dbo].[GearmentDepartment](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentDepartment] PRIMARY KEY CLUSTERED 
  (
	[DepartmentId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentDepartment]  WITH CHECK ADD  CONSTRAINT [FK_GearmentDepartment_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO