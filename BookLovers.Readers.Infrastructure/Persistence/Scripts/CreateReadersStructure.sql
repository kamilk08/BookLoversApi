USE [ReadersContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

CREATE TABLE [dbo].[Sexes](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Sexes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Authors](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AuthorGuid] [uniqueidentifier] NOT NULL,
	[AuthorId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Authors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Avatars](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderId] [int] NOT NULL,
	[AvatarUrl] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[MimeType] [nvarchar](max) NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.Avatars] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Books](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BookGuid] [uniqueidentifier] NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Readers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Username] [nvarchar](255) NOT NULL,
	[AddedResourcesCount] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Email] [nvarchar](max) NULL,
	[Role] [nvarchar](max) NULL,
	[JoinedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Readers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Profiles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[FullName] [nvarchar](128) NULL,
	[Country] [nvarchar](255) NULL,
	[City] [nvarchar](max) NULL,
	[BirthDate] [datetime2](0) NULL,
	[About] [nvarchar](2083) NULL,
	[WebSite] [nvarchar](2083) NULL,
	[Status] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[JoinedAt] [datetime] NOT NULL,
	[Sex] [int] NOT NULL,
	[CurrentRole] [nvarchar](max) NULL,
	[SexName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Profiles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Profiles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Profiles_dbo.Readers_ReaderId] FOREIGN KEY([ReaderId])
REFERENCES [dbo].[Readers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Profiles] CHECK CONSTRAINT [FK_dbo.Profiles_dbo.Readers_ReaderId]
GO

CREATE TABLE [dbo].[ProfilePrivacyManagers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ProfileGuid] [uniqueidentifier] NOT NULL,
	[ProfileId] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ProfilePrivacyManagers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[NotificationWalls](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.NotificationWalls] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TimeLines](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TimeLines] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[TimelineActivities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[ActivityType] [int] NOT NULL,
	[Show] [bit] NOT NULL,
	[ActivityObjectGuid] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[TimelineId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TimelineActivities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[TimelineActivities]  WITH CHECK ADD  CONSTRAINT [FK_dbo.TimelineActivities_dbo.TimeLines_TimelineId] FOREIGN KEY([TimelineId])
REFERENCES [dbo].[TimeLines] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[TimelineActivities] CHECK CONSTRAINT [FK_dbo.TimelineActivities_dbo.TimeLines_TimelineId]
GO

CREATE TABLE [dbo].[Statistics](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[ReviewsCount] [int] NOT NULL,
	[ReceivedLikes] [int] NOT NULL,
	[GivenLikes] [int] NOT NULL,
	[ShelvesCount] [int] NOT NULL,
	[FollowersCount] [int] NOT NULL,
	[FollowingsCount] [int] NOT NULL,
	[BooksCount] [int] NOT NULL,
	[AddedAuthors] [int] NOT NULL,
	[AddedBooks] [int] NOT NULL,
	[AddedQuotes] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Statistics] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Statistics]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Statistics_dbo.Readers_ReaderId] FOREIGN KEY([ReaderId])
REFERENCES [dbo].[Readers] ([Id])
ON DELETE CASCADE
GO

CREATE TABLE [dbo].[Reviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[Review] [nvarchar](max) NOT NULL,
	[Date] [datetime2](0) NOT NULL,
	[EditedDate] [datetime2](0) NULL,
	[LikesCount] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[BookId] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[MarkedAsSpoilerByReader] [bit] NOT NULL,
	[MarkedAsSpoilerByOthers] [bit] NOT NULL,
	[SpoilerTagsCount] [int] NOT NULL,
	[ReportsCount] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Reviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
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

CREATE TABLE [dbo].[ReviewSpoilerTags](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReviewId] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReviewSpoilerTags] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReviewSpoilerTags]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReviewSpoilerTags_dbo.Reviews_ReviewId] FOREIGN KEY([ReviewId])
REFERENCES [dbo].[Reviews] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ReviewSpoilerTags] CHECK CONSTRAINT [FK_dbo.ReviewSpoilerTags_dbo.Reviews_ReviewId]
GO

CREATE TABLE [dbo].[ReviewReports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderId] [int] NOT NULL,
	[ReviewId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReviewReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReviewReports]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReviewReports_dbo.Reviews_ReviewId] FOREIGN KEY([ReviewId])
REFERENCES [dbo].[Reviews] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[ReviewReports] CHECK CONSTRAINT [FK_dbo.ReviewReports_dbo.Reviews_ReviewId]
GO

CREATE TABLE [dbo].[ReviewLikes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[ReviewReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ReviewLikes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReviewLikes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReviewLikes_dbo.Reviews_ReviewReadModel_Id] FOREIGN KEY([ReviewReadModel_Id])
REFERENCES [dbo].[Reviews] ([Id])
GO

ALTER TABLE [dbo].[ReviewLikes] CHECK CONSTRAINT [FK_dbo.ReviewLikes_dbo.Reviews_ReviewReadModel_Id]
GO

CREATE TABLE [dbo].[ReviewEdits](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReviewGuid] [uniqueidentifier] NOT NULL,
	[ReviewId] [int] NOT NULL,
	[Review] [nvarchar](max) NULL,
	[EditedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.ReviewEdits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[PrivacyOptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PrivacyTypeName] [nvarchar](max) NULL,
	[PrivacyTypeId] [int] NOT NULL,
	[PrivacyOptionName] [nvarchar](max) NULL,
	[PrivacyTypeOptionId] [int] NOT NULL,
	[ProfilePrivacyManagerReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.PrivacyOptions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NotificationObjects] [nvarchar](max) NULL,
	[AppearedAt] [datetime] NOT NULL,
	[SeenAt] [datetime] NULL,
	[IsVisible] [bit] NOT NULL,
	[NotificationGuid] [uniqueidentifier] NOT NULL,
	[NotificationType] [int] NOT NULL,
	[NotificationWallReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Notifications]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Notifications_dbo.NotificationWalls_NotificationWallReadModel_Id] FOREIGN KEY([NotificationWallReadModel_Id])
REFERENCES [dbo].[NotificationWalls] ([Id])
GO

ALTER TABLE [dbo].[Notifications] CHECK CONSTRAINT [FK_dbo.Notifications_dbo.NotificationWalls_NotificationWallReadModel_Id]
GO

CREATE TABLE [dbo].[Followings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Followed_Id] [int] NOT NULL,
	[Follower_Id] [int] NULL,
 CONSTRAINT [PK_dbo.Followings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Followings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Followings_dbo.Readers_Followed_Id] FOREIGN KEY([Followed_Id])
REFERENCES [dbo].[Readers] ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[Followings] CHECK CONSTRAINT [FK_dbo.Followings_dbo.Readers_Followed_Id]
GO

ALTER TABLE [dbo].[Followings]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Followings_dbo.Readers_Follower_Id] FOREIGN KEY([Follower_Id])
REFERENCES [dbo].[Readers] ([Id])
GO

ALTER TABLE [dbo].[Followings] CHECK CONSTRAINT [FK_dbo.Followings_dbo.Readers_Follower_Id]
GO

CREATE TABLE [dbo].[Favourites](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FavouriteGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Favourites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[FavouriteOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OwnerGuid] [uniqueidentifier] NOT NULL,
	[FavouriteReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.FavouriteOwners] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[FavouriteOwners]  WITH CHECK ADD  CONSTRAINT [FK_dbo.FavouriteOwners_dbo.Favourites_FavouriteReadModel_Id] FOREIGN KEY([FavouriteReadModel_Id])
REFERENCES [dbo].[Favourites] ([Id])
GO

ALTER TABLE [dbo].[FavouriteOwners] CHECK CONSTRAINT [FK_dbo.FavouriteOwners_dbo.Favourites_FavouriteReadModel_Id]
GO

CREATE TABLE [dbo].[AddedResources](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ResourceGuid] [uniqueidentifier] NOT NULL,
	[ReaderReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.AddedResources] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AddedResources]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AddedResources_dbo.Readers_ReaderReadModel_Id] FOREIGN KEY([ReaderReadModel_Id])
REFERENCES [dbo].[Readers] ([Id])
GO

ALTER TABLE [dbo].[AddedResources] CHECK CONSTRAINT [FK_dbo.AddedResources_dbo.Readers_ReaderReadModel_Id]
GO

CREATE TABLE [dbo].[ProfileFavourites](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FavouriteGuid] [uniqueidentifier] NOT NULL,
	[FavouriteType] [int] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[ProfileReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ProfileFavourites] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProfileFavourites]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ProfileFavourites_dbo.Profiles_ProfileReadModel_Id] FOREIGN KEY([ProfileReadModel_Id])
REFERENCES [dbo].[Profiles] ([Id])
GO

ALTER TABLE [dbo].[ProfileFavourites] CHECK CONSTRAINT [FK_dbo.ProfileFavourites_dbo.Profiles_ProfileReadModel_Id]
GO

PRINT 'Readers database structure created !'