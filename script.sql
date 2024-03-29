USE [master]
GO
/****** Object:  Database [PHONE_SHOPPING]    Script Date: 2024-01-12 9:54:46 PM ******/
CREATE DATABASE [PHONE_SHOPPING]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PHONE_SHOPPING', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLDEV\MSSQL\DATA\PHONE_SHOPPING.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PHONE_SHOPPING_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLDEV\MSSQL\DATA\PHONE_SHOPPING_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PHONE_SHOPPING] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PHONE_SHOPPING].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PHONE_SHOPPING] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ARITHABORT OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PHONE_SHOPPING] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PHONE_SHOPPING] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PHONE_SHOPPING] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PHONE_SHOPPING] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET RECOVERY FULL 
GO
ALTER DATABASE [PHONE_SHOPPING] SET  MULTI_USER 
GO
ALTER DATABASE [PHONE_SHOPPING] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PHONE_SHOPPING] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PHONE_SHOPPING] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PHONE_SHOPPING] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PHONE_SHOPPING] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PHONE_SHOPPING] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PHONE_SHOPPING', N'ON'
GO
ALTER DATABASE [PHONE_SHOPPING] SET QUERY_STORE = ON
GO
ALTER DATABASE [PHONE_SHOPPING] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PHONE_SHOPPING]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[quantity] [int] NOT NULL,
	[isCheckout] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Order]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[OrderID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[status] [varchar](50) NOT NULL,
	[address] [varchar](max) NOT NULL,
	[Note] [varchar](max) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[OrderID] [uniqueidentifier] NOT NULL,
	[ProductID] [uniqueidentifier] NOT NULL,
	[quantity] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [uniqueidentifier] NOT NULL,
	[ProductName] [varchar](max) NOT NULL,
	[image] [varchar](max) NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
	[CategoryID] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2024-01-12 9:54:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [uniqueidentifier] NOT NULL,
	[FullName] [varchar](max) NOT NULL,
	[phone] [varchar](10) NOT NULL,
	[email] [varchar](max) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varchar](max) NOT NULL,
	[RoleID] [int] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Category] ON 
GO
INSERT [dbo].[Category] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (1, N'Samsung', CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
INSERT [dbo].[Category] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (2, N'Oppo', CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
INSERT [dbo].[Category] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (3, N'Iphone', CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
INSERT [dbo].[Category] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (4, N'Vivo', CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
INSERT [dbo].[Category] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (5, N'Nokia', CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'9f64f4c4-e5f3-4106-888b-0ea0ac44eb24', N'OPPO A16', N'https://cdn.tgdd.vn/Products/Images/42/240631/oppo-a16-silver-8-600x600.jpg', CAST(7.67 AS Decimal(10, 2)), 2, 10, CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'c966ba66-8501-4bce-a88a-1254d9e7659f', N'iPhone 11', N'https://cdn.tgdd.vn/Products/Images/42/153856/iphone-xi-xanhla-600x600.jpg', CAST(12.23 AS Decimal(10, 2)), 3, 10, CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'cd2729f7-9ff4-4b0f-9493-195eec2f806f', N'iPhone 13 Pro Max', N'https://cdn.tgdd.vn/Products/Images/42/230529/iphone-13-pro-max-gold-1-600x600.jpg', CAST(20.40 AS Decimal(10, 2)), 3, 10, CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'e8684492-dfb9-4a14-a189-1e4d2b85e043', N'iPhone 12 Pro Max 512GB', N'https://cdn.tgdd.vn/Products/Images/42/228744/iphone-12-pro-max-xanh-duong-new-600x600-600x600.jpg', CAST(18.70 AS Decimal(10, 2)), 3, 10, CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'b76d9720-3ee6-4aae-a7b9-28019369598f', N'iPhone 13 Pro', N'https://cdn.tgdd.vn/Products/Images/42/230521/iphone-13-pro-sierra-blue-600x600.jpg', CAST(17.53 AS Decimal(10, 2)), 3, 10, CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'3eccee4f-0ca2-46e4-a851-62cf6c612aa1', N'Vivo Y15 series', N'https://cdn.tgdd.vn/Products/Images/42/249720/Vivo-y15s-2021-xanh-den-660x600.jpg', CAST(6.86 AS Decimal(10, 2)), 4, 10, CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'15e4e819-864c-4e6f-bd0d-6c3358890186', N'Galaxy S22 Ultra 5G', N'https://cdn.tgdd.vn/Products/Images/42/235838/Galaxy-S22-Ultra-Burgundy-600x600.jpg', CAST(8.99 AS Decimal(10, 2)), 1, 10, CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'98026735-7f4f-4033-9127-6cf364cbe338', N'Vivo V21 5G', N'https://cdn.tgdd.vn/Products/Images/42/238047/vivo-v21-5g-xanh-den-600x600.jpg', CAST(5.56 AS Decimal(10, 2)), 4, 10, CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5433333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'6158e784-afe7-4914-a336-769ce65d1e72', N'Vivo Y55', N'https://cdn.tgdd.vn/Products/Images/42/278949/vivo-y55-den-thumb-600x600.jpg', CAST(10.23 AS Decimal(10, 2)), 4, 10, CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'db34e16e-cdb3-4a2a-99d4-85eeb538aed4', N'Samsung Galaxy A03', N'https://cdn.tgdd.vn/Products/Images/42/251856/samsung-galaxy-a03-xanh-thumb-600x600.jpg', CAST(8.99 AS Decimal(10, 2)), 1, 10, CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'11252e0a-429e-4811-a951-916f845ffae6', N'Samsung Galaxy A33 5G 6GB', N'https://cdn.tgdd.vn/Products/Images/42/246199/samsung-galaxy-a33-5g-xanh-thumb-600x600.jpg', CAST(5.50 AS Decimal(10, 2)), 1, 10, CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5333333' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'331c3656-2432-476b-8873-9483dd972875', N'Vivo Y53s', N'https://cdn.tgdd.vn/Products/Images/42/240286/vivo-y53s-xanh-600x600.jpg', CAST(4.65 AS Decimal(10, 2)), 4, 10, CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'5ce247ab-d25d-406b-8a16-b12a020df2e5', N'Nokia G21', N'https://cdn.tgdd.vn/Products/Images/42/270207/nokia-g21-xanh-thumb-600x600.jpg', CAST(2.77 AS Decimal(10, 2)), 5, 10, CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'6d70c885-1a55-488f-9a0f-b3b329ba39b4', N'Nokia G11', N'https://cdn.tgdd.vn/Products/Images/42/272148/Nokia-g11-x%C3%A1m-thumb-600x600.jpg', CAST(1.99 AS Decimal(10, 2)), 5, 10, CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5466667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'1cbe4214-c2e8-45e2-9e6c-bd6b5bf5af80', N'OPPO Reno7 series', N'https://cdn.tgdd.vn/Products/Images/42/271717/oppo-reno7-z-5g-thumb-1-1-600x600.jpg', CAST(10.25 AS Decimal(10, 2)), 2, 10, CAST(N'2024-01-12T21:50:52.5366667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5366667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'a8723b4f-e197-40d0-a084-bf30dcdcdd26', N'Nokia G10', N'https://cdn.tgdd.vn/Products/Images/42/235995/Nokia%20g10%20xanh%20duong-600x600.jpg', CAST(3.65 AS Decimal(10, 2)), 5, 10, CAST(N'2024-01-12T21:50:52.5500000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5500000' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'3ee588de-6a91-40d2-91fe-c0f28e541116', N'Nokia 215 4G', N'https://cdn.tgdd.vn/Products/Images/42/228366/nokia-215-xanh-ngoc-new-600x600-600x600.jpg', CAST(2.65 AS Decimal(10, 2)), 5, 10, CAST(N'2024-01-12T21:50:52.5500000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5500000' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'f5db8135-ac32-4f27-a3f3-e5a74125207f', N'OPPO Find X5 Pro 5G', N'https://cdn.tgdd.vn/Products/Images/42/250622/oppo-find-x5-pro-den-thumb-600x600.jpg', CAST(10.35 AS Decimal(10, 2)), 2, 10, CAST(N'2024-01-12T21:50:52.5366667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5366667' AS DateTime2), 0)
GO
INSERT [dbo].[Product] ([ProductID], [ProductName], [image], [price], [CategoryID], [quantity], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'bebec556-dae2-40db-a358-e89433e47fa9', N'OPPO A76', N'https://cdn.tgdd.vn/Products/Images/42/270360/OPPO-A76-%C4%91en-600x600.jpg', CAST(7.99 AS Decimal(10, 2)), 2, 10, CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5400000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[Role] ON 
GO
INSERT [dbo].[Role] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (1, N'Admin', CAST(N'2024-01-12T21:50:52.5166667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5166667' AS DateTime2), 0)
GO
INSERT [dbo].[Role] ([ID], [Name], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (2, N'Customer', CAST(N'2024-01-12T21:50:52.5200000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5200000' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[Role] OFF
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'2803070f-0645-4676-959a-0e21ea7936f2', N'Kirk Nelson', N'4533389559', N'oparagreen0@usnews.com', N'Admin123', N'70db85967ceb5ab1d79060fe0e2fc536f02ca747086564989953385fe58cab7f', 1, CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'4568d977-a5d3-40ab-a230-1a2d3d452236', N'Nguyen Thi Thu', N'4533389559', N'oparagreen0@usnews.com', N'ThuThu', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'fe8270bf-77b4-4973-9071-250139180daf', N'Nguyen Anh Tuan', N'6298446654', N'kfleet1@artisteer.com', N'AnhTuan', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'a2b5b86f-366c-450e-9557-303750156ac4', N'Chu Quang Quan', N'8851738015', N'fellcock2@earthlink.net', N'QuangQuan', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), CAST(N'2024-01-12T21:50:52.5233333' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'baf77e4d-e06b-4e64-9ef9-380ade2efab8', N'Nguyen Minh Duc', N'5541282702', N'bkervin4@fotki.com', N'MinhDuc', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'fd068a91-f7fc-4fad-942f-3984bfe2b8e1', N'Nicky Gaitone', N'7583151589', N'ngaitone6@cyberchimps.com', N'Nicky', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'6184d843-5479-4321-8cad-4df2009716f0', N'Elvis Dutton', N'3475443555', N'edutton7@angelfire.com', N'Elvis', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'5c32794e-f915-4b4a-b007-56ee06e226c3', N'Niel Kerwood', N'5399463237', N'nkerwood8@nps.gov', N'Niel', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), CAST(N'2024-01-12T21:50:52.5266667' AS DateTime2), 0)
GO
INSERT [dbo].[User] ([UserID], [FullName], [phone], [email], [username], [password], [RoleID], [CreatedAt], [UpdateAt], [IsDeleted]) VALUES (N'67670494-14d9-448f-91e6-65d956e90e8c', N'Willey Lefley', N'9561589898', N'wlefley9@squarespace.com', N'Willey', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), CAST(N'2024-01-12T21:50:52.5300000' AS DateTime2), 0)
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([OrderID])
REFERENCES [dbo].[Order] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Category] ([ID])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Role] ([ID])
GO
USE [master]
GO
ALTER DATABASE [PHONE_SHOPPING] SET  READ_WRITE 
GO
