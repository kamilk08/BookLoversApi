USE [BookcaseStoreContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventEntities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AggregateGuid] [uniqueidentifier] NOT NULL,
	[Event_Data] [nvarchar](max) NOT NULL,
	[Event_Type] [nvarchar](max) NOT NULL,
	[Event_Assembly] [nvarchar](max) NOT NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [Id] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

USE [BookcaseStoreContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Snapshots](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AggregateGuid] [uniqueidentifier] NOT NULL,
	[SnapshottedAggregate] [nvarchar](max) NOT NULL,
	[Version] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Snapshots] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

PRINT 'Bookcase store structure created';




