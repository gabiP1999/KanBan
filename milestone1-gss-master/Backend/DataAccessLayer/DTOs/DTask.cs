using System;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
    public class DTask : DTO
    {
        public const string TitleColumnName = "Title";
        public const string DescriptionColumnName = "Description";
        public const string CreationTimeColumnName = "CreationTime";
        public const string DueDateColumnName = "DueDate";
        public const string AssigneeEmailColumnName = "AssigneeEmail";
        public const string ColumnIdColumnName = "ColumnID";
        public const string BoardIdColumnName = "BoardID";





        private string _title;
        public string Title { get => _title; set { _title = value; _controller.Update(Id, TitleColumnName, value); } }
        private string _desctiption;
        public string Description { get => _desctiption; set { _desctiption = value; _controller.Update(Id, DescriptionColumnName, value); } }
        private DateTime _creationTime;
        public DateTime CreationTime { get => _creationTime; set { _creationTime = value; _controller.Update(Id, CreationTimeColumnName, value); } }

        private DateTime _dueDate;
        public DateTime DueDate { get => _dueDate; set { _dueDate = value; _controller.Update(Id, DueDateColumnName, value); } }
        private string _asigneeEmail;
        public string AssigneeEmail { get => _asigneeEmail; set { _asigneeEmail = value; _controller.Update(Id, AssigneeEmailColumnName, value); } }

        private int _columnID;
        public int ColumnID { get => _columnID; set { _columnID = value; _controller.Update(Id, ColumnIdColumnName, value); } }

       

        private int _boardID;
        public int BoardID { get => _boardID; set { _boardID = value; _controller.Update(Id, BoardIdColumnName, value); } }


        public DTask(long ID, string title, DateTime dueDate,string description,DateTime creationTime,int columnID,string assigneeEmail,int boardID ) : base(new DTaskController())
        {
            Id = ID;
            _boardID = boardID;
            _title = title;
            _dueDate = dueDate;
            _desctiption = description;
            _creationTime = creationTime;
            _columnID = columnID;
            _asigneeEmail = assigneeEmail;
        }
        internal void InsertTask()
        {
            ((DTaskController)_controller).Insert(this);
        }

        internal void Delete()
        {
            ((DTaskController)_controller).Delete(this);
        }
    }
}
