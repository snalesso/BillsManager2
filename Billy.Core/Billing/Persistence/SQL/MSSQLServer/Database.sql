DROP DATABASE [Billy]
GO

CREATE DATABASE [Billy]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Billy', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Billy.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Billy_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Billy_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 1024KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Billy].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Billy] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Billy] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Billy] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Billy] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Billy] SET ARITHABORT OFF 
GO

ALTER DATABASE [Billy] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Billy] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Billy] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Billy] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Billy] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Billy] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Billy] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Billy] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Billy] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Billy] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Billy] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Billy] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Billy] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Billy] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Billy] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Billy] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Billy] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Billy] SET RECOVERY FULL 
GO

ALTER DATABASE [Billy] SET  MULTI_USER 
GO

ALTER DATABASE [Billy] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Billy] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Billy] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Billy] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Billy] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Billy] SET QUERY_STORE = OFF
GO

ALTER DATABASE [Billy] SET  READ_WRITE 
GO

