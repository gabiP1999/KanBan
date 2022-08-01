using System;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
    public class DUser : DTO
    {
        public const string UserEmailColumnName = "Email";
        public const string UserPasswordColumnName = "Password";


        private string _email;
        public string Email { get => _email; set { _email = value; _controller.Update(Id, UserEmailColumnName, value); } }
        private string _password;
        public string Password { get => _password; set { _password = value; _controller.Update(Id, UserPasswordColumnName, value); } }

        public DUser(long ID, string Email, string Password) : base(new DUserController())
        {
            Id = ID;
            _email = Email;
            _password = Password;
        }

        internal void InsertUser()
        {
            ((DUserController)_controller).Insert(this);
        }
    }
}
