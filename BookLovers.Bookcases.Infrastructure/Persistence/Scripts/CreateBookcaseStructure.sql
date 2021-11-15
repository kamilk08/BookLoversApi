USE [BookcaseContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Bookcases](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Bookcases] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[AggregateGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[InBoxMessages](
	[Guid] [uniqueidentifier] NOT NULL,
	[OccurredOn] [datetime] NOT NULL,
	[ProcessedAt] [datetime] NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Assembly] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.InBoxMessages] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[OutboxMessages](
	[Guid] [uniqueidentifier] NOT NULL,
	[OccuredAt] [datetime] NOT NULL,
	[ProcessedAt] [datetime] NULL,
	[Type] [nvarchar](max) NOT NULL,
	[Assembly] [nvarchar](max) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
	[Map] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.OutboxMessages] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Readers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[BookCaseId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Readers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SettingsManagers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[BookcaseGuid] [uniqueidentifier] NOT NULL,
	[BookcaseId] [int] NOT NULL,
	[Privacy] [int] NOT NULL,
	[Capacity] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SettingsManagers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Shelves](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ShelfName] [nvarchar](255) NOT NULL,
	[ShelfCategory] [tinyint] NOT NULL,
	[BookcaseId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Shelves] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Shelves]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Shelves_dbo.Bookcases_BookcaseId] FOREIGN KEY([BookcaseId])
REFERENCES [dbo].[Bookcases] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Shelves] CHECK CONSTRAINT [FK_dbo.Shelves_dbo.Bookcases_BookcaseId]
GO

CREATE TABLE [dbo].[ShelfRecordTrackers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ShelfRecordTrackerGuid] [uniqueidentifier] NOT NULL,
	[BookcaseGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ShelfRecordTrackers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ShelfRecords](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookRowId] [int] NOT NULL,
	[ShelfId] [int] NOT NULL,
	[AddedAt] [datetime] NULL,
 CONSTRAINT [PK_dbo.ShelfRecords] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShelfRecords]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelfRecords_dbo.Books_BookRowId] FOREIGN KEY([BookRowId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelfRecords] CHECK CONSTRAINT [FK_dbo.ShelfRecords_dbo.Books_BookRowId]
GO

ALTER TABLE [dbo].[ShelfRecords]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelfRecords_dbo.Shelves_ShelfId] FOREIGN KEY([ShelfId])
REFERENCES [dbo].[Shelves] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelfRecords] CHECK CONSTRAINT [FK_dbo.ShelfRecords_dbo.Shelves_ShelfId]
GO

CREATE TABLE [dbo].[ShelfRecordsWithTrackers](
	[ShelfRecordTrackerReadModel_Id] [int] NOT NULL,
	[ShelfRecordReadModel_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ShelfRecordsWithTrackers] PRIMARY KEY CLUSTERED 
(
	[ShelfRecordTrackerReadModel_Id] ASC,
	[ShelfRecordReadModel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShelfRecordsWithTrackers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelfRecordsWithTrackers_dbo.ShelfRecords_ShelfRecordReadModel_Id] FOREIGN KEY([ShelfRecordReadModel_Id])
REFERENCES [dbo].[ShelfRecords] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelfRecordsWithTrackers] CHECK CONSTRAINT [FK_dbo.ShelfRecordsWithTrackers_dbo.ShelfRecords_ShelfRecordReadModel_Id]
GO

ALTER TABLE [dbo].[ShelfRecordsWithTrackers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelfRecordsWithTrackers_dbo.ShelfRecordTrackers_ShelfRecordTrackerReadModel_Id] FOREIGN KEY([ShelfRecordTrackerReadModel_Id])
REFERENCES [dbo].[ShelfRecordTrackers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelfRecordsWithTrackers] CHECK CONSTRAINT [FK_dbo.ShelfRecordsWithTrackers_dbo.ShelfRecordTrackers_ShelfRecordTrackerReadModel_Id]
GO



CREATE TABLE [dbo].[ShelvesWithBooks](
	[ShelfRowId] [int] NOT NULL,
	[BookRowId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ShelvesWithBooks] PRIMARY KEY CLUSTERED 
(
	[ShelfRowId] ASC,
	[BookRowId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ShelvesWithBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelvesWithBooks_dbo.Books_BookRowId] FOREIGN KEY([BookRowId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelvesWithBooks] CHECK CONSTRAINT [FK_dbo.ShelvesWithBooks_dbo.Books_BookRowId]
GO

ALTER TABLE [dbo].[ShelvesWithBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ShelvesWithBooks_dbo.Shelves_ShelfRowId] FOREIGN KEY([ShelfRowId])
REFERENCES [dbo].[Shelves] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ShelvesWithBooks] CHECK CONSTRAINT [FK_dbo.ShelvesWithBooks_dbo.Shelves_ShelfRowId]
GO

PRINT 'Bookcase database structure created !'