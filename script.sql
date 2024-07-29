USE [master]
GO
/****** Object:  Database [PHONE_STORE]    Script Date: 2024-07-03 11:10:07 AM ******/
CREATE DATABASE [PHONE_STORE]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PHONE_STORE', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLDEV\MSSQL\DATA\PHONE_STORE.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PHONE_STORE_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLDEV\MSSQL\DATA\PHONE_STORE_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [PHONE_STORE] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PHONE_STORE].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PHONE_STORE] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PHONE_STORE] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PHONE_STORE] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PHONE_STORE] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PHONE_STORE] SET ARITHABORT OFF 
GO
ALTER DATABASE [PHONE_STORE] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PHONE_STORE] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PHONE_STORE] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PHONE_STORE] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PHONE_STORE] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PHONE_STORE] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PHONE_STORE] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PHONE_STORE] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PHONE_STORE] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PHONE_STORE] SET  ENABLE_BROKER 
GO
ALTER DATABASE [PHONE_STORE] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PHONE_STORE] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PHONE_STORE] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PHONE_STORE] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PHONE_STORE] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PHONE_STORE] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PHONE_STORE] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PHONE_STORE] SET RECOVERY FULL 
GO
ALTER DATABASE [PHONE_STORE] SET  MULTI_USER 
GO
ALTER DATABASE [PHONE_STORE] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PHONE_STORE] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PHONE_STORE] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PHONE_STORE] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PHONE_STORE] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PHONE_STORE] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'PHONE_SHOPPING', N'ON'
GO
ALTER DATABASE [PHONE_STORE] SET QUERY_STORE = ON
GO
ALTER DATABASE [PHONE_STORE] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [PHONE_STORE]
GO
/****** Object:  Table [dbo].[cart]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[cart](
	[cart_id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[product_id] [uniqueidentifier] NOT NULL,
	[quantity] [int] NOT NULL,
	[is_check_out] [bit] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_cart] PRIMARY KEY CLUSTERED 
(
	[cart_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[category]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[category_name] [varchar](50) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_category] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[client]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[client](
	[client_id] [int] IDENTITY(1,1) NOT NULL,
	[hareware_info] [varchar](max) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_client] PRIMARY KEY CLUSTERED 
(
	[client_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order](
	[order_id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[status] [nvarchar](50) NOT NULL,
	[address] [nvarchar](max) NOT NULL,
	[note] [nvarchar](max) NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_order] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_detail]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_detail](
	[detail_id] [uniqueidentifier] NOT NULL,
	[order_id] [uniqueidentifier] NOT NULL,
	[product_id] [uniqueidentifier] NOT NULL,
	[product_name] [nvarchar](max) NOT NULL,
	[image] [nvarchar](max) NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
	[quantity] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_order_detail] PRIMARY KEY CLUSTERED 
(
	[detail_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[product]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[product](
	[product_id] [uniqueidentifier] NOT NULL,
	[product_name] [nvarchar](max) NOT NULL,
	[image] [nvarchar](max) NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
	[category_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_product] PRIMARY KEY CLUSTERED 
(
	[product_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_name] [varchar](50) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_role] PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user](
	[user_id] [uniqueidentifier] NOT NULL,
	[full_name] [varchar](max) NOT NULL,
	[phone] [nvarchar](10) NOT NULL,
	[email] [nvarchar](max) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[password] [nvarchar](max) NOT NULL,
	[role_id] [int] NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[user_client]    Script Date: 2024-07-03 11:10:07 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[user_client](
	[user_client_id] [uniqueidentifier] NOT NULL,
	[user_id] [uniqueidentifier] NOT NULL,
	[client_id] [int] NOT NULL,
	[token] [nvarchar](max) NOT NULL,
	[expire_date] [datetime2](7) NOT NULL,
	[created_at] [datetime2](7) NOT NULL,
	[update_at] [datetime2](7) NOT NULL,
	[is_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_user_client] PRIMARY KEY CLUSTERED 
(
	[user_client_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[category] ON 

INSERT [dbo].[category] ([category_id], [category_name], [created_at], [update_at], [is_deleted]) VALUES (1, N'Samsung', CAST(N'2024-07-03T01:46:24.5563326' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563336' AS DateTime2), 0)
INSERT [dbo].[category] ([category_id], [category_name], [created_at], [update_at], [is_deleted]) VALUES (2, N'Oppo', CAST(N'2024-07-03T01:46:24.5563339' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563339' AS DateTime2), 0)
INSERT [dbo].[category] ([category_id], [category_name], [created_at], [update_at], [is_deleted]) VALUES (3, N'Iphone', CAST(N'2024-07-03T01:46:24.5563341' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563341' AS DateTime2), 0)
INSERT [dbo].[category] ([category_id], [category_name], [created_at], [update_at], [is_deleted]) VALUES (4, N'Vivo', CAST(N'2024-07-03T01:46:24.5563342' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563342' AS DateTime2), 0)
INSERT [dbo].[category] ([category_id], [category_name], [created_at], [update_at], [is_deleted]) VALUES (5, N'Nokia', CAST(N'2024-07-03T01:46:24.5563343' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563343' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[category] OFF
GO
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'b540b32f-4347-4dca-833f-02bbbd79d1dc', N'iPhone 13 Pro', N'https://cdn.tgdd.vn/Products/Images/42/230521/iphone-13-pro-sierra-blue-600x600.jpg', CAST(17.53 AS Decimal(10, 2)), 3, 10, CAST(N'2024-07-03T01:46:24.5563489' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563490' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'e57e5ae6-629e-4064-99a7-0906fae61203', N'Vivo Y15 series', N'https://cdn.tgdd.vn/Products/Images/42/249720/Vivo-y15s-2021-xanh-den-660x600.jpg', CAST(6.86 AS Decimal(10, 2)), 4, 10, CAST(N'2024-07-03T01:46:24.5563492' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563492' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'4e5cdf47-5bd3-42e6-9927-0b6371327eb8', N'iPhone 11', N'https://cdn.tgdd.vn/Products/Images/42/153856/iphone-xi-xanhla-600x600.jpg', CAST(12.23 AS Decimal(10, 2)), 3, 10, CAST(N'2024-07-03T01:46:24.5563480' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563481' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'9dca93eb-9b48-44a3-9069-167c875772eb', N'OPPO Reno7 series', N'https://cdn.tgdd.vn/Products/Images/42/271717/oppo-reno7-z-5g-thumb-1-1-600x600.jpg', CAST(10.25 AS Decimal(10, 2)), 2, 10, CAST(N'2024-07-03T01:46:24.5563514' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563514' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'71bc386c-8a38-4efe-b5be-1f86126c5e27', N'Nokia G21', N'https://cdn.tgdd.vn/Products/Images/42/270207/nokia-g21-xanh-thumb-600x600.jpg', CAST(2.77 AS Decimal(10, 2)), 5, 10, CAST(N'2024-07-03T01:46:24.5563510' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563511' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'2126de48-415e-46c1-90ee-2d1c0b4174b7', N'Nokia G11', N'https://cdn.tgdd.vn/Products/Images/42/272148/Nokia-g11-x%C3%A1m-thumb-600x600.jpg', CAST(1.99 AS Decimal(10, 2)), 5, 10, CAST(N'2024-07-03T01:46:24.5563512' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563513' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'7b28b953-3fbe-4474-b6c7-417a81e92105', N'Samsung Galaxy A33 5G 6GB', N'https://cdn.tgdd.vn/Products/Images/42/246199/samsung-galaxy-a33-5g-xanh-thumb-600x600.jpg', CAST(5.50 AS Decimal(10, 2)), 1, 10, CAST(N'2024-07-03T01:46:24.5563505' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563506' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'8d301247-8085-41a5-9d25-469d74ed9cf7', N'OPPO A76', N'https://cdn.tgdd.vn/Products/Images/42/270360/OPPO-A76-%C4%91en-600x600.jpg', CAST(7.99 AS Decimal(10, 2)), 2, 10, CAST(N'2024-07-03T01:46:24.5563522' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563522' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'cbb43f37-7fe4-4a05-b95f-47eecfb3746f', N'OPPO Find X5 Pro 5G', N'https://cdn.tgdd.vn/Products/Images/42/250622/oppo-find-x5-pro-den-thumb-600x600.jpg', CAST(10.35 AS Decimal(10, 2)), 2, 10, CAST(N'2024-07-03T01:46:24.5563520' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563521' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'24cd3165-7217-4cbc-90cc-4b2f96e743ff', N'Vivo V21 5G', N'https://cdn.tgdd.vn/Products/Images/42/238047/vivo-v21-5g-xanh-den-600x600.jpg', CAST(5.56 AS Decimal(10, 2)), 4, 10, CAST(N'2024-07-03T01:46:24.5563496' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563496' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'445686a0-cc62-4e7e-939f-4c6c3d969b55', N'Vivo Y53s', N'https://cdn.tgdd.vn/Products/Images/42/240286/vivo-y53s-xanh-600x600.jpg', CAST(4.65 AS Decimal(10, 2)), 4, 10, CAST(N'2024-07-03T01:46:24.5563507' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563508' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'e731a372-897a-4e4f-a811-6f5cd7c1162e', N'OPPO A16', N'https://cdn.tgdd.vn/Products/Images/42/240631/oppo-a16-silver-8-600x600.jpg', CAST(7.67 AS Decimal(10, 2)), 2, 10, CAST(N'2024-07-03T01:46:24.5563477' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563478' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'8d3c6d85-8d84-4244-8b0f-7041c6544c3d', N'iPhone 12 Pro Max 512GB', N'https://cdn.tgdd.vn/Products/Images/42/228744/iphone-12-pro-max-xanh-duong-new-600x600-600x600.jpg', CAST(18.70 AS Decimal(10, 2)), 3, 10, CAST(N'2024-07-03T01:46:24.5563485' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563485' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'd9c20953-f211-4cef-9936-8787441bf212', N'Vivo Y55', N'https://cdn.tgdd.vn/Products/Images/42/278949/vivo-y55-den-thumb-600x600.jpg', CAST(10.23 AS Decimal(10, 2)), 4, 10, CAST(N'2024-07-03T01:46:24.5563497' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563498' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'4e1088e4-77fa-43f0-b821-91b97afb33ec', N'Nokia G10', N'https://cdn.tgdd.vn/Products/Images/42/235995/Nokia%20g10%20xanh%20duong-600x600.jpg', CAST(3.65 AS Decimal(10, 2)), 5, 10, CAST(N'2024-07-03T01:46:24.5563516' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563516' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'047b6968-b552-431a-9477-91ffddc520e3', N'Samsung Galaxy A03', N'https://cdn.tgdd.vn/Products/Images/42/251856/samsung-galaxy-a03-xanh-thumb-600x600.jpg', CAST(8.99 AS Decimal(10, 2)), 1, 10, CAST(N'2024-07-03T01:46:24.5563503' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563504' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'ced9d12b-9f6e-43c3-8f0b-ada146c137cc', N'Nokia 215 4G', N'https://cdn.tgdd.vn/Products/Images/42/228366/nokia-215-xanh-ngoc-new-600x600-600x600.jpg', CAST(2.65 AS Decimal(10, 2)), 5, 10, CAST(N'2024-07-03T01:46:24.5563518' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563518' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'512dbe03-8c7c-480d-9d0e-d2100312a760', N'iPhone 13 Pro Max', N'https://cdn.tgdd.vn/Products/Images/42/230529/iphone-13-pro-max-gold-1-600x600.jpg', CAST(20.40 AS Decimal(10, 2)), 3, 10, CAST(N'2024-07-03T01:46:24.5563483' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563483' AS DateTime2), 0)
INSERT [dbo].[product] ([product_id], [product_name], [image], [price], [category_id], [quantity], [created_at], [update_at], [is_deleted]) VALUES (N'47a71d57-9e30-4381-8d46-e2ee9189c588', N'Galaxy S22 Ultra 5G', N'https://cdn.tgdd.vn/Products/Images/42/235838/Galaxy-S22-Ultra-Burgundy-600x600.jpg', CAST(8.99 AS Decimal(10, 2)), 1, 10, CAST(N'2024-07-03T01:46:24.5563494' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563494' AS DateTime2), 0)
GO
SET IDENTITY_INSERT [dbo].[role] ON 

INSERT [dbo].[role] ([role_id], [role_name], [created_at], [update_at], [is_deleted]) VALUES (1, N'Admin', CAST(N'2024-07-03T01:46:24.5563385' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563385' AS DateTime2), 0)
INSERT [dbo].[role] ([role_id], [role_name], [created_at], [update_at], [is_deleted]) VALUES (2, N'Customer', CAST(N'2024-07-03T01:46:24.5563387' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563387' AS DateTime2), 0)
SET IDENTITY_INSERT [dbo].[role] OFF
GO
INSERT [dbo].[user] ([user_id], [full_name], [phone], [email], [username], [password], [role_id], [created_at], [update_at], [is_deleted]) VALUES (N'28fd3cb4-679e-44e2-a1b2-239c560beeed', N'Chu Quang Quan', N'8851738015', N'fellcock2@gmail.com', N'QuangQuan', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-07-03T01:46:24.5563457' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563458' AS DateTime2), 0)
INSERT [dbo].[user] ([user_id], [full_name], [phone], [email], [username], [password], [role_id], [created_at], [update_at], [is_deleted]) VALUES (N'dd72fe50-fce2-427b-b90d-4c9f2c4d4ef9', N'Nguyen Minh Duc', N'5541282702', N'bkervin4@gmail.com', N'MinhDuc', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-07-03T01:46:24.5563462' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563462' AS DateTime2), 0)
INSERT [dbo].[user] ([user_id], [full_name], [phone], [email], [username], [password], [role_id], [created_at], [update_at], [is_deleted]) VALUES (N'149766de-b9ad-49d1-ba23-8b3a1a187152', N'Kirk Nelson', N'4533389559', N'oparagreen0@usnews.com', N'Admin123', N'70db85967ceb5ab1d79060fe0e2fc536f02ca747086564989953385fe58cab7f', 1, CAST(N'2024-07-03T01:46:24.5563460' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563460' AS DateTime2), 0)
INSERT [dbo].[user] ([user_id], [full_name], [phone], [email], [username], [password], [role_id], [created_at], [update_at], [is_deleted]) VALUES (N'3cd31819-756d-44bb-a899-a2d539e85131', N'Nguyen Thi Thu', N'0984739845', N'oparagreen0@gmail.com', N'ThuThu', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-07-03T01:46:24.5563431' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563431' AS DateTime2), 0)
INSERT [dbo].[user] ([user_id], [full_name], [phone], [email], [username], [password], [role_id], [created_at], [update_at], [is_deleted]) VALUES (N'2a01c0d5-9de8-405b-b526-dda8daa1acb0', N'Nguyen Anh Tuan', N'6298446654', N'kfleet1@gmail.com', N'AnhTuan', N'c1d0e46fdeb2b72758a6a5bd5eecf2622ff8b84a8964c8e9687c6c05c9f474b5', 2, CAST(N'2024-07-03T01:46:24.5563455' AS DateTime2), CAST(N'2024-07-03T01:46:24.5563455' AS DateTime2), 0)
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_product_product_id] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([product_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_product_product_id]
GO
ALTER TABLE [dbo].[cart]  WITH CHECK ADD  CONSTRAINT [FK_cart_user_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[cart] CHECK CONSTRAINT [FK_cart_user_user_id]
GO
ALTER TABLE [dbo].[order]  WITH CHECK ADD  CONSTRAINT [FK_order_user_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[order] CHECK CONSTRAINT [FK_order_user_user_id]
GO
ALTER TABLE [dbo].[order_detail]  WITH CHECK ADD  CONSTRAINT [FK_order_detail_order_order_id] FOREIGN KEY([order_id])
REFERENCES [dbo].[order] ([order_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[order_detail] CHECK CONSTRAINT [FK_order_detail_order_order_id]
GO
ALTER TABLE [dbo].[order_detail]  WITH CHECK ADD  CONSTRAINT [FK_order_detail_product_product_id] FOREIGN KEY([product_id])
REFERENCES [dbo].[product] ([product_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[order_detail] CHECK CONSTRAINT [FK_order_detail_product_product_id]
GO
ALTER TABLE [dbo].[product]  WITH CHECK ADD  CONSTRAINT [FK_product_category_category_id] FOREIGN KEY([category_id])
REFERENCES [dbo].[category] ([category_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[product] CHECK CONSTRAINT [FK_product_category_category_id]
GO
ALTER TABLE [dbo].[user]  WITH CHECK ADD  CONSTRAINT [FK_user_role_role_id] FOREIGN KEY([role_id])
REFERENCES [dbo].[role] ([role_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[user] CHECK CONSTRAINT [FK_user_role_role_id]
GO
ALTER TABLE [dbo].[user_client]  WITH CHECK ADD  CONSTRAINT [FK_user_client_client_client_id] FOREIGN KEY([client_id])
REFERENCES [dbo].[client] ([client_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[user_client] CHECK CONSTRAINT [FK_user_client_client_client_id]
GO
ALTER TABLE [dbo].[user_client]  WITH CHECK ADD  CONSTRAINT [FK_user_client_user_user_id] FOREIGN KEY([user_id])
REFERENCES [dbo].[user] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[user_client] CHECK CONSTRAINT [FK_user_client_user_user_id]
GO
