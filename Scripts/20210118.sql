USE [master]
GO
/****** Object:  Database [Oqtane-202101071839]    Script Date: 1/18/2021 2:07:54 PM ******/
CREATE DATABASE [Oqtane-202101071839]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Oqtane-202101071839', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Oqtane-202101071839.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Oqtane-202101071839_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Oqtane-202101071839_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Oqtane-202101071839].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Oqtane-202101071839] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET ARITHABORT OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Oqtane-202101071839] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Oqtane-202101071839] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Oqtane-202101071839] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Oqtane-202101071839] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [Oqtane-202101071839] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET RECOVERY FULL 
GO
ALTER DATABASE [Oqtane-202101071839] SET  MULTI_USER 
GO
ALTER DATABASE [Oqtane-202101071839] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Oqtane-202101071839] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Oqtane-202101071839] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Oqtane-202101071839] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Oqtane-202101071839', N'ON'
GO
USE [Oqtane-202101071839]
GO
/****** Object:  Table [dbo].[Alias]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Alias](
	[AliasId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[TenantId] [int] NOT NULL,
	[SiteId] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Alias] PRIMARY KEY CLUSTERED 
(
	[AliasId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[File]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[File](
	[FileId] [int] IDENTITY(1,1) NOT NULL,
	[FolderId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Extension] [nvarchar](50) NOT NULL,
	[Size] [int] NOT NULL,
	[ImageHeight] [int] NOT NULL,
	[ImageWidth] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_File] PRIMARY KEY CLUSTERED 
(
	[FileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Folder]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Folder](
	[FolderId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[Path] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[ParentId] [int] NULL,
	[Order] [int] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Folder] PRIMARY KEY CLUSTERED 
(
	[FolderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentDepartment]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GearmentDepartment](
	[DepartmentId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DailyStartTime] [datetime] NOT NULL,
	[DailyEndTime] [datetime] NOT NULL,
	[BreakStartTime] [datetime] NOT NULL,
	[BreakEndTime] [datetime] NOT NULL,
	[TotalRestHour] [int] NOT NULL,
 CONSTRAINT [PK_GearmentDepartment] PRIMARY KEY CLUSTERED 
(
	[DepartmentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentEmployee]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GearmentEmployee](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[PayrollID] [int] NOT NULL,
	[Rate] [float] NOT NULL,
	[Department] [nvarchar](256) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Status] [nvarchar](256) NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_GearmentEmployee] PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentSummary]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GearmentSummary](
	[SummaryId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_GearmentSummary] PRIMARY KEY CLUSTERED 
(
	[SummaryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentTimesheet]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentTimesheetData]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GearmentTimesheetData](
	[TimesheetDataId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](256) NOT NULL,
	[LastName] [nvarchar](256) NOT NULL,
	[DayOfWeek] [nvarchar](256) NOT NULL,
	[PayRollID] [nvarchar](256) NOT NULL,
	[Date] [nvarchar](256) NOT NULL,
	[Rate] [float] NOT NULL,
	[Department] [nvarchar](256) NOT NULL,
	[Status] [nvarchar](256) NOT NULL,
	[DailyStartTime] [datetime2](7) NOT NULL,
	[DailyEndTime] [datetime2](7) NOT NULL,
	[BreakStartTime] [datetime2](7) NOT NULL,
	[BreakEndTime] [datetime2](7) NOT NULL,
	[TotalRestHour] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[TotalWorkingHour] [int] NOT NULL,
 CONSTRAINT [PK_GearmentTimesheetDataId] PRIMARY KEY CLUSTERED 
(
	[TimesheetDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GearmentTimesheetFilter]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GearmentTimesheetFilter](
	[Year] [int] NOT NULL,
	[Month] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HtmlText]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HtmlText](
	[HtmlTextId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_HtmlText] PRIMARY KEY CLUSTERED 
(
	[HtmlTextId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Job]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Job](
	[JobId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[JobType] [nvarchar](200) NOT NULL,
	[Frequency] [char](1) NOT NULL,
	[Interval] [int] NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsStarted] [bit] NOT NULL,
	[IsExecuting] [bit] NOT NULL,
	[NextExecution] [datetime] NULL,
	[RetentionHistory] [int] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[JobLog]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobLog](
	[JobLogId] [int] IDENTITY(1,1) NOT NULL,
	[JobId] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[FinishDate] [datetime] NULL,
	[Succeeded] [bit] NULL,
	[Notes] [nvarchar](max) NULL,
 CONSTRAINT [PK_JobLog] PRIMARY KEY CLUSTERED 
(
	[JobLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NULL,
	[LogDate] [datetime] NOT NULL,
	[PageId] [int] NULL,
	[ModuleId] [int] NULL,
	[UserId] [int] NULL,
	[Url] [nvarchar](2048) NOT NULL,
	[Server] [nvarchar](200) NOT NULL,
	[Category] [nvarchar](200) NOT NULL,
	[Feature] [nvarchar](200) NOT NULL,
	[Function] [nvarchar](20) NOT NULL,
	[Level] [nvarchar](20) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[MessageTemplate] [nvarchar](max) NOT NULL,
	[Exception] [nvarchar](max) NULL,
	[Properties] [nvarchar](max) NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Module]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Module](
	[ModuleId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[ModuleDefinitionName] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[AllPages] [bit] NULL,
 CONSTRAINT [PK_Module] PRIMARY KEY CLUSTERED 
(
	[ModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModuleDefinition]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModuleDefinition](
	[ModuleDefinitionId] [int] IDENTITY(1,1) NOT NULL,
	[ModuleDefinitionName] [nvarchar](200) NOT NULL,
	[Name] [nvarchar](200) NULL,
	[Description] [nvarchar](2000) NULL,
	[Categories] [nvarchar](200) NULL,
	[Version] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_ModuleDefinition] PRIMARY KEY CLUSTERED 
(
	[ModuleDefinitionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[FromUserId] [int] NULL,
	[ToUserId] [int] NULL,
	[ToEmail] [nvarchar](256) NOT NULL,
	[Subject] [nvarchar](256) NOT NULL,
	[Body] [nvarchar](max) NOT NULL,
	[ParentId] [int] NULL,
	[CreatedOn] [datetime] NOT NULL,
	[IsDelivered] [bit] NOT NULL,
	[DeliveredOn] [datetime] NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[FromDisplayName] [nvarchar](50) NULL,
	[FromEmail] [nvarchar](256) NULL,
	[ToDisplayName] [nvarchar](50) NULL,
	[SendOn] [datetime] NULL,
 CONSTRAINT [PK_Notification] PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Page]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Page](
	[PageId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[Path] [nvarchar](256) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](200) NULL,
	[ThemeType] [nvarchar](200) NULL,
	[Icon] [nvarchar](50) NOT NULL,
	[ParentId] [int] NULL,
	[Order] [int] NOT NULL,
	[IsNavigation] [bit] NOT NULL,
	[Url] [nvarchar](500) NULL,
	[LayoutType] [nvarchar](200) NOT NULL,
	[UserId] [int] NULL,
	[IsPersonalizable] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DefaultContainerType] [nvarchar](200) NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED 
(
	[PageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PageModule]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PageModule](
	[PageModuleId] [int] IDENTITY(1,1) NOT NULL,
	[PageId] [int] NOT NULL,
	[ModuleId] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Pane] [nvarchar](50) NOT NULL,
	[Order] [int] NOT NULL,
	[ContainerType] [nvarchar](200) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_PageModule] PRIMARY KEY CLUSTERED 
(
	[PageModuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Permission]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permission](
	[PermissionId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NOT NULL,
	[EntityName] [nvarchar](50) NOT NULL,
	[EntityId] [int] NOT NULL,
	[PermissionName] [nvarchar](50) NOT NULL,
	[RoleId] [int] NULL,
	[UserId] [int] NULL,
	[IsAuthorized] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Permission] PRIMARY KEY CLUSTERED 
(
	[PermissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Profile]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Profile](
	[ProfileId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Category] [nvarchar](50) NOT NULL,
	[ViewOrder] [int] NOT NULL,
	[MaxLength] [int] NOT NULL,
	[DefaultValue] [nvarchar](2000) NULL,
	[IsRequired] [bit] NOT NULL,
	[IsPrivate] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[Options] [nvarchar](2000) NULL,
 CONSTRAINT [PK_Profile] PRIMARY KEY CLUSTERED 
(
	[ProfileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[SiteId] [int] NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [varchar](256) NOT NULL,
	[IsAutoAssigned] [bit] NOT NULL,
	[IsSystem] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchemaVersions]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchemaVersions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ScriptName] [nvarchar](255) NOT NULL,
	[Applied] [datetime] NOT NULL,
 CONSTRAINT [PK_SchemaVersions_Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Setting]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Setting](
	[SettingId] [int] IDENTITY(1,1) NOT NULL,
	[EntityName] [nvarchar](50) NOT NULL,
	[EntityId] [int] NOT NULL,
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED 
(
	[SettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Site]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Site](
	[SiteId] [int] IDENTITY(1,1) NOT NULL,
	[TenantId] [int] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[LogoFileId] [int] NULL,
	[FaviconFileId] [int] NULL,
	[DefaultThemeType] [nvarchar](200) NOT NULL,
	[DefaultLayoutType] [nvarchar](200) NOT NULL,
	[DefaultContainerType] [nvarchar](200) NOT NULL,
	[PwaIsEnabled] [bit] NOT NULL,
	[PwaAppIconFileId] [int] NULL,
	[PwaSplashIconFileId] [int] NULL,
	[AllowRegistration] [bit] NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_Site] PRIMARY KEY CLUSTERED 
(
	[SiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tenant]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenant](
	[TenantId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DBConnectionString] [nvarchar](1024) NOT NULL,
	[Version] [nvarchar](50) NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
(
	[TenantId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](256) NOT NULL,
	[DisplayName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](256) NOT NULL,
	[PhotoFileId] [int] NULL,
	[LastLoginOn] [datetime] NULL,
	[LastIPAddress] [nvarchar](50) NOT NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
	[DeletedBy] [nvarchar](256) NULL,
	[DeletedOn] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 1/18/2021 2:07:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserRoleId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[EffectiveDate] [datetime] NULL,
	[ExpiryDate] [datetime] NULL,
	[CreatedBy] [nvarchar](256) NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](256) NOT NULL,
	[ModifiedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Alias]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Alias] ON [dbo].[Alias]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_AspNetUserClaims_UserId]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] ON [dbo].[AspNetUserClaims]
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [EmailIndex]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE NONCLUSTERED INDEX [EmailIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedEmail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UserNameIndex]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] ON [dbo].[AspNetUsers]
(
	[NormalizedUserName] ASC
)
WHERE ([NormalizedUserName] IS NOT NULL)
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_File]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_File] ON [dbo].[File]
(
	[FolderId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Folder]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Folder] ON [dbo].[Folder]
(
	[SiteId] ASC,
	[Path] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Job]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Job] ON [dbo].[Job]
(
	[JobType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_ModuleDefinition]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ModuleDefinition] ON [dbo].[ModuleDefinition]
(
	[ModuleDefinitionName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Page]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Page] ON [dbo].[Page]
(
	[SiteId] ASC,
	[Path] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Permission]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Permission] ON [dbo].[Permission]
(
	[SiteId] ASC,
	[EntityName] ASC,
	[EntityId] ASC,
	[PermissionName] ASC,
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Profile]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Profile] ON [dbo].[Profile]
(
	[SiteId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Role]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Role] ON [dbo].[Role]
(
	[SiteId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Setting]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Setting] ON [dbo].[Setting]
(
	[EntityName] ASC,
	[EntityId] ASC,
	[SettingName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Site]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Site] ON [dbo].[Site]
(
	[TenantId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Tenant]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Tenant] ON [dbo].[Tenant]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_User]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_User] ON [dbo].[User]
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserRole]    Script Date: 1/18/2021 2:07:55 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_UserRole] ON [dbo].[UserRole]
(
	[RoleId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GearmentDepartment] ADD  DEFAULT (getdate()) FOR [DailyStartTime]
GO
ALTER TABLE [dbo].[GearmentDepartment] ADD  DEFAULT (getdate()) FOR [DailyEndTime]
GO
ALTER TABLE [dbo].[GearmentDepartment] ADD  DEFAULT (getdate()) FOR [BreakStartTime]
GO
ALTER TABLE [dbo].[GearmentDepartment] ADD  DEFAULT (getdate()) FOR [BreakEndTime]
GO
ALTER TABLE [dbo].[GearmentDepartment] ADD  DEFAULT ((0)) FOR [TotalRestHour]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT ((0)) FOR [PayrollID]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT ('') FOR [Department]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT (getdate()) FOR [StartDate]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT ('') FOR [Status]
GO
ALTER TABLE [dbo].[GearmentEmployee] ADD  DEFAULT ('') FOR [Note]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [FirstName]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [LastName]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [DayOfWeek]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [PayRollID]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [Date]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ((0)) FOR [Rate]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [Department]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [Status]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [DailyStartTime]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [DailyEndTime]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [BreakStartTime]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [BreakEndTime]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ((0)) FOR [TotalRestHour]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [CreatedBy]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ('') FOR [ModifiedBy]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT (getdate()) FOR [ModifiedOn]
GO
ALTER TABLE [dbo].[GearmentTimesheetData] ADD  DEFAULT ((0)) FOR [TotalWorkingHour]
GO
ALTER TABLE [dbo].[GearmentTimesheetFilter] ADD  DEFAULT ('') FOR [Year]
GO
ALTER TABLE [dbo].[GearmentTimesheetFilter] ADD  DEFAULT ('') FOR [Month]
GO
ALTER TABLE [dbo].[Alias]  WITH CHECK ADD  CONSTRAINT [FK_Alias_Tenant] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([TenantId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Alias] CHECK CONSTRAINT [FK_Alias_Tenant]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[File]  WITH CHECK ADD  CONSTRAINT [FK_File_Folder] FOREIGN KEY([FolderId])
REFERENCES [dbo].[Folder] ([FolderId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[File] CHECK CONSTRAINT [FK_File_Folder]
GO
ALTER TABLE [dbo].[Folder]  WITH CHECK ADD  CONSTRAINT [FK_Folder_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Folder] CHECK CONSTRAINT [FK_Folder_Site]
GO
ALTER TABLE [dbo].[GearmentDepartment]  WITH CHECK ADD  CONSTRAINT [FK_GearmentDepartment_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GearmentDepartment] CHECK CONSTRAINT [FK_GearmentDepartment_Module]
GO
ALTER TABLE [dbo].[GearmentEmployee]  WITH CHECK ADD  CONSTRAINT [FK_GearmentEmployee_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GearmentEmployee] CHECK CONSTRAINT [FK_GearmentEmployee_Module]
GO
ALTER TABLE [dbo].[GearmentSummary]  WITH CHECK ADD  CONSTRAINT [FK_GearmentSummary_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GearmentSummary] CHECK CONSTRAINT [FK_GearmentSummary_Module]
GO
ALTER TABLE [dbo].[GearmentTimesheet]  WITH CHECK ADD  CONSTRAINT [FK_GearmentTimesheet_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GearmentTimesheet] CHECK CONSTRAINT [FK_GearmentTimesheet_Module]
GO
ALTER TABLE [dbo].[HtmlText]  WITH CHECK ADD  CONSTRAINT [FK_HtmlText_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[HtmlText] CHECK CONSTRAINT [FK_HtmlText_Module]
GO
ALTER TABLE [dbo].[JobLog]  WITH NOCHECK ADD  CONSTRAINT [FK_JobLog_Job] FOREIGN KEY([JobId])
REFERENCES [dbo].[Job] ([JobId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[JobLog] CHECK CONSTRAINT [FK_JobLog_Job]
GO
ALTER TABLE [dbo].[Log]  WITH CHECK ADD  CONSTRAINT [FK_Log_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Log] CHECK CONSTRAINT [FK_Log_Site]
GO
ALTER TABLE [dbo].[Module]  WITH CHECK ADD  CONSTRAINT [FK_Module_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Module] CHECK CONSTRAINT [FK_Module_Site]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_Site]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Site]
GO
ALTER TABLE [dbo].[PageModule]  WITH CHECK ADD  CONSTRAINT [FK_PageModule_Module] FOREIGN KEY([ModuleId])
REFERENCES [dbo].[Module] ([ModuleId])
GO
ALTER TABLE [dbo].[PageModule] CHECK CONSTRAINT [FK_PageModule_Module]
GO
ALTER TABLE [dbo].[PageModule]  WITH CHECK ADD  CONSTRAINT [FK_PageModule_Page] FOREIGN KEY([PageId])
REFERENCES [dbo].[Page] ([PageId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PageModule] CHECK CONSTRAINT [FK_PageModule_Page]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK_Permission_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK_Permission_Role]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK_Permission_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK_Permission_Site]
GO
ALTER TABLE [dbo].[Permission]  WITH CHECK ADD  CONSTRAINT [FK_Permission_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Permission] CHECK CONSTRAINT [FK_Permission_User]
GO
ALTER TABLE [dbo].[Profile]  WITH NOCHECK ADD  CONSTRAINT [FK_Profile_Sites] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Profile] CHECK CONSTRAINT [FK_Profile_Sites]
GO
ALTER TABLE [dbo].[Role]  WITH CHECK ADD  CONSTRAINT [FK_Role_Site] FOREIGN KEY([SiteId])
REFERENCES [dbo].[Site] ([SiteId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Role] CHECK CONSTRAINT [FK_Role_Site]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[Role] ([RoleId])
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_Role]
GO
ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD  CONSTRAINT [FK_UserRole_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRole] CHECK CONSTRAINT [FK_UserRole_User]
GO
USE [master]
GO
ALTER DATABASE [Oqtane-202101071839] SET  READ_WRITE 
GO
