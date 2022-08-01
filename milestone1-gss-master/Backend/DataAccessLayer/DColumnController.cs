using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.DataAccessLayer
{
    class DColumnController : DalController
    {
        private const string ColumnTableName = "Column";

        public DColumnController() : base(ColumnTableName)
        {

        }


        public List<DColumn> SelectAllColumns()
        {
            List<DColumn> result = Select().Cast<DColumn>().ToList();

            return result;
        }
        internal List<DColumn> GetColumns(int id)
        {

            List<DColumn> columnslist = new List<DColumn>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string stm = $"SELECT * FROM {ColumnTableName} WHERE {DTO.IDColumnName}='{id}' ORDER by ColumnID";
                SQLiteCommand command = new SQLiteCommand(stm, connection);
                try
                {
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        columnslist.Add(new DColumn((long)reader.GetValue(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3)));
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return columnslist;
            }
        }




        public bool Insert(DColumn column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName}" +
                        $" ({DTO.IDColumnName} ,{DColumn.ColumnIdColumnName},{DColumn.NameColumnName}," +
                        $"{DColumn.LimitColumnName}) " +
                        $"VALUES (@idVal,@columnIDVal,@nameVal,@limitVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", column.Id);
                    SQLiteParameter columnIDParam = new SQLiteParameter(@"columnIDVal", column.ColumnId);
                    SQLiteParameter nameValParam = new SQLiteParameter(@"nameVal", column.Name);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", column.Limit);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(columnIDParam);
                    command.Parameters.Add(nameValParam);
                    command.Parameters.Add(limitParam);
                
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch(Exception e )
                {
                    Console.WriteLine(e);
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool DeleteAllColumns()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {ColumnTableName};";
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
            DColumn result = new DColumn((long)reader.GetValue(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3));
            return result;

        }
        public bool Remove(DColumn column)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {ColumnTableName} WHERE {DTO.IDColumnName}={column.Id} and {DColumn.ColumnIdColumnName}={column.ColumnId};";

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

