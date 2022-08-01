using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.DataAccessLayer
{
    public class DMembersController : DalController
    {
        private const string MembersTableName = "Members";

        public DMembersController() : base(MembersTableName)
        {

        }


   


        public List<string> joinBetweenMembersAndBoards(long id)
        {
            List<string> res_members = new List<string>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string stm = $"SELECT * FROM {MembersTableName} WHERE {DTO.IDColumnName}={id}";
                SQLiteCommand command = new SQLiteCommand(stm, connection);
                try
                {
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        res_members.Add(reader.GetString(1));
                    }
                }
                catch
                {
                    Console.WriteLine("Error in Member loading");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res_members;
            }
        }

        internal bool DeleteAllMembers()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {MembersTableName};";
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

        public bool Insert(DMembers member)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {MembersTableName} ({DTO.IDColumnName},{DMembers.MembersColumnName}) " +
                        $"VALUES (@idVal,@membersVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", member.Id);
                    SQLiteParameter membersParam = new SQLiteParameter(@"membersVal", member.Members);


                    command.Parameters.Add(idParam);
                    command.Parameters.Add(membersParam);

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

        internal List<int> GetBoards(string email)
        {

            List<int> boards = new List<int>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string stm = $"SELECT * FROM {MembersTableName} WHERE {DMembers.MembersColumnName}='{email}'";
                SQLiteCommand command = new SQLiteCommand(stm, connection);
                try
                {
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        boards.Add(reader.GetInt32(0));
                    }
                }
                catch
                {
                    Console.WriteLine("Error in Member loading");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return boards;
            }
        }

        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            DMembers result = new DMembers((long)reader.GetValue(0), reader.GetString(1));
            return result;

        }
        public bool Remove(DMembers dMembers)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {MembersTableName} WHERE {DTO.IDColumnName}={dMembers.Id} AND {DMembers.MembersColumnName}='{dMembers.Members}';";

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
        public bool RemoveAllMembers()
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {MembersTableName};";

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
        public bool RemoveBoard(int id)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {MembersTableName} WHERE {DTO.IDColumnName}={id};";

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


    }
}

