using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.DataAccessLayer
{
    class DBoardController : DalController
    {
        private const string BoardTableName = "Board";

        public DBoardController() : base(BoardTableName)
        {

        }


        public List<DBoard> SelectAllBoards()
        {
            List<DBoard> result = Select().Cast<DBoard>().ToList();

            return result;
        }



        public bool Insert(DBoard board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName}" +
                        $" ({DTO.IDColumnName} ,{DBoard.BoardNameColumnName},{DBoard.CreatorEmailColumnName})" +
                        $"VALUES (@idVal,@creatorNameVal,@creatorEmailVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.Id);
                    SQLiteParameter creatorNameParam = new SQLiteParameter(@"creatorNameVal", board.CreatorName);
                    SQLiteParameter creatorEmailParam = new SQLiteParameter(@"creatorEmailVal", board.CreatorEmail);
        



                    command.Parameters.Add(idParam);
                    command.Parameters.Add(creatorNameParam);
                    command.Parameters.Add(creatorEmailParam);
            


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

        internal bool DeleteAllBoards()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {BoardTableName};";
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
            DBoard result = new DBoard((long)reader.GetValue(0), reader.GetString(1), reader.GetString(2));
            return result;

        }
        public bool Remove(DBoard board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {BoardTableName} WHERE {DTO.IDColumnName}={board.Id};";

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
        public bool RemoveAllBoards()
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {BoardTableName};";

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

