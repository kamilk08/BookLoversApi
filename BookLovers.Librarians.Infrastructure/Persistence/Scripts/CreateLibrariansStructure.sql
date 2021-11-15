USE [LibrariansContext]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TicketConcerns](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TicketConcerns] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Decisions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Decisions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[TicketOwners](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.TicketOwners] PRIMARY KEY CLUSTERED 
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

CREATE TABLE [dbo].[Librarians](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Librarians] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[PromotionAvailabilities](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[AvailabilityId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.PromotionAvailabilities] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[PromotionWaiters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReaderGuid] [uniqueidentifier] NOT NULL,
	[ReaderId] [int] NOT NULL,
	[PromotionAvailability] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_dbo.PromotionWaiters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ReportReasons](
	[ReasonId] [int] IDENTITY(1,1) NOT NULL,
	[ReasonName] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.ReportReasons] PRIMARY KEY CLUSTERED 
(
	[ReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Tickets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[TicketObjectGuid] [uniqueidentifier] NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[TicketStateValue] [tinyint] NOT NULL,
	[TicketState] [nvarchar](max) NOT NULL,
	[TicketConcernValue] [tinyint] NOT NULL,
	[TicketConcern] [nvarchar](max) NOT NULL,
	[DecisionValue] [tinyint] NOT NULL,
	[Decision] [nvarchar](max) NOT NULL,
	[LibrarianGuid] [uniqueidentifier] NULL,
	[TicketOwnerId] [int] NOT NULL,
	[TicketOwnerGuid] [uniqueidentifier] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Tickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[CreatedTickets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketGuid] [uniqueidentifier] NOT NULL,
	[IsSolved] [bit] NOT NULL,
	[TicketOwnerReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.CreatedTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CreatedTickets]  WITH CHECK ADD  CONSTRAINT [FK_dbo.CreatedTickets_dbo.TicketOwners_TicketOwnerReadModel_Id] FOREIGN KEY([TicketOwnerReadModel_Id])
REFERENCES [dbo].[TicketOwners] ([Id])
GO

ALTER TABLE [dbo].[CreatedTickets] CHECK CONSTRAINT [FK_dbo.CreatedTickets_dbo.TicketOwners_TicketOwnerReadModel_Id]
GO

CREATE TABLE [dbo].[ResolvedTickets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[TicketGuid] [uniqueidentifier] NOT NULL,
	[Justification] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
	[DecisionName] [nvarchar](max) NOT NULL,
	[DecisionValue] [tinyint] NOT NULL,
	[LibrarianReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ResolvedTickets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ResolvedTickets]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ResolvedTickets_dbo.Librarians_LibrarianReadModel_Id] FOREIGN KEY([LibrarianReadModel_Id])
REFERENCES [dbo].[Librarians] ([Id])
GO

ALTER TABLE [dbo].[ResolvedTickets] CHECK CONSTRAINT [FK_dbo.ResolvedTickets_dbo.Librarians_LibrarianReadModel_Id]
GO

CREATE TABLE [dbo].[ReviewReportRegisters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[ReviewGuid] [uniqueidentifier] NOT NULL,
	[LibrarianGuid] [uniqueidentifier] NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ReviewReportRegisters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ReviewReportItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReportedByGuid] [uniqueidentifier] NOT NULL,
	[ReportReasonId] [int] NOT NULL,
	[ReportReasonName] [nvarchar](max) NULL,
	[ReviewReportRegisterReadModel_Id] [int] NULL,
 CONSTRAINT [PK_dbo.ReviewReportItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReviewReportItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ReviewReportItems_dbo.ReviewReportRegisters_ReviewReportRegisterReadModel_Id] FOREIGN KEY([ReviewReportRegisterReadModel_Id])
REFERENCES [dbo].[ReviewReportRegisters] ([Id])
GO

ALTER TABLE [dbo].[ReviewReportItems] CHECK CONSTRAINT [FK_dbo.ReviewReportItems_dbo.ReviewReportRegisters_ReviewReportRegisterReadModel_Id]
GO

PRINT 'Librarians database structure created !'