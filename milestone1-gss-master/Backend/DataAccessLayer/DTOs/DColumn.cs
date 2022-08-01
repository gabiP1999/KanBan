using System;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
    public class DColumn : DTO
    {
        public const string ColumnIdColumnName = "ColumnID";
        public const string NameColumnName = "Name";
        public const string LimitColumnName = "ColumnLimit";
       

        private int _columnId;
        public int ColumnId { get => _columnId; set { _controller.Update(Id, ColumnId, ColumnIdColumnName, value); _columnId = value; } }
        private string _name;
        public string Name { get => _name; set { _name = value; _controller.Update(Id, ColumnId, NameColumnName, value); } }
        private int _limit;
        public int Limit { get => _limit; set { _limit = value; _controller.Update(Id, ColumnId, LimitColumnName, value); } }

        public DColumn(long ID, int columnID, string name, int limit) : base(new DColumnController())
        {
            Id = ID;
            _columnId = columnID;
            _name = name;
            _limit = limit;
        }
        public void insertColumn()
        {
            ((DColumnController)_controller).Insert(this);
        }

        internal void Delete()
        {
            ((DColumnController)_controller).Remove(this);
        }
    }
}
