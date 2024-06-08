CREATE TABLE [User] (
    UserId INT PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(50),
    LastLogin DATETIME,
    Status INT
);

EXEC sp_rename 'User', 'Users';

USE [UserRegistry]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 5/1/2024 2:18:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[LastLogin] [datetime] NULL,
	[Status] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


SELECT * FROM Users;