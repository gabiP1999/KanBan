using System;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
    public class DBoard : DTO
    {
        public const string CreatorEmailColumnName = "CreatorEmail";
        public const string BoardNameColumnName = "Name";

        private string _creatoremail;
        public string CreatorEmail { get => _creatoremail; set { _creatoremail = value; _controller.Update(Id, CreatorEmailColumnName, value); } }
        private string _creatorName;
        public string CreatorName { get => _creatorName; set { _creatorName = value; _controller.Update(Id, BoardNameColumnName, value); } }

        public DBoard(long ID, string creatormail, string creatorname) : base(new DBoardController())
        {
            Id = ID;
            _creatoremail = creatormail;
            _creatorName = creatorname;
         
        }
        public void insertBoard() {
            ((DBoardController)_controller).Insert(this);
        }

        internal void Delete()
        {
            ((DBoardController)_controller).Delete(this);
        }
    }
}
