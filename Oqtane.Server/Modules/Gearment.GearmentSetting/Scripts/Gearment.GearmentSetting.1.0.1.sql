ALTER TABLE [dbo].[GearmentGearmentSetting]
ADD [Value] [nvarchar](MAX) NOT NULL DEFAULT ''

SET IDENTITY_INSERT [dbo].[GearmentGearmentSetting] ON 
GO
INSERT [dbo].[GearmentGearmentSetting] ([GearmentSettingId], [ModuleId], [Name], [CreatedBy], [CreatedOn], [ModifiedBy], [ModifiedOn], [Value]) VALUES (1, 1010, N'logs-image-url', N'host', CAST(N'2021-06-24T16:52:24.487' AS DateTime), N'host', CAST(N'2021-06-24T17:06:03.297' AS DateTime), N'http://gmreko.s3.amazonaws.com/log-images/')
GO
SET IDENTITY_INSERT [dbo].[GearmentGearmentSetting] OFF
GO

