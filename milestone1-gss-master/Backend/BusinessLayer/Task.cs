using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        private int columnID;
        public int ColumnID { get => columnID; set { columnID = value; DTask.ColumnID = value; } }
        private int id;
        public int ID { get => id; set { id = value; DTask.Id = value; } }
        private int boardID;
        public int BoardID { get => boardID; set { BoardID = value; DTask.BoardID = value; } }
        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; set { creationTime = value; DTask.CreationTime = value; } }

        private DateTime dueDate;

        public DateTime DueDate { get => dueDate; set { dueDate = value; DTask.DueDate = value; } }
        private string description;
        public string Description { get => description; set { description = value; DTask.Description = value; } }
        private string title;
        public string Title { get => title; set { title = value; DTask.Title = value; } }
        private bool isDone;
        internal string assignee;
        public string Assignee { get => assignee; set { assignee = value; DTask.AssigneeEmail = value; } }
        private int MAX_LENGTH_DESCRIPTION = 300;
        private int MAX_LENGTH_TITLE = 50;
        private int MIN_LENGTH_TITLE = 0;
        private DataAccessLayer.DTOs.DTask DTask;

        
        public Task(int bID,int id, DateTime creationTime, DateTime dueDate, string title, string description ,string ass) // Constructor
        {
            this.columnID = 0;
            if (dueDate < DateTime.Now)
                throw new Exception("dueDate cant be before today");
            this.id = id;
            this.boardID = bID;
            this.creationTime = creationTime;
            this.dueDate = dueDate;
            if (!ValidateTitle(title)) throw new Exception("Invalid title");
            this.title = title;
            if (!ValidateDescription(description)) throw new Exception("Invalid description");
            this.description = description;
            isDone = false;
            assignee = ass;
            DTask = new DataAccessLayer.DTOs.DTask(id, title, dueDate, description, creationTime, columnID, ass, bID);
            DTask.InsertTask();
            
            
        }
        public Task(DataAccessLayer.DTOs.DTask dTask)
        {
            this.assignee = dTask.AssigneeEmail;
            this.id = (int)dTask.Id;
            this.boardID = dTask.BoardID;
            this.columnID = dTask.ColumnID;
            this.creationTime = dTask.CreationTime;
            this.dueDate = dTask.DueDate;
            this.description = dTask.Description;
            this.title = dTask.Title;
            if (columnID.Equals(2))
                isDone = true;
            else isDone = false;
            this.DTask = dTask;
        }


      

        /// <summary>
        /// Change the task's title
        /// </summary>
        /// <param name="title">new title</param>
        public void ChangeTitle(string title,string email)
        {
            if (!email.Equals(assignee)) throw new Exception("Title can be changed by assignee only!");
            if (!ValidateTitle(title)) throw new Exception("Invalid title");
            this.title = title;
            DTask.Title = title;
            
            
        }

        /// <summary>
        /// Returns true if the title is valid (isn't null and <=50 chars)
        /// </summary>
        /// <param name="title">Title</param>
        /// <returns></returns>
        private bool ValidateTitle(string title)
        {
            return title != null &&  title.Length> MIN_LENGTH_TITLE && title.Length <= MAX_LENGTH_TITLE;

        }

        /// <summary>
        /// Returns true if description is valid (length<= 300 chars)
        /// </summary>
        /// <param name="description">Description</param>
        /// <returns></returns>
        private bool ValidateDescription(string description)
        {
            return description.Length <= MAX_LENGTH_DESCRIPTION;

        }

        /// <summary>
        /// Change task's description
        /// </summary>
        /// <param name="description">new description</param>
        public void ChangeDescription(string description,string email)
        {
            if (!email.Equals(assignee)) throw new Exception("Description can be changed by assignee only!");
            if (!ValidateDescription(description)) throw new Exception("Invalid description");
            this.description = description;
            DTask.Description = description;
            
        }


        // Update the status to done 
        public void SetDone()
        {
            this.isDone = true;
        }

        /// <summary>
        /// deleting the data task
        /// </summary>
        /// <returns>void </returns>
        internal void Delete()
        {
            DTask.Delete();
        }

        /// <summary>
        /// Change task's due date
        /// </summary>
        /// <param name="dueDate">new due date</param>
        public void SetDueDate(DateTime dueDate,string email)
        {
            if (!email.Equals(assignee)) throw new Exception("Due date can be changed by assignee only!");
            if (isDone)
                throw new Exception("task is done");
            if (dueDate<DateTime.Now)
                throw new Exception("dueDate cant be before today"); 
         

            this.dueDate = dueDate;
            DTask.DueDate = dueDate;
        }
        public void SetAssignee(string email)
        {
            assignee = email;
        }

    
     
        public void SetColumnID(int n)
        {
            if (n > 2 || n < 0) throw new Exception("Ilegal column number");
            this.columnID = n;
            DTask.ColumnID = n;
            
        }
        public new string ToString => "#Task# ID-" + id.ToString() + " Title-" + title;
    }
}