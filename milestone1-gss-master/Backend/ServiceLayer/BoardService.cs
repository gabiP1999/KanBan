using System.Collections.Generic;
using System;
using System.Linq;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using System.IO;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class BoardService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly BoardController boardController;


        public BoardService(LoggedInUser U)
        {
            boardController = new(U);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }

        internal Response LoadData()
        {
            try
            {
                boardController.LoadData();
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response DeleteData()
        {
            try
            {
                boardController.DeleteData();
                return new Response();

            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response AddBoard(string userEmail, string boardName)
        {
            try
            {
                boardController.AddBoard(userEmail, boardName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            try
            {
                boardController.LimitColumn(userEmail, creatorEmail, boardName, columnOrdinal, limit);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response<int> GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                int output = boardController.GetColumnLimit(userEmail, creatorEmail, boardName, columnOrdinal);
                return Response<int>.FromValue(output);
            }
            catch (Exception e)
            {
                return Response<int>.FromError(e.Message);
            }
        }

        internal Response<string> GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                string output = boardController.GetColumnName(userEmail, creatorEmail, boardName, columnOrdinal);
                return Response<string>.FromValue(output);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }
        }

        internal Response<Objects.Task> AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            try
            {
                BusinessLayer.Task t = boardController.AddTask(userEmail, creatorEmail, boardName, title, description, dueDate);
                Objects.Task stask = new Objects.Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate,t.Assignee);
                return Response<Objects.Task>.FromValue(stask);
            }
            catch (Exception e)
            {
                return Response<Objects.Task>.FromError(e.Message);
            }
        }

        internal Response JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                boardController.JoinBoard(userEmail, creatorEmail, boardName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            try
            {
                boardController.UpdateTaskDueDate(userEmail, creatorEmail, boardName, columnOrdinal, taskId, dueDate);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            try
            {
                boardController.UpdateTaskTitle(userEmail, creatorEmail, boardName, columnOrdinal, taskId, title);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            try
            {
                boardController.UpdateTaskDescription(userEmail, creatorEmail, boardName, columnOrdinal, taskId, description);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response AdvanceTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId)
        {
            try
            {
                boardController.AdvanceTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId);
                return new Response();

            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response<IList<Objects.Task>> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                IList<Objects.Task> output = new List<Objects.Task>();
                IList<BusinessLayer.Task> businessTasks = boardController.GetColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                foreach (BusinessLayer.Task t in businessTasks)
                {
                    Objects.Task newTask = new Objects.Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate,t.Assignee);
                    output.Add(newTask);
                }
                return Response<IList<Objects.Task>>.FromValue(output);

            }
            catch (Exception e)
            {
                return Response<IList<Objects.Task>>.FromError(e.Message);
            }
        }

        internal Response<IList<string>> GetBoardNames(string userEmail)
        {
            try
            {
                IList<string> output = boardController.GetBoardNames(userEmail);
                return Response<IList<string>>.FromValue(output);
            }
            catch (Exception e)
            {
                return Response<IList<string>>.FromError(e.Message);
            }
        }

        internal Response RemoveBoard(string userEmail, string creatorEmail, string boardName)
        {
            try
            {
                boardController.RemoveBoard(boardName, creatorEmail, userEmail);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response<IList<Objects.Task>> InProgressTasks(string userEmail)
        {
            try
            {
                IList<BusinessLayer.Task> tlist = boardController.GetInProgressAssignnedTasks(userEmail);
                IList<Objects.Task> output = new List<Objects.Task>();
                foreach (BusinessLayer.Task t in tlist)
                {
                    output.Add(new Objects.Task(t.ID, t.CreationTime, t.Title, t.Description, t.DueDate,t.Assignee));
                }
                return Response<IList<Objects.Task>>.FromValue(output);
            }
            catch (Exception e)
            {
                return Response<IList<Objects.Task>>.FromError(e.Message);
            }
        }

        internal Response AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            try
            {
                boardController.AssignTask(userEmail, creatorEmail, boardName, columnOrdinal, taskId, emailAssignee);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            try
            {
                boardController.AddColumn(userEmail, creatorEmail, boardName, columnOrdinal, columnName);
                return new Response(); 
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            try
            {
                boardController.RenameColumn(userEmail, creatorEmail, boardName, columnOrdinal, newColumnName);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }
        public Response RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            try
            {
                boardController.RemoveColumn(userEmail, creatorEmail, boardName, columnOrdinal);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }

        }

        internal Response MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            try
            {
                boardController.MoveColumn(userEmail, creatorEmail, boardName, columnOrdinal, shiftSize);
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response<List<Objects.Board>> GetBoards(string email)
        {
            try
            {
                IList<Board> boards = boardController.GetBoards(email);
                List<Objects.Board> objectBoards = new();
                foreach (Board b in boards)
                {
                    objectBoards.Add(new Objects.Board(b.Id, b.Name, b.CreatorEmail));
                }
                return Response<List<Objects.Board>>.FromValue(objectBoards);
            }
            catch(Exception e)
            {
                return Response<List<Objects.Board>>.FromError(e.Message);
            }
        }
        internal Response<List<Objects.Board>> GetBoardsOther(string email)
        {
            try
            {
                IList<Board> boards = boardController.GetBoardsOther(email);
                List<Objects.Board> objectBoards = new();
                foreach (Board b in boards)
                {
                    objectBoards.Add(new Objects.Board(b.Id, b.Name, b.CreatorEmail));
                }
                return Response<List<Objects.Board>>.FromValue(objectBoards);
            }
            catch (Exception e)
            {
                return Response<List<Objects.Board>>.FromError(e.Message);
            }
        }
        internal Response<List<Objects.Column>> GetColumns(string email, string creatorEmail, string boardName)
        {
            try
            {
                IList<Column> columns = boardController.GetColumns(email, creatorEmail, boardName);
                List<Objects.Column> objectColumns = new();
                foreach (Column c in columns)
                {
                    objectColumns.Add(new Objects.Column(c.ID, c.Name, c.Limit));
                }
                return Response<List<Objects.Column>>.FromValue(objectColumns);
            }
            catch (Exception e)
            {
                return Response<List<Objects.Column>>.FromError(e.Message);
            }
        }
    }
}