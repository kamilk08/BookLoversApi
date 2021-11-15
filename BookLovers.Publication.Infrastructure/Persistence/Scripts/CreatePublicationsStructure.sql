USE [PublicationsContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[CoverTypes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CoverType] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.CoverTypes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Languages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_dbo.Languages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SubCategories](
	[Id] [int] NOT NULL,
	[SubCategory] [nvarchar](max) NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.SubCategories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[SubCategories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.SubCategories_dbo.Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[SubCategories] CHECK CONSTRAINT [FK_dbo.SubCategories_dbo.Categories_CategoryId]
GO

CREATE TABLE [dbo].[Readers](
	[ReaderId] [int] NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Readers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Publishers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Publisher] [nvarchar](max) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Publishers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Series](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Series] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Cycles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Cycle] [nvarchar](255) NOT NULL,
	[Status] [int] NOT NULL,
	[PublisherId] [int] NULL,
 CONSTRAINT [PK_dbo.Cycles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Cycles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Cycles_dbo.Publishers_PublisherId] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[Publishers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Cycles] CHECK CONSTRAINT [FK_dbo.Cycles_dbo.Publishers_PublisherId]
GO

CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](256) NOT NULL,
	[FirstName] [nvarchar](128) NULL,
	[SecondName] [nvarchar](128) NOT NULL,
	[Status] [int] NOT NULL,
	[BirthDate] [datetime2](0) NULL,
	[DeathDate] [datetime2](0) NULL,
	[BirthPlace] [nvarchar](255) NULL,
	[AboutAuthor] [nvarchar](2083) NULL,
	[DescriptionSource] [nvarchar](2083) NULL,
	[WebSite] [nvarchar](2083) NULL,
	[Sex] [int] NOT NULL,
	[AddedById] [int] NULL,
 CONSTRAINT [PK_dbo.Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Authors]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Authors_dbo.Readers_AddedById] FOREIGN KEY([AddedById])
REFERENCES [dbo].[Readers] ([Id])
GO

ALTER TABLE [dbo].[Authors] CHECK CONSTRAINT [FK_dbo.Authors_dbo.Readers_AddedById]
GO

CREATE TABLE [dbo].[AuthorImages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorPictureUrl] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[MimeType] [nvarchar](max) NOT NULL,
	[AuthorGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.AuthorImages] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[AuthorFollowers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorId] [int] NOT NULL,
	[FollowedById] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AuthorFollowers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuthorFollowers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorFollowers_dbo.Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorFollowers] CHECK CONSTRAINT [FK_dbo.AuthorFollowers_dbo.Authors_AuthorId]
GO

ALTER TABLE [dbo].[AuthorFollowers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorFollowers_dbo.Readers_FollowedById] FOREIGN KEY([FollowedById])
REFERENCES [dbo].[Readers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorFollowers] CHECK CONSTRAINT [FK_dbo.AuthorFollowers_dbo.Readers_FollowedById]
GO

CREATE TABLE [dbo].[AuthorCategories](
	[AuthorId] [int] NOT NULL,
	[SubCategoryId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AuthorCategories] PRIMARY KEY CLUSTERED 
(
	[AuthorId] ASC,
	[SubCategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuthorCategories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorCategories_dbo.Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorCategories] CHECK CONSTRAINT [FK_dbo.AuthorCategories_dbo.Authors_AuthorId]
GO

ALTER TABLE [dbo].[AuthorCategories]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorCategories_dbo.SubCategories_SubCategoryId] FOREIGN KEY([SubCategoryId])
REFERENCES [dbo].[SubCategories] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorCategories] CHECK CONSTRAINT [FK_dbo.AuthorCategories_dbo.SubCategories_SubCategoryId]
GO

CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[ISBN] [nvarchar](30) NOT NULL,
	[Category] [nvarchar](max) NULL,
	[CategoryId] [int] NOT NULL,
	[SubCategory] [nvarchar](max) NULL,
	[SubCategoryId] [int] NOT NULL,
	[PublicationDate] [datetime2](0) NOT NULL,
	[Description] [nvarchar](4000) NULL,
	[DescriptionSource] [nvarchar](4000) NULL,
	[CoverSource] [nvarchar](max) NULL,
	[Pages] [int] NULL,
	[Status] [int] NOT NULL,
	[PublisherId] [int] NULL,
	[SeriesId] [int] NULL,
	[SeriesPosition] [int] NULL,
	[ReaderGuid] [int] NULL,
	[LanguageId] [int] NULL,
	[Language] [nvarchar](max) NULL,
	[CoverType] [nvarchar](max) NULL,
	[CoverTypeId] [int] NOT NULL,
	[HashTags] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherId] FOREIGN KEY([PublisherId])
REFERENCES [dbo].[Publishers] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Publishers_PublisherId]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Readers_ReaderGuid] FOREIGN KEY([ReaderGuid])
REFERENCES [dbo].[Readers] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Readers_ReaderGuid]
GO

ALTER TABLE [dbo].[Books]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Books_dbo.Series_SeriesId] FOREIGN KEY([SeriesId])
REFERENCES [dbo].[Series] ([Id])
GO

ALTER TABLE [dbo].[Books] CHECK CONSTRAINT [FK_dbo.Books_dbo.Series_SeriesId]
GO

CREATE TABLE [dbo].[AuthorBooks](
	[BookId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.AuthorBooks] PRIMARY KEY CLUSTERED 
(
	[BookId] ASC,
	[AuthorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AuthorBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorBooks_dbo.Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorBooks] CHECK CONSTRAINT [FK_dbo.AuthorBooks_dbo.Authors_AuthorId]
GO

ALTER TABLE [dbo].[AuthorBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AuthorBooks_dbo.Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[AuthorBooks] CHECK CONSTRAINT [FK_dbo.AuthorBooks_dbo.Books_BookId]
GO

CREATE TABLE [dbo].[CycleBooks](
	[CycleId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.CycleBooks] PRIMARY KEY CLUSTERED 
(
	[CycleId] ASC,
	[BookId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CycleBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CycleBooks_dbo.Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CycleBooks] CHECK CONSTRAINT [FK_dbo.CycleBooks_dbo.Books_BookId]
GO

ALTER TABLE [dbo].[CycleBooks]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CycleBooks_dbo.Cycles_CycleId] FOREIGN KEY([CycleId])
REFERENCES [dbo].[Cycles] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[CycleBooks] CHECK CONSTRAINT [FK_dbo.CycleBooks_dbo.Cycles_CycleId]
GO

CREATE TABLE [dbo].[Reviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Reviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reviews_dbo.Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_dbo.Reviews_dbo.Books_BookId]
GO

ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Reviews_dbo.Readers_ReaderId] FOREIGN KEY([ReaderId])
REFERENCES [dbo].[Readers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_dbo.Reviews_dbo.Readers_ReaderId]
GO

CREATE TABLE [dbo].[Quotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Quote] [nvarchar](max) NOT NULL,
	[AddedAt] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
	[AuthorId] [int] NULL,
	[ReaderGuid] [uniqueidentifier] NULL,
	[ReaderId] [int] NULL,
	[BookId] [int] NULL,
	[QuoteType] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Quotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Quotes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Quotes_dbo.Authors_AuthorId] FOREIGN KEY([AuthorId])
REFERENCES [dbo].[Authors] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Quotes] CHECK CONSTRAINT [FK_dbo.Quotes_dbo.Authors_AuthorId]
GO

ALTER TABLE [dbo].[Quotes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Quotes_dbo.Books_BookId] FOREIGN KEY([BookId])
REFERENCES [dbo].[Books] ([Id])
GO

ALTER TABLE [dbo].[Quotes] CHECK CONSTRAINT [FK_dbo.Quotes_dbo.Books_BookId]
GO

CREATE TABLE [dbo].[QuoteLikes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[QuoteId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.QuoteLikes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[QuoteLikes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.QuoteLikes_dbo.Quotes_QuoteId] FOREIGN KEY([QuoteId])
REFERENCES [dbo].[Quotes] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[QuoteLikes] CHECK CONSTRAINT [FK_dbo.QuoteLikes_dbo.Quotes_QuoteId]
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

CREATE TABLE [dbo].[BookCovers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CoverUrl] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[MimeType] [nvarchar](max) NOT NULL,
	[BookGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.BookCovers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

PRINT 'Publications database structure created !'