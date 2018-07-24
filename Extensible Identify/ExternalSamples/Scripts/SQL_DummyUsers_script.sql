
CREATE TABLE [dbo].[DummyUser](
	[UserName] [nchar](10) NOT NULL,
	[Password] [nchar](10) NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[IsDisabled] [bit] NOT NULL,
 CONSTRAINT [PK_DummyUser] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)

-- admin user
INSERT INTO [dbo].[DummyUser]([UserName], [Password], [IsLocked], [IsDisabled])
VALUES ('admin', 'dummy', 0, 0);
-- disabled user
INSERT INTO [dbo].[DummyUser]([UserName], [Password], [IsLocked], [IsDisabled])
VALUES ('user1', 'dummy', 0, 1);
-- locked user
INSERT INTO [dbo].[DummyUser]([UserName], [Password], [IsLocked], [IsDisabled])
VALUES ('user2', 'dummy', 1, 0);


