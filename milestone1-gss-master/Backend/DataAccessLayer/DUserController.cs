using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.DataAccessLayer
{
    class DUserController : DalController
    {
        private const string UserTableName = "Users";

        public DUserController() : base(UserTableName)
        {

        }


        public List<DUser> SelectAllUsers()
        {
            List<DUser> result = Select().Cast<DUser>().ToList();

            return result;
        }



        public bool Insert(DUser user)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({DTO.IDColumnName} ,{DUser.UserEmailColumnName},{DUser.UserPasswordColumnName}) " +
                        $"VALUES (@idVal,@emailVal,@passwordVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", user.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", user.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", user.Password);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool DeleteAllUsers()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {UserTableName};";
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    //log error
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DUser result = new DUser((long)reader.GetValue(0), reader.GetString(1),reader.GetString(2));
            return result;

        }
    }
}

