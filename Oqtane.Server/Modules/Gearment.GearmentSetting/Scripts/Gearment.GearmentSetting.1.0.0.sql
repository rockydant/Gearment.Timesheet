/*  
Create GearmentGearmentSetting table
*/

CREATE TABLE [dbo].[GearmentGearmentSetting](
	[GearmentSettingId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
  CONSTRAINT [PK_GearmentGearmentSetting] PRIMARY KEY CLUSTERED 
  (
	[GearmentSettingId] ASC
  )
)
GO

/*  
Create foreign key relationships
*/
ALTER TABLE [dbo].[GearmentGearmentSetting]  WITH CHECK ADD  CONSTRAINT [FK_GearmentGearmentSetting_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].Module ([ModuleId])
ON DELETE CASCADE
GO