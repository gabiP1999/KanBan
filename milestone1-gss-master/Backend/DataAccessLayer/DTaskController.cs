using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.DataAccessLayer
{
    public class DTaskController : DalController
    {
        private const string TaskTableName = "Tasks";

        public DTaskController() : base(TaskTableName)
        {

        }


        public List<DTask> SelectAllTasks()
        {
            List<DTask> result = Select().Cast<DTask>().ToList();

            return result;
        }



        public bool Insert(DTask task)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({DTO.IDColumnName},{DTask.TitleColumnName}," +
                        $"{DTask.DueDateColumnName},{DTask.CreationTimeColumnName},{DTask.DescriptionColumnName}," +
                        $"{DTask.AssigneeEmailColumnName},{DTask.ColumnIdColumnName},{DTask.BoardIdColumnName}) " +
                        $"VALUES (@idVal,@TitleVal,@DueDateVal,@CreationTimeVal,@descriptionVal,@assigneeEmailVal,@columnIDval,@boardIDval);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", task.Id);
                    SQLiteParameter boardIDParam = new SQLiteParameter(@"boardIDval", task.BoardID);
                    SQLiteParameter titleParam = new SQLiteParameter(@"TitleVal", task.Title);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"DueDateVal", task.DueDate);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"CreationTimeVal", task.CreationTime);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", task.Description);
                    SQLiteParameter assignmeeEmailParam = new SQLiteParameter(@"assigneeEmailVal", task.AssigneeEmail);
                    SQLiteParameter columnIDParam = new SQLiteParameter(@"columnIDval", task.ColumnID);


                    command.Parameters.Add(idParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(assignmeeEmailParam);
                    command.Parameters.Add(columnIDParam);
                    command.Parameters.Add(boardIDParam);


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

        internal List<DTask> GetTasks(int id, int column)
        {
            List<DTask> tasks = new List<DTask>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string stm = $"SELECT * FROM {TaskTableName} WHERE {DTask.BoardIdColumnName}={id} AND {DTask.ColumnIdColumnName}={column};";
                SQLiteCommand command = new SQLiteCommand(stm, connection);
                try
                {
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();


                    while (reader.Read())
                    {
                        DTask t = (DTask)ConvertReaderToObject(reader);
                        tasks.Add(t);
                    }
                }
                catch
                {
                    Console.WriteLine("Error in Task loading");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return tasks;
            }
        }

        internal bool DeleteAllTasks()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {TaskTableName};";
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
            DTask result = new DTask((long)reader.GetValue(0), reader.GetString(1), reader.GetDateTime(2),
                reader.GetString(3),reader.GetDateTime(4),reader.GetInt32(5),reader.GetString(6),reader.GetInt32(7));
            return result;

        }

        internal bool RemoveBoard(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"DELETE FROM {TaskTableName} WHERE {DTask.BoardIdColumnName}={id};";

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

