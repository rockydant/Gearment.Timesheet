ALTER TABLE [GearmentTimesheetData]
ADD [CreatedBy] [nvarchar](256) NOT NULL DEFAULT '',
	[CreatedOn] [datetime] NOT NULL DEFAULT GETDATE(),
	[ModifiedBy] [nvarchar](256) NOT NULL DEFAULT '',
	[ModifiedOn] [datetime] NOT NULL DEFAULT GETDATE(),
    [TotalWorkingHour] [int] NOT NULL DEFAULT 0