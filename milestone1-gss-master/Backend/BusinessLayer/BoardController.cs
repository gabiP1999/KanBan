using IntroSE.Kanban.DataAccessLayer;
using IntroSE.Kanban.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        private Dictionary<string, Board> BoardList;
        private Dictionary<int, Board> BoardListInt;
        private Dictionary<string, IList<Task>> InProgressTasks;
        private Dictionary<string, IList<Board>> UserBoard;
        private int taskId;
        private int boardId;
        private DBoardController dboardController = new DBoardController();
        private DMembersController dmembersController = new DMembersController();
        private DTaskController dTaskController = new DTaskController();
        private DColumnController dcolumnController = new DColumnController();
        LoggedInUser Users;
        public BoardController(LoggedInUser u)
        {
            Users = u;
            taskId = 0;
            boardId = 0;
            BoardList = new Dictionary<string, Board>();
            BoardListInt = new Dictionary<int, Board>();
            InProgressTasks = new Dictionary<string, IList<Task>>();
            UserBoard = new Dictionary<string, IList<Board>>();
        }

        /// <summary>
        /// deleting all the data 
        /// </summary>
        /// <returns>void </returns>
        public void DeleteData()
        {
            dboardController.DeleteAllBoards();
            dmembersController.DeleteAllMembers();
            dTaskController.DeleteAllTasks();
            dcolumnController.DeleteAllColumns();
            taskId = 0;
            boardId = 0;
            BoardList = new Dictionary<string, Board>();
            BoardListInt = new Dictionary<int, Board>();
            InProgressTasks = new Dictionary<string, IList<Task>>();
            UserBoard = new Dictionary<string, IList<Board>>();
        }

        /// <summary>
        /// loading the data from the data base
        /// </summary>
        /// <returns></returns>
        public void LoadData()
        {
            
            List<DBoard> dboards = dboardController.SelectAllBoards();

            foreach (DBoard Dboard in dboards)
            {
                List<string> members = dmembersController.joinBetweenMembersAndBoards(Dboard.Id);
                List<DColumn> columns = dcolumnController.GetColumns((int)Dboard.Id);
                Board newBoard = new Board(Dboard, members,columns);
                foreach(string s in members)
                {
                    if (!InProgressTasks.ContainsKey(s))
                        InProgressTasks[s] = new List<Task>();
                    if (!UserBoard.ContainsKey(s))
                        UserBoard[s] = new List<Board>();
                        UserBoard[s].Add(newBoard);
                }
                int res = newBoard.LoadData(dTaskController);
                this.taskId += res;
                for(int i=1;i<newBoard.columnsList.Count-1;i++)
                foreach(Task t in newBoard.GetColumn(i).GetTasks().Values)
                {
                    if (!InProgressTasks.ContainsKey(t.assignee))
                    {
                        List<Task> userTasks= new List<Task>();
                        userTasks.Add(t);
                        InProgressTasks[t.assignee] = userTasks;
                    }
                    else
                    {
                        InProgressTasks[t.assignee].Add(t);
                    }
                }
                BoardList[Dboard.CreatorName + "-" + Dboard.CreatorEmail] = newBoard;
                BoardListInt[(int)Dboard.Id] = newBoard;
                boardId = (int)Dboard.Id+1;
                
            }
        }

        /// <summary>
        /// changing the limit column
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">creator's email</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">ordinal column</param>
        /// <param name="limit">limit of the column</param>
        /// <returns>void </returns>
        internal void LimitColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int limit)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            Board b =GetBoard(creatorEmail, boardName);
            if (!b.BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not a board member!");
            b.SetColumnLimit(columnOrdinal, limit);
        }

        /// <summary>
        /// return the column limit 
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">creator's email</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">ordinal column</param>
        /// <returns>int limit of the column </returns>
        internal int GetColumnLimit(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            return GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).Limit;
        }

        /// <summary>
        /// return the column name 
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">creator's email</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="columnOrdinal">ordinal column</param>
        /// <returns>name of the column </returns>
        internal string GetColumnName(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            return GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).Name;
        }

        internal Board GetBoard(int id)
        {
            return BoardListInt[id];
        }

        /// <summary>
        /// adding task to the board
        /// </summary>
        /// <param name="userEmail">user's email</param>
        /// <param name="creatorEmail">creator's email</param>
        /// <param name="boardName">name of the board</param>
        /// <param name="title">title of the task</param>
        /// <param name="description">description of the task</param>
        /// <param name="dueDate">due Date of the task</param>
        /// <returns>the task that eas added </returns>
        internal Task AddTask(string userEmail, string creatorEmail, string boardName, string title, string description, DateTime dueDate)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            Board b = GetBoard(creatorEmail, boardName);
            if (!b.BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not a member!");
            taskId++; //update the id number
            return b.AddTask(DateTime.Now, dueDate, title, description, taskId - 1, userEmail); //Adding new task to the board
        }



        /// <summary>
        /// Adds a new board to BoardList dictionary
        /// </summary>
        /// <param name="email">creator email</param>
        /// <param name="name">board name</param>
        /// <returns></returns>
        public Board AddBoard(string email, string name)
        {
            if (!Users.CheckLoggedIn(email)) throw new Exception(email + " is logged out!");
            int id = boardId;
            if (BoardExists(email,name)) throw new Exception("A board with the same ID exists");
            Board board = new Board(id,name, email,new DBoard(id,email,name));
            BoardList.Add(board.BoardId, board);
            BoardListInt.Add(id, board);
            if (!UserBoard.ContainsKey(email))
                UserBoard[email] = new List<Board>();
            UserBoard[email].Add(board);
            boardId += 1;
            return board;
        }

        

        internal void AddColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string columnName)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (!BoardExists(creatorEmail, boardName)) throw new Exception("The board is not exist");
            if (!BoardList[boardName + "-" + creatorEmail].BoardMembers.Contains(userEmail)) throw new Exception(userEmail + "The user is not a member");
            if (columnOrdinal<0) throw new Exception("The column Ordinal is not in the range- under 0");
            int columnsNum = BoardList[boardName + "-" + creatorEmail].columnsList.Count;
            if (columnOrdinal > columnsNum) throw new Exception("The column Ordinal is not in the range- up than the max");
            BoardList[boardName + "-" + creatorEmail].AddColumn(columnOrdinal, columnName);
            if(columnOrdinal == 0)
            {
                foreach(Task t in BoardList[boardName + "-" + creatorEmail].GetColumn(1).Tasks.Values)
                {
                    InProgressTasks[t.assignee].Add(t);
                }
            }
        }

        internal void MoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int shiftSize)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (!BoardExists(creatorEmail, boardName)) throw new Exception("The board is not exist");
            if (!BoardList[boardName + "-" + creatorEmail].BoardMembers.Contains(userEmail)) throw new Exception(userEmail + "The user is not a member");
            if (columnOrdinal < 0) throw new Exception("The column Ordinal is not in the range- under 0");
            int columnsNum = BoardList[boardName + "-" + creatorEmail].columnsList.Count;
            if (columnOrdinal >= columnsNum) throw new Exception("The column Ordinal is not in the range- up than the max");
            if (shiftSize == 0) return;
            BoardList[boardName + "-" + creatorEmail].MoveColumn(columnOrdinal, shiftSize);
            if(columnOrdinal+shiftSize == 0)
            {
                foreach(Task t in BoardList[boardName + "-" + creatorEmail].columnsList[1].Tasks.Values)
                {
                    InProgressTasks[t.Assignee].Add(t);
                }
            }
            if (columnOrdinal == 0)
            {
                foreach (Task t in BoardList[boardName + "-" + creatorEmail].columnsList[0].Tasks.Values)
                {
                    InProgressTasks[t.Assignee].Remove(t);
                }

            }
        }

        internal IList<Board> GetBoards(string userEmail)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            return UserBoard[userEmail];
        }
        internal IList<Board> GetBoardsOther(string userEmail)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            IList<Board> boards = new List<Board>();
            foreach (Board b in BoardList.Values)
                if (!b.BoardMembers.Contains(userEmail))
                    boards.Add(b);
            return boards;
        }
        internal IList<Column> GetColumns(string email, string creatorEmail, string boardName)
        {
            if (!Users.CheckLoggedIn(email)) throw new Exception(email + " is logged out!");
            if (!BoardList.ContainsKey(boardName + "-" + creatorEmail)) throw new Exception("No such board" + boardName);
            return BoardList[boardName + "-" + creatorEmail].columnsList;
            
        }

        /// <summary>
        /// updating the due dat's task
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="dueDate">due date of the task</param>
        /// <returns> void </returns>
        internal void UpdateTaskDueDate(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (columnOrdinal == 2) throw new Exception("Once a task is done its due date cannot be changed!");
            GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).GetTask(taskId).SetDueDate(dueDate, userEmail);
        }

        internal void RenameColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal, string newColumnName)
        {
            if(!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (!BoardExists(creatorEmail, boardName)) throw new Exception("the board is not exists");
            if (!BoardList[boardName + "-" + creatorEmail].BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not a board member, not allowed to rename Column!");
            int columnNum = BoardList[boardName + "-" + creatorEmail].columnsList.Count;
            if (columnNum - 1 < columnOrdinal || columnOrdinal<0)
                throw new Exception("column " + columnOrdinal + "does not exist");
            BoardList[boardName + "-" + creatorEmail].columnsList[columnOrdinal].Name = newColumnName;
           
        }

        internal void RemoveColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (!BoardExists(creatorEmail, boardName)) throw new Exception("the board is not exists");
            if (!BoardList[boardName + "-" + creatorEmail].BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not a board member, not allowed to remove Column!");
            int columnNum = BoardList[boardName + "-" + creatorEmail].columnsList.Count;
            if (columnNum <= columnOrdinal || columnOrdinal<0)
                throw new Exception("columnOrdinal is illegal");
            if (columnNum == 2)
                throw new Exception("removing column is forbidden, because the number of columns is less or equal to 2");
            Dictionary<int, Task> tasks = new();
            if (columnOrdinal == 1) {
                tasks = BoardList[boardName + "-" + creatorEmail].columnsList[columnOrdinal].GetTasks();
            }
            if (columnOrdinal == columnNum - 1)
            {
                tasks = BoardList[boardName + "-" + creatorEmail].columnsList[columnOrdinal - 1].GetTasks();
            }
            BoardList[boardName + "-" + creatorEmail].RemoveColumn(columnOrdinal);
            if (columnOrdinal == 1 || columnOrdinal == columnNum-1 )
            { 
                foreach(Task t in tasks.Values)
                {
                    InProgressTasks[t.assignee].Remove(t);
                }
            }
          
        }

        /// <summary>
        /// updating the title's task
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="title">title of the task</param>
        /// <returns> void </returns>
        internal void UpdateTaskTitle(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string title)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).GetTask(taskId).ChangeTitle(title,userEmail);

        }

        /// <summary>
        /// updating the description's task
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="taskId">id of the task</param>
        /// <param name="description">description of the task</param>
        /// <returns> void </returns>
        internal void UpdateTaskDescription(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string description)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            GetBoard(creatorEmail, boardName).GetColumn(columnOrdinal).GetTask(taskId).ChangeDescription(description, userEmail);
        }

        /// <summary>
        /// checking if the boards exists
        /// </summary>
        /// <param name="email">creator email</param>
        /// <param name="name">name of the board</param>
        /// <returns> void </returns>
        private bool BoardExists(string email, string name)
        {
            foreach(Board b in BoardList.Values)
            {
                if (b.CreatorEmail.Equals(email) && b.Name.Equals(name))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// returning the board with this creator and name
        /// </summary>
        /// <param name="email">creator email</param>
        /// <param name="name">name of the board</param>
        /// <returns> board </returns>
        public Board GetBoard (string email,string name)
        {
            string id = name + "-" + email;
            return BoardList[id];
        }

        /// <summary>
        /// return the exsact column
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <returns> list of task </returns>
        internal IList<Task> GetColumn(string userEmail, string creatorEmail, string boardName, int columnOrdinal)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            Board b = GetBoard(creatorEmail, boardName);
            if (!b.BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not member");
            return b.GetColumn(columnOrdinal).GetTasksList();
        }


        /// <summary>
        /// Removes a board from BoardList given its ID
        /// </summary>
        /// <param name="id">Boards ID</param>
        /// <returns></returns>
        public void RemoveBoard(string name,string BoardCreatorEmail,string userEmail)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            Board tmp = GetBoard(BoardCreatorEmail, name);
            if (!userEmail.Equals(tmp.CreatorEmail))
                throw new Exception("Board can be deleted only by it's creator!");
            List<string> members = tmp.BoardMembers;
            foreach(string m in members)
            {
                UserBoard[m].Remove(tmp);
            }
            tmp.Delete();
            dmembersController.RemoveBoard(tmp.Id);
            dTaskController.RemoveBoard(tmp.Id);
            BoardList.Remove(tmp.BoardId);
            BoardListInt.Remove(tmp.Id);
        }

        /// <summary>
        /// return all the name board
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <returns> list of boards name</returns>
        internal IList<string> GetBoardNames(string userEmail)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            if (UserBoard.ContainsKey(userEmail))
            {
                IList<string> names = new List<string>();
                foreach(Board b in UserBoard[userEmail])
                {
                    names.Add(b.Name);
                }
                return names;
            }
            return new List<string>();
        }

        /// <summary>
        /// Advanced a Task from backlog to in progress or from in progress to done
        /// </summary>
        /// <param name="email">Board creator email</param>
        /// <param name="boardName">Board name</param>
        /// <param name="columnOrdinal">column number</param>
        /// <param name="taskId">Task ID</param>
        /// <returns> void</returns>
       
        public void AdvanceTask(string userEmail,string email, string boardName, int columnOrdinal, int taskId)        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out!");
            Board b = GetBoard(email, boardName);
            Task t = b.GetColumn(columnOrdinal).GetTask(taskId);
            if (!t.assignee.Equals(userEmail)) 
                throw new Exception("Task can be advanced by assignee only!");
            if(b.columnsList.Count-1 == columnOrdinal)
                throw new Exception("cant advance task in the last column");
            b.AdvanceTask(columnOrdinal, taskId);
            if (columnOrdinal == 0 && b.columnsList.Count > 2)
            {
                if (!InProgressTasks.ContainsKey(userEmail))
                {
                    IList<Task> lst = new List<Task>();
                    InProgressTasks[userEmail] = lst;
                }
                    InProgressTasks[userEmail].Add(t);
                

            }
            if (columnOrdinal == b.columnsList.Count - 2)
                InProgressTasks[userEmail].Remove(t);
       

          //  if (columnOrdinal == 0)
            //{ checking if the task in backlog column
            //    b.ToInProgress(t);
                //if (!InProgressTasks.ContainsKey(t.GetAssignee()))
              //  {
                  //  List<Task> tasks = new List<Task>();
                    //tasks.Add(t);
                   // InProgressTasks[t.GetAssignee()] = tasks;
               // }
               // else
              //  {
                //    InProgressTasks[t.GetAssignee()].Add(t);
               // }
            //}
           // else if (columnOrdinal == 1)
            //{
                // checking if the task in progress column 
              //  b.ToDone(t);
               // InProgressTasks[t.GetAssignee()].Remove(t);
           // }

        //    else
           //     throw new Exception("Invalid ColumnOrdinal"); 
            // if the task is not in backlog or in progress column it is not possible to move it 
        }

        /// <summary>
        /// adding board name
        /// </summary>
        /// <param name="userEmail">Board creator email</param>
        /// <param name="creatorEmail">Board creator email</param>
        /// <param name="boardName">Board name</param
        /// <returns> void </returns>

        internal void JoinBoard(string userEmail, string creatorEmail, string boardName)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out. ");
            GetBoard(creatorEmail, boardName).AddMember(userEmail);
            if (!UserBoard.ContainsKey(userEmail))
                UserBoard[userEmail] = new List<Board>();
            UserBoard[userEmail].Add(GetBoard(creatorEmail, boardName));
        }

        /// <summary>
        /// Get all In Progress Assignned Tasks
        /// </summary>
        /// <param name="email">creator email</param>
        /// <returns> list of tasks </returns>
        public IList<Task> GetInProgressAssignnedTasks(string email)
        {
            if (!InProgressTasks.ContainsKey(email))
                return new List<Task>();
            return InProgressTasks[email];
        }


        /// <summary>
        /// adding the assignee task
        /// </summary>
        /// <param name="userEmail">user email</param>
        /// <param name="creatorEmail">creator email</param>
        /// <param name="boardName">board name</param>
        /// <param name="columnOrdinal">column Ordinal</param>
        /// <param name="taskId">board name</param>
        /// <param name="emailAssignee">column Ordinal</param>
        /// <returns> void </returns>
        public void AssignTask(string userEmail, string creatorEmail, string boardName, int columnOrdinal, int taskId, string emailAssignee)
        {
            if (!Users.CheckLoggedIn(userEmail)) throw new Exception(userEmail + " is logged out.");
            Board b = GetBoard(creatorEmail, boardName);
            if (!b.BoardMembers.Contains(userEmail)) throw new Exception(userEmail + " is not a board member, not allowed to assign tasks!");
            if (!b.BoardMembers.Contains(emailAssignee)) throw new Exception(emailAssignee + " is not a board member, cannot be assigned to tasks!");
            Task t =b.GetColumn(columnOrdinal).GetTask(taskId);
            
                string oldAssignee = t.assignee;
                t.SetAssignee(emailAssignee);
            if (columnOrdinal == 1)
            {
                InProgressTasks[oldAssignee].Remove(t);
                if (!InProgressTasks.ContainsKey(emailAssignee))
                {
                    List<Task> tasks = new List<Task>();
                    tasks.Add(t);
                    InProgressTasks[emailAssignee] = tasks;
                }
                else
                {
                    InProgressTasks[emailAssignee].Add(t);
                }
            }
            
        }
    }
}