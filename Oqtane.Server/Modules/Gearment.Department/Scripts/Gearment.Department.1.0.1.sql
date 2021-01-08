ALTER TABLE [dbo].[GearmentDepartment]
ADD [DailyStartTime] [datetime] NOT NULL DEFAULT GETDATE(),
[DailyEndTime] [datetime] NOT NULL DEFAULT GETDATE(),
[BreakStartTime] [datetime] NOT NULL DEFAULT GETDATE(),
[BreakEndTime] [datetime] NOT NULL DEFAULT GETDATE(),
[TotalRestHour] [int] NOT NULL DEFAULT 0