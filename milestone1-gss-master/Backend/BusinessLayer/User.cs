using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer;
using IntroSE.Kanban.DataAccessLayer.DTOs;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        
        private int id;
        public int ID { get => id; set { id = value; } }
        private string email;
        public string Email { get => email; set { email = value; } }
        private string password;
        
        private Dictionary<string, Task> TaskList; // Tasks Dictionary by id
        private Dictionary<string, Board> BoardList; // Boards Dictionary by id
        private DataAccessLayer.DTOs.DUser DUser;
        private DUser u;

        public User(int id,string email, string password)  // Constructor
        {
            if (email == null)
                throw new ArgumentNullException("email cann't be null");
            if (password == null)
                throw new ArgumentNullException("password cann't be null");
            this.id = id;
            this.email = email;
            this.password = password;
            DUser = new DataAccessLayer.DTOs.DUser(id, email, password);
            DUser.InsertUser();
        }

        public User(DUser u) // Constructor with data user 
        {
            this.id = (int)(u.Id);
            this.email = u.Email;
            this.password = u.Password;
            this.DUser = u;
        }

        // Checking if the passward is match
        public bool ValidatePasswordMatch(string password)
        {
            return password.Equals(this.password);
        }


       
       
    }
}