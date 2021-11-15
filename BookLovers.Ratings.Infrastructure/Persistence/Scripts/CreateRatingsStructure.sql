USE [RatingsContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[AuthorGuid] [uniqueidentifier] NOT NULL,
	[AuthorId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Average] [float] NOT NULL,
	[RatingsCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Readers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Readers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Publishers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[PublisherGuid] [uniqueidentifier] NOT NULL,
	[PublisherId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Average] [float] NOT NULL,
	[RatingsCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Publishers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PublisherCycles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[PublisherCycleGuid] [uniqueidentifier] NOT NULL,
	[PublisherCycleId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Average] [float] NOT NULL,
	[RatingsCount] [int] NOT NULL,
	[PublisherReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.PublisherCycles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[PublisherCycles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.PublisherCycles_dbo.Publishers_PublisherReadModel_Id] FOREIGN KEY([PublisherReadModel_Id])
REFERENCES [dbo].[Publishers] ([Id])
GO

ALTER TABLE [dbo].[PublisherCycles] CHECK CONSTRAINT [FK_dbo.PublisherCycles_dbo.Publishers_PublisherReadModel_Id]
GO

CREATE TABLE [dbo].[Series](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[SeriesGuid] [uniqueidentifier] NOT NULL,
	[SeriesId] [int] NOT NULL,
	[Average] [float] NOT NULL,
	[RatingsCount] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Series] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[BookGuid] [uniqueidentifier] NOT NULL,
	[BookId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Average] [float] NOT NULL,
	[RatingsCount] [int] NOT NULL,
	[PublisherCycleReadModel_Id] [int] NULL,
	[PublisherReadModel_Id] [int] NULL,
	[SeriesReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.PublisherCycles_PublisherCycleReadModel_Id] FOREIGN KEY([PublisherCycleReadModel_Id])
REFERENCES [dbo].[PublisherCycles] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.PublisherCycles_PublisherCycleReadModel_Id]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherReadModel_Id] FOREIGN KEY([PublisherReadModel_Id])
REFERENCES [dbo].[Publishers] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherReadModel_Id]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Series_SeriesReadModel_Id] FOREIGN KEY([SeriesReadModel_Id])
REFERENCES [dbo].[Series] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Series_SeriesReadModel_Id]
GO


CREATE TABLE [dbo].[Ratings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookId] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Stars] [int] NOT NULL,
	[BookReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Ratings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Ratings_dbo.Books_BookReadModel_Id] FOREIGN KEY([BookReadModel_Id])
REFERENCES [dbo].[Books] ([Id])
GO

ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_dbo.Ratings_dbo.Books_BookReadModel_Id]
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

CREATE TABLE [dbo].[BookReadModelAuthorReadModels](
	[BookReadModel_Id] [int] NOT NULL,
	[AuthorReadModel_Id] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BookReadModelAuthorReadModels] PRIMARY KEY CLUSTERED 
(
	[BookReadModel_Id] ASC,
	[AuthorReadModel_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[BookReadModelAuthorReadModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookReadModelAuthorReadModels_dbo.Authors_AuthorReadModel_Id] FOREIGN KEY([AuthorReadModel_Id])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookReadModelAuthorReadModels] CHECK CONSTRAINT [FK_dbo.BookReadModelAuthorReadModels_dbo.Authors_AuthorReadModel_Id]
GO

ALTER TABLE [dbo].[BookReadModelAuthorReadModels]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BookReadModelAuthorReadModels_dbo.Books_BookReadModel_Id] FOREIGN KEY([BookReadModel_Id])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[BookReadModelAuthorReadModels] CHECK CONSTRAINT [FK_dbo.BookReadModelAuthorReadModels_dbo.Books_BookReadModel_Id]
GO

PRINT 'Ratings database structure created !'