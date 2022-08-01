using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Frontend.Model
{
    public class ColumnModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string _columnName;
        public string ColumnName
        {
            get => _columnName;
            set
            {
                this._columnName = value;
                RaisePropertyChanged("ColumnName");
            }
        }
        private int _limit;
        public int Limit
        {
            get => _limit;
            set
            {
                this._limit = value;

                RaisePropertyChanged("Limit");
            }
        }
        private string UserEmail;
        public ColumnModel(BackendController controller, int id, string columnName, int limit, string user_email) : base(controller)
        {
            Id = id;
            ColumnName = columnName;
            Limit = limit;
            UserEmail = user_email;
        }

    }
}
