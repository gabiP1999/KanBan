using System;
using System.Collections.Generic;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class BackendController
    {
        private Service Service { get; set; }
        public BackendController(Service service)
        {
            this.Service = service;
        }

        public BackendController()
        {
            this.Service = new Service();
            Service.LoadData();
        }

        internal List<TaskModel> GetInProgressTasks(UserModel userModel)
        {
            Response<IList<Task>> response = Service.InProgressTasks(userModel.Email);
            List < TaskModel >  tasks= new();
            if (response.ErrorOccured) return tasks;
            foreach (Task t in response.Value)
            {
                tasks.Add(new TaskModel(this, t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.emailAssignee, userModel.Email));
            }
            return tasks;
        }

        internal List<BoardModel> GetBoards(UserModel user)
        {
            Response<List<Board>> Response = Service.GetBoards(user.Email);
            List<BoardModel> boards = new();
            if (Response.ErrorOccured)
                return boards;
            foreach(Board b in Response.Value)
            {
                boards.Add(new BoardModel(this, b.Id, b.BoardName, b.CreatorEmail, user.Email));
            }
            return boards;
        }

        internal void SetColumnLimit(UserModel user, BoardModel board, int limit, ColumnModel col)
        {
            Response res = Service.LimitColumn(user.Email, board.CreatorEmail, board.BoardName, col.Id, limit);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
            }
            MessageBox.Show("Limit changed successfully!");
        }

        internal void SetColumnName(UserModel user, BoardModel board, ColumnModel col, string name)
        {
            Response res = Service.RenameColumn(user.Email, board.CreatorEmail, board.BoardName, col.Id, name);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
            }
            MessageBox.Show("Column name changed successfully!");
        }

        internal void DeleteData()
        {
            Service.DeleteData();
        }

        internal void AddColumn(UserModel user,BoardModel board,int columnOridinal, string name)
        {
            Response res = Service.AddColumn(user.Email, board.CreatorEmail, board.BoardName, columnOridinal, name);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
            }
            MessageBox.Show("Column created successfully!");
        }

        internal List<BoardModel> GetBoardsOther(UserModel user)
        {
            Response<List<Board>> Response = Service.GetBoardsOther(user.Email);
            List<BoardModel> boards = new();
            if (Response.ErrorOccured)
                return boards;
            foreach (Board b in Response.Value)
            {
                boards.Add(new BoardModel(this, b.Id, b.BoardName, b.CreatorEmail, user.Email));
            }
            return boards;
        }
        internal List<ColumnModel> GetColumns(UserModel user, BoardModel board)
        {
            Response<List<Column>> Response = Service.GetColumns(user.Email, board.CreatorEmail, board.BoardName);
            List<ColumnModel> columns = new();
            if (Response.ErrorOccured)
                return columns;
            foreach (Column c in Response.Value)
            {
                columns.Add(new ColumnModel(this, c.Id, c.Name, c.Limit, user.Email));
            }
            
            return columns;
        }
        internal List<TaskModel> GetTasks(UserModel user, BoardModel board, ColumnModel column)
        {
            Response<IList<Task>> Response = Service.GetColumn(user.Email, board.CreatorEmail, board.BoardName, column.Id);
            List<TaskModel> tasks = new();
            if (Response.ErrorOccured)
                return tasks;
            foreach (Task t in Response.Value)
            {
                tasks.Add(new TaskModel(this, t.Id, t.CreationTime, t.Title, t.Description, t.DueDate, t.emailAssignee, user.Email));
            }
            return tasks;
        }
        public UserModel Login(string username, string password)
        {
            Response<User> user = Service.Login(username, password);
            if (user.ErrorOccured)
            {
                MessageBox.Show(user.ErrorMessage);
                return null;
            }
            return new UserModel(this, username);
        }

        internal void RemoveColumn(string email, string creatorEmail, string boardName, int id)
        {
            Response res = Service.RemoveColumn(email, creatorEmail, boardName, id);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
            MessageBox.Show("Column was removed successfully");
        }

        internal string Register(string username, string password)
        {
            Response res = Service.Register(username, password);
            if (res.ErrorOccured)
            {
                
                return res.ErrorMessage;
            }
            return null;
        }

        internal string RemoveBoard(string email, string creatoremail, string boardname)
        {
            Response res = Service.RemoveBoard(email, creatoremail, boardname);
            if (res.ErrorOccured)
            {

                return res.ErrorMessage;
            }
            return null;
        }
        internal string JoinBoard(string email, string creatoremail, string boardname)
        {
            Response res = Service.JoinBoard(email, creatoremail, boardname);
            if (res.ErrorOccured)
            {
                return res.ErrorMessage;
            }
            return null;
        }
        internal void AddBoard(UserModel m, string title)
        {
            Response res = Service.AddBoard(m.Email, title);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
            MessageBox.Show("Board was created successfully");
        }
        internal void MoveColumn(UserModel user, BoardModel board,int columnOridinal,int shiftSize )
        {
            Response res = Service.MoveColumn(user.Email, board.CreatorEmail, board.BoardName, columnOridinal, shiftSize);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
            MessageBox.Show("Column moved successfully");
        }
        internal void AddTask(UserModel user, BoardModel board,string title,string description,DateTime dueDate)
        {
            Response res = Service.AddTask(user.Email, board.CreatorEmail, board.BoardName, title, description, dueDate);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
            MessageBox.Show("Task created successfully");
        }
        internal void UpdateTaskTitle(UserModel user, BoardModel board,ColumnModel column,TaskModel task, string title)
        {
            Response res = Service.UpdateTaskTitle(user.Email, board.CreatorEmail, board.BoardName, column.Id, task.Id, title);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
        }
        internal void UpdateTaskDescription(UserModel user, BoardModel board, ColumnModel column, TaskModel task, string description)
        {
            Response res = Service.UpdateTaskDescription(user.Email, board.CreatorEmail, board.BoardName, column.Id, task.Id, description);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
        }
        internal void UpdateTaskDueDate(UserModel user, BoardModel board, ColumnModel column, TaskModel task, DateTime duedate)
        {
            Response res = Service.UpdateTaskDueDate(user.Email, board.CreatorEmail, board.BoardName, column.Id, task.Id, duedate);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
        }
        internal string AdvanceTask(UserModel user, BoardModel board, ColumnModel column, TaskModel task)
        {
            Response res = Service.AdvanceTask(user.Email, board.CreatorEmail, board.BoardName, column.Id, task.Id);
            if (res.ErrorOccured)
            {
                return res.ErrorMessage;
            }
            return null;
        }
        internal void UpdateTaskAssignee(UserModel user, BoardModel board, ColumnModel column, TaskModel task,string assignee)
        {
            Response res = Service.AssignTask(user.Email, board.CreatorEmail, board.BoardName, column.Id, task.Id, assignee);
            if (res.ErrorOccured)
            {
                MessageBox.Show(res.ErrorMessage);
                return;
            }
        }




    }
}
