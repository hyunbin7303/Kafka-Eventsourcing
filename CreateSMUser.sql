Use SocialMedia
GO

IF NOT EXISTS(SELECT * FROM sys.server_principals where name = 'SMUser')
BEGIN
	CREATE LOGIN SMUser with password = N'SmPA$$06500', Default_Database= SocialMedia
END

IF NOT EXISTS(SELECT * FROM sys.database_principals where name = 'SMUser')
BEGIN
	EXEC sp_adduser 'SMUser', 'SMUser', 'db_owner';
END 