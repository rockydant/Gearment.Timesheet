ALTER TABLE [dbo].[GearmentEmployee]
ADD [PayrollID] [int] NOT NULL DEFAULT 0,
[Rate] [float] NOT NULL DEFAULT 0,
[Department] [nvarchar](256) NOT NULL DEFAULT '',
[StartDate] [datetime] NOT NULL DEFAULT GETDATE(),
[Status] [nvarchar](256) NOT NULL DEFAULT '',
[Note] [nvarchar](MAX) NOT NULL DEFAULT ''
