# How to use DummyGenericValidatorUsernamePassword

Use the script in Scripts\SQL_DummyUsers_script.sql to create DummyUser table on the database for testing.

The installed users are:
1. admin/dummy  -> can login successfully
2. user1/dummy  -> user is disabled
3. user2/dummy  -> user is locked

How to use:
1. Create DummyUser table
2. Copy all views to runtime
3. Create generic connection
 - Add key: ConnectionString (case sensitive) and the connection string to connect to your installed database which contains DummyUser table
 - View name: file name without the cshtml extension. E.g. DummyGenericUsernamePasswordLogin
 - Login with the generic connection
4. Copy all resources files to \runtime\App_GlobalResources. Please note that text resources files' properties must be set correctly:
 - Build Action: None
 - Copy to output: Do not copy
 - Custom tool: GlobalResourceProxyGenerator