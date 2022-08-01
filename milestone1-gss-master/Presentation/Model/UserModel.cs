using System.Collections.Generic;

namespace Frontend.Model
{
    public class UserModel : NotifiableModelObject
    {
        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                RaisePropertyChanged("Email");
            }
        }
        public List<TaskModel> GetInProgressTasks()
        {
            return Controller.GetInProgressTasks(this);
        }
        public BoardsModel GetBoards()
        {
            return new BoardsModel(Controller, this);
        }
        public UserModel(BackendController controller, string email) : base(controller)
        {
            this.Email = email;
        }
    }
}
