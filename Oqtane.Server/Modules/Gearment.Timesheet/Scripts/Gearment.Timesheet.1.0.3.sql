DROP TABLE IF EXISTS GearmentTimesheetData

CREATE TABLE [dbo].[GearmentTimesheetData](	
    [TimesheetDataId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL DEFAULT '',
	[LastName] [nvarchar](256) NOT NULL DEFAULT '',
	[DayOfWeek] [nvarchar](256) NOT NULL DEFAULT '',
	[PayRollID] [nvarchar](256) NOT NULL DEFAULT '',
	[Date] [nvarchar](256) NOT NULL DEFAULT '',
    [Rate] [float] NOT NULL DEFAULT 0,
    [Department] [nvarchar](256) NOT NULL DEFAULT '',
    [Status] [nvarchar](256) NOT NULL DEFAULT '',
    [DailyStartTime] [datetime] NOT NULL DEFAULT GETDATE(),
    [DailyEndTime] [datetime] NOT NULL DEFAULT GETDATE(),
    [BreakStartTime] [datetime] NOT NULL DEFAULT GETDATE(),
    [BreakEndTime] [datetime] NOT NULL DEFAULT GETDATE(),
    [TotalRestHour] [int] NOT NULL DEFAULT 0,
     CONSTRAINT [PK_GearmentTimesheetDataId] PRIMARY KEY CLUSTERED 
  (
	[TimesheetDataId] ASC
  )
)
GO
