using IntroSE.Kanban.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Board
    {
        private int id;
        public int Id { get => id; set { id = value; } }
        private string boardId;
        public string BoardId { get => boardId; set { boardId = value; } }
        private string name;
        public string Name { get => name; set {name = value; } }
        private string creatorEmail;
        public string CreatorEmail { get => creatorEmail; set { creatorEmail = value; } }
        private List<string> boardMembers;
        public List<String> BoardMembers { get => boardMembers; set { boardMembers = value;} }
        private DBoard dboard;
        internal List<Column> columnsList;


        public Board(int id, string name, string email, DBoard dboard) //constructor 
        {
            this.BoardMembers = new List<string>();
            NameCheck(name);
            this.name = name;
            this.creatorEmail = email;
            this.id = id;
            this.boardId = name + "-" + email;
            this.dboard = dboard;
            this.AddMember(email);
            columnsList = new List<Column>();
            columnsList.Add(new Column(0, "backlog", new DColumn(id, 0, "backlog", -1)));
            columnsList.Add(new Column(1, "inprogress", new DColumn(id, 1, "inprogress", -1)));
            columnsList.Add(new Column(2, "done", new DColumn(id, 2, "done", -1)));
            dboard.insertBoard();
        }

        private void NameCheck(string name)
        {
            if (name.Length == 0) throw new Exception("Can not create a board with empty name!");
            if (name[0].Equals(' ')) throw new Exception("Statring a board name with a space is forbidden!");
        }

        public Board(DBoard dboard, List<string> dmembers, List<DColumn> columnList) {
            this.name = dboard.CreatorName;
            this.creatorEmail = dboard.CreatorEmail;
            this.id = (int)dboard.Id;
            BoardMembers = dmembers;
            this.dboard = dboard;
            columnsList = new List<Column>();
            foreach (DColumn c in columnList) {
                columnsList.Add(new Column(c));
            }

        }

        /// <summary>
        /// loading the date
        /// </summary>
        /// <param name="dTaskController">data Task controller</param>
        /// <returns>int </returns>
        public int LoadData(DataAccessLayer.DTaskController dTaskController)
        {
            int a = 0;
            foreach (Column c in columnsList)
                a += c.LoadData(this.id, dTaskController);
            return a;

           // int a = this.backlog.LoadData(this.id, dTaskController);
            //int b = this.inprogress.LoadData(this.id, dTaskController);
            //int c = this.done.LoadData(this.id, dTaskController);
            //id += a + b + c;
            //return a + b + c;
            
        }

        /// <summary>
        /// Adds a new task to the backlog column
        /// </summary>
        /// <param name="creationTime">current time</param>
        /// <param name="dueDate">specipied due date</param>
        /// <param name="title">task title</param>
        /// <param name="description">task description</param>
        /// <param name="taskIndex">task index generated by the board controller</param>
        /// <returns></returns>
        public Task AddTask(DateTime creationTime, DateTime dueDate, string title, string description, int taskIndex, string taskCreator)
        {
            if (!BoardMembers.Contains(taskCreator))
                throw new Exception("Only board members can add tasks!");
            Task newTask = new Task(this.Id, taskIndex, creationTime, dueDate, title, description, taskCreator); // Creat a new Task
            this.columnsList[0].AddTask(newTask); // Adding the Task to the backlog column 
                        return newTask; // Return the the added class 
        }



        /// <summary>
        /// Advances a task from the backlog column to in progress
        /// </summary>
        /// <param name="task">task that is currently in backlog</param>
        /// 

        public void AdvanceTask(int columnOrdinal, int taskId) {
            Task task = columnsList[columnOrdinal].GetTask(taskId);
            task.ColumnID++;
            columnsList[columnOrdinal].RemoveTask(task);
            columnsList[columnOrdinal + 1].AddTask(task);
        }
  //      public void ToInProgress(Task task)
    //    {

      //      if (!backlog.IsBelong(task)) throw new Exception("Task does not belong to backlog");
        //    if (inProgress.IsBelong(task)) throw new Exception("Task already belongs to in Progress");
         //   task.SetColumnID(1);
          //  backlog.RemoveTask(task); // Removing the Task from backlog column 
          //  inProgress.AddTask(task); // Adding the Task to in progress column 

//        }

        /// <summary>
        /// Advances a task from in progress to done column
        /// </summary>
        /// <param name="task">task that is currently in in progress column</param>
  //      public void ToDone(Task task)
    //    {
      //      if (!inProgress.IsBelong(task)) throw new Exception("Task does not belong to in Progress");
        //    if (done.IsBelong(task)) throw new Exception("Task already belongs to done");
        //    inProgress.RemoveTask(task); // Removing the Task from in progress column 
         //   task.SetColumnID(2);
          //  task.SetDone();
           // done.AddTask(task); // Adding the Task to done
      //  }

      
        //getters functions:


        /// <summary>
        /// chamge the column limit
        /// </summary>
        /// <param name="column"> number of a column</param>
        /// <param name="limit"> the limit of the column</param>
        /// <returns>void </returns>
        public void SetColumnLimit(int column, int limit)
        {
            if (limit <=0) throw new Exception("Limit has to be above 0");
            if (column > columnsList.Count-1 || column < 0) throw new Exception("Column ID is illegal");
            GetColumn(column).Limit = limit;
        }

        /// <summary>
        /// return a column number a
        /// </summary>
        /// <param name="a"> a number of the column</param>
        /// <returns>column </returns>
        public Column GetColumn(int a)
        {
            if(a<0 || a>=columnsList.Count)
                throw new Exception("there is no such column");
            return columnsList[a];
        }

        /// <summary>
        /// adding a member to the board
        /// </summary>
        /// <param name="email"> email of the memeber</param>
        /// <returns>Task </returns>
        public void AddMember(string email)
        {
            if (BoardMembers.Contains(email))
                throw new Exception("User is already a member!");
            boardMembers.Add(email);
            DataAccessLayer.DTOs.DMembers dMember = new DMembers(this.id, email);
            dMember.InsertMember();
        }

        /// <summary>
        /// removing the member with that email
        /// </summary>
        /// <param name="email"> email of the member</param>
        /// <returns>void </returns>
        public void RemoveMember(string email)
        {
            if (!BoardMembers.Contains(email))
                throw new Exception("Can not remove a user because user is not a member!");
            BoardMembers.Remove(email);
            DataAccessLayer.DTOs.DMembers dMember = new DMembers(this.id, email);
            dMember.RemoveMember();
        }

        /// <summary>
        /// return a task with that id
        /// </summary>
        /// <param name="column"> number of a column</param>
        /// <param name="taskid"> id of a Task</param>
        /// <returns>Task </returns>
        public Task GetTask(int column, int taskid)
        {
            return GetColumn(column).GetTask(taskid);
        }

        /// <summary>
        /// change the task assignee
        /// </summary>
        /// <param name="column"> number of a column</param>
        /// <param name="taskid"> id of a Task</param>
        /// <param name="ass"> ass</param>
        /// <returns>void </returns>
        public void SetTaskAssignee(int column, int taskid, string ass)
        {
            if (!BoardMembers.Contains(ass))
                throw new Exception("Task can't be assigned to users who are not board members!");
            GetTask(column, taskid).SetAssignee(ass);
        }

        /// <summary>
        /// return all the tatsks
        /// </summary>
        /// <returns>list of task </returns>
        public IList<Task> GetAllTasks()
        {
            List<Task> list = new List<Task>();
            foreach (Column c in columnsList)
                list.AddRange(c.GetTasksList());
         // list.AddRange(backlog.GetTasksList());
         // list.AddRange(inProgress.GetTasksList());
         // list.AddRange(done.GetTasksList());
            return list;
        }

        /// <summary>
        /// deleting the data
        /// </summary>
        /// <returns>void </returns>
        internal void Delete()
        {
            dboard.Delete();
            foreach (Column c in columnsList)
                c.Delete();
            //GetColumn(0).Delete();
            //GetColumn(1).Delete();
            //GetColumn(2).Delete();
        }


        internal void AddColumn(int columnOrdinal, string columnName)
        {
            for (int i = columnsList.Count - 1; i >= columnOrdinal; i--)
                columnsList[i].ID += 1;
            columnsList.Insert(columnOrdinal, new Column(columnOrdinal, columnName, new DColumn(id, columnOrdinal, columnName, -1)));
        }


        internal void RemoveColumn(int columnOrdinal)
        {
            if (columnOrdinal > 0)
            {
                if (columnsList[columnOrdinal - 1].Limit != -1 && columnsList[columnOrdinal].GetTasks().Count + columnsList[columnOrdinal - 1].GetTasks().Count > columnsList[columnOrdinal - 1].Limit)
                    throw new Exception("there is no option to move this column to the previus column");
                Dictionary<int, Task> tasks = columnsList[columnOrdinal].GetTasks();
                columnsList[columnOrdinal].dColumn.Delete();
                columnsList.RemoveAt(columnOrdinal);
                for (int i = columnOrdinal; i < columnsList.Count; i++)
                {
                    columnsList[i].ID -= 1;
                }
                int idtask = columnsList[columnOrdinal - 1].Tasks.Count;
                foreach (Task t in tasks.Values)
                {
                    t.ColumnID = columnOrdinal;
                    t.ID = idtask;
                    idtask++;
                    columnsList[columnOrdinal - 1].Tasks.Add(idtask, t);

                }
            }
            else
            {
                if (columnsList[columnOrdinal+1].Limit != -1 && columnsList[columnOrdinal].GetTasks().Count + columnsList[columnOrdinal + 1].GetTasks().Count > columnsList[columnOrdinal + 1].Limit)
                    throw new Exception("there is no option to move this column to the next column");
                Dictionary<int, Task> tasks = columnsList[columnOrdinal].GetTasks();
                columnsList[columnOrdinal].dColumn.Delete();
                columnsList.RemoveAt(columnOrdinal);
                for (int i = columnOrdinal; i < columnsList.Count; i++)
                {
                    Console.WriteLine("shir test: id value is: "+columnsList[i].ID);
                    columnsList[i].ID -= 1;
                }
                int idtask = columnsList[columnOrdinal + 1].Tasks.Count;
                foreach (Task t in tasks.Values)
                {
                    t.ID = idtask;
                    idtask++;
                    columnsList[columnOrdinal + 1].Tasks.Add(idtask, t);
                }
            }
        }
        internal void MoveColumn(int columnOrdinal, int shiftSize)
        {
            if (columnsList[columnOrdinal].Tasks.Count != 0)
                throw new Exception("Cant move a column with tasks!");
            if (columnOrdinal + shiftSize < 0 || columnOrdinal + shiftSize >= columnsList.Count)
                throw new Exception("shiftSize out of range!");
            columnsList[columnOrdinal].ID = -1;
            if (shiftSize>0)
            {
                for (int i = columnOrdinal + 1; i <= columnOrdinal+shiftSize; i++)
                    columnsList[i].ID -= 1;
            }
            else
            {
                for (int i = columnOrdinal - 1; i >= columnOrdinal + shiftSize; i--)
                    columnsList[i].ID += 1;
            }
            columnsList[columnOrdinal].ID = columnOrdinal + shiftSize;
            Column column = columnsList[columnOrdinal];
            columnsList.RemoveAt(columnOrdinal);
            columnsList.Insert(columnOrdinal + shiftSize, column);
        }
        

    }
}
 