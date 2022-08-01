using IntroSE.Kanban.Backend.ServiceLayer.Objects;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
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
        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            set
            {
                this._boardName = value;
                RaisePropertyChanged("BoardName");
            }
        }
        private string _creatorEmail;
        public string CreatorEmail
        {
            get => _creatorEmail;
            set
            {
                this._creatorEmail = value;
                RaisePropertyChanged("CreatorEmail");
            }
        }
        private string UserEmail;
        public BoardModel(BackendController controller, int id, string boardName, string creatorEmail,string user_email) : base(controller)
        {
            Id = id;
            BoardName = boardName;
            CreatorEmail = creatorEmail;
            UserEmail = user_email;
        }

    }
}
