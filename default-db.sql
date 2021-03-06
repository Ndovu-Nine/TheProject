USE [master]
GO
/****** Object:  Database [tembo]    Script Date: 14/03/2022 11:57:01 ******/
CREATE DATABASE [tembo]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'tembo', FILENAME = N'E:\large_database\tembo.mdf' , SIZE = 860160KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'tembo_log', FILENAME = N'E:\large_database\tembo_log.ldf' , SIZE = 13468608KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [tembo] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [tembo].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [tembo] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [tembo] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [tembo] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [tembo] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [tembo] SET ARITHABORT OFF 
GO
ALTER DATABASE [tembo] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [tembo] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [tembo] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [tembo] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [tembo] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [tembo] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [tembo] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [tembo] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [tembo] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [tembo] SET  DISABLE_BROKER 
GO
ALTER DATABASE [tembo] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [tembo] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [tembo] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [tembo] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [tembo] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [tembo] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [tembo] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [tembo] SET RECOVERY FULL 
GO
ALTER DATABASE [tembo] SET  MULTI_USER 
GO
ALTER DATABASE [tembo] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [tembo] SET DB_CHAINING OFF 
GO
ALTER DATABASE [tembo] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [tembo] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [tembo] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [tembo] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'tembo', N'ON'
GO
ALTER DATABASE [tembo] SET QUERY_STORE = OFF
GO
USE [tembo]
GO
/****** Object:  Table [dbo].[asset]    Script Date: 14/03/2022 11:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[asset](
	[SYSID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[lastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_asset] PRIMARY KEY CLUSTERED 
(
	[SYSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_asset_name] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[position]    Script Date: 14/03/2022 11:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[position](
	[SYSID] [int] IDENTITY(1,1) NOT NULL,
	[assetSYSID] [int] NOT NULL,
	[strategySYSID] [int] NOT NULL,
	[durationOfCandle] [varchar](10) NOT NULL,
	[openTime] [datetime] NOT NULL,
	[endTime] [datetime] NOT NULL,
	[durationOfTrade] [int] NOT NULL,
	[openPrice] [float] NOT NULL,
	[closePrice] [float] NULL,
	[direction] [bit] NOT NULL,
	[fractal] [int] NOT NULL,
	[macd] [int] NOT NULL,
	[rainbow] [int] NOT NULL,
	[rsi] [bit] NOT NULL,
	[stoch] [int] NOT NULL,
	[wpr] [int] NOT NULL,
	[trendA] [int] NOT NULL,
	[trendB] [int] NOT NULL,
	[trendC] [int] NOT NULL,
	[trendD] [int] NOT NULL,
	[trendE] [int] NOT NULL,
	[trendF] [int] NOT NULL,
	[outcome] [bit] NULL,
	[isSent] [bit] NOT NULL,
	[dateCreated] [datetime] NOT NULL,
	[lastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_position] PRIMARY KEY CLUSTERED 
(
	[SYSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[strategy]    Script Date: 14/03/2022 11:57:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[strategy](
	[SYSID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
	[summary] [varchar](1000) NOT NULL,
	[details] [text] NULL,
	[isIndicator] [bit] NOT NULL,
	[indicatorName] [varchar](50) NULL,
	[indicatorConfiguration] [text] NULL,
	[dateCreated] [datetime] NOT NULL,
	[lastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_strategy] PRIMARY KEY CLUSTERED 
(
	[SYSID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_strategy_name] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[asset] ADD  CONSTRAINT [DF_asset_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[asset] ADD  CONSTRAINT [DF_asset_lastUpdate]  DEFAULT (getdate()) FOR [lastUpdate]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_duration]  DEFAULT ('M1') FOR [durationOfCandle]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_bbSell]  DEFAULT ((0)) FOR [fractal]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_rainBuy]  DEFAULT ((0)) FOR [rainbow]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_rsiBuy]  DEFAULT ((0)) FOR [rsi]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_stoBuy]  DEFAULT ((0)) FOR [stoch]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_wprBuy]  DEFAULT ((0)) FOR [wpr]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_wprSell]  DEFAULT ((0)) FOR [trendA]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_rsiSell]  DEFAULT ((0)) FOR [trendC]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_atr]  DEFAULT ((0)) FOR [trendD]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_isSent]  DEFAULT ((0)) FOR [isSent]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[position] ADD  CONSTRAINT [DF_position_lastUpdate]  DEFAULT (getdate()) FOR [lastUpdate]
GO
ALTER TABLE [dbo].[strategy] ADD  CONSTRAINT [DF_strategy_dateCreated]  DEFAULT (getdate()) FOR [dateCreated]
GO
ALTER TABLE [dbo].[strategy] ADD  CONSTRAINT [DF_strategy_lastUpdate]  DEFAULT (getdate()) FOR [lastUpdate]
GO
ALTER TABLE [dbo].[position]  WITH CHECK ADD  CONSTRAINT [FK_position_asset] FOREIGN KEY([assetSYSID])
REFERENCES [dbo].[asset] ([SYSID])
GO
ALTER TABLE [dbo].[position] CHECK CONSTRAINT [FK_position_asset]
GO
ALTER TABLE [dbo].[position]  WITH CHECK ADD  CONSTRAINT [FK_position_strategy] FOREIGN KEY([strategySYSID])
REFERENCES [dbo].[strategy] ([SYSID])
GO
ALTER TABLE [dbo].[position] CHECK CONSTRAINT [FK_position_strategy]
GO
USE [master]
GO
ALTER DATABASE [tembo] SET  READ_WRITE 
GO
