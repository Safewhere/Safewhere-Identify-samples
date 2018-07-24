using System;
using System.Data.SqlClient;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// Dummy user model
    /// </summary>
    public class DummyUser
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsLocked { get; set; }
    }

    /// <summary>
    /// A repository which returns users from a sample user store.
    /// </summary>
    public class DummyUserRepository
    {
        private readonly string connectionString;

        public DummyUserRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            this.connectionString = connectionString;
        }

        internal DummyUser GetUserByName(string username)
        {
            DummyUser user = null;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = new SqlCommand("SELECT [UserName], [Password], [IsLocked], [IsDisabled] FROM [dbo].[DummyUser] WHERE [UserName] = @UserName", connection);
                command.Parameters.AddWithValue("@UserName", username);
                
                var dataReader = command.ExecuteReader();
                if (dataReader.Read())
                {
                    user = new DummyUser
                               {
                                   Username = username,
                                   Password = dataReader.GetString(dataReader.GetOrdinal("Password")),
                                   IsDisabled = dataReader.GetBoolean(dataReader.GetOrdinal("IsDisabled")),
                                   IsLocked = dataReader.GetBoolean(dataReader.GetOrdinal("IsLocked"))
                               };
                }
            }

            return user;
        }
    }
}
