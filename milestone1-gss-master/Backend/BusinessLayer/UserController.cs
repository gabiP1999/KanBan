using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using IntroSE.Kanban.DataAccessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private Dictionary<string, User> users;
        private LoggedInUser loggedUsers;
      
        private int MAX_LENGTH_PASSWORD = 20;
        private int MIN_LENGTH_PASSWORD = 4;
        private int userId = 0;
        private string[] illegalpasswords;


        public UserController(LoggedInUser u) // Constructor
        {
            loggedUsers = u;
            users = new Dictionary<string, User>();
            illegalpasswords = loadPasswords();
        }

        private string[] loadPasswords()
        {
            string passwords = File.ReadAllText(@"password.txt");
            string[] illegalpasswords = passwords.Split(' ');

            return illegalpasswords;
            
        }

        

        /// <summary>
        /// loading the data from the data base
        /// </summary>
        /// <returns>void </returns>
        public void LoadData()
        {
            DUserController DUserCon = new DUserController();
            List<DataAccessLayer.DTOs.DUser> usersList = DUserCon.SelectAllUsers();
            foreach (DataAccessLayer.DTOs.DUser u in usersList)
            {
                User U1 = new User(u);
                this.users.Add(u.Email, U1);
                userId = (int)u.Id + 1;
            }
        }

        /// <summary>
        /// deleting the data
        /// </summary>
        /// <returns>void </returns>
        public void DeleteData()
        {
            DUserController DUserCon = new DUserController();
            DUserCon.DeleteAllUsers();
            users = new Dictionary<string, User>();
        }

        public User GetUser(string email)
        {
            if(email==null)
                throw new ArgumentNullException("email cann't be null");
            if (users.ContainsKey(email) == true)
                return users[email];
            else
                throw new ArgumentException("there is no user with this email");
        }

        /// <summary>
        /// Returns a user and set it to logged in state
        /// </summary>
        /// <param name="email">user email</param>
        /// <param name="password">user password</param>
        /// <returns></returns>
        public User Login(string email, string password)
        {
            if (email == null)
                throw new ArgumentNullException("email cann't be null");
            if (password == null)
                throw new ArgumentNullException("password cann't be null");
            if (this.IsUserLogged(email) == false)
            {
                if (users.ContainsKey(email))
                {
                    if (this.GetUser(email).ValidatePasswordMatch(password))
                    {
                        this.loggedUsers.Add(email);
                        return this.GetUser(email);
                    }
                    throw new ArgumentException("the password is not match");
                }
                else
                    throw new ArgumentException("the email is not exists");
            }
            else
                throw new ArgumentException("the user is already logged in");

        }

        /// <summary>
        /// Create a new user and add it to the user list
        /// </summary>
        /// <param name="email">new email</param>
        /// <param name="password">new password</param>
        /// <returns></returns>
        public User Register(string email, string password)
        {
            if (email == null)
                throw new ArgumentNullException("email cann't be null");
            if (password == null)
                throw new ArgumentNullException("password cann't be null");
            if(!ValidateUniqueEmail(email)) // Checking if the email is unique 
                throw new ArgumentException("email is already used!");
            if(!IsValidEmail(email)) // Checking if the email is legal 
                throw new ArgumentException("email is not legal!");
            if(!isValidPassword(password))
                throw new ArgumentException("illegal password!");
            if (ValidatePasswordRules(password)) // Checking if the passwors is legal
            {
                User newUser = new User(this.userId,email, password); // Create a new user
                users.Add(email, newUser); // Adding the user to the Dictionary
                userId += 1;
                return newUser;
            }
            else
                throw new ArgumentException("the password is invalid");
        }

        private bool isValidPassword(string password)
        {
            for (int i = 0; i < illegalpasswords.Length; i++)
                if (password.Equals(illegalpasswords[i]))
                    return false;
            return true;
        }

        /// <summary>
        /// Return true if email is valid
        /// </summary>
        /// <param name="email">smail</param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
        {
            string expression = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, "[א-ת]"))
                return false;
            if (Regex.IsMatch(email, expression))
            {
                if (Regex.Replace(email, expression, string.Empty).Length == 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Return true if email is unique
        /// </summary>
        /// <param name="email">email</param>
        /// <returns></returns>
        private bool ValidateUniqueEmail(string email)
        {
            if (email == null)
                throw new ArgumentNullException("email cann't be null");
            return !(users.ContainsKey(email));
        }
        /// <summary>
        /// Return true if password is valid
        /// </summary>
        /// <param name="password">password</param>
        /// <returns></returns>
        private bool ValidatePasswordRules(string password)
        {
            if (!(password.Length >= MIN_LENGTH_PASSWORD & password.Length <= MAX_LENGTH_PASSWORD))
                return false;
            if (!password.Any(char.IsLower) | !password.Any(char.IsDigit) | !password.Any(char.IsUpper))
                return false;
               return true;
        }
        
        
        /// <summary>
        /// Set user's state to logged out
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns></returns>
        public void Logout(string email) 
        {
            if (email == null)
                throw new ArgumentNullException("email cann't be null");
            if (this.IsUserLogged(email) == false) //Checking if the user avialable
                throw new ArgumentException("the user is already logged out");
            else
            {
                this.loggedUsers.Remove(email);
            }
            
        }
 
        /// <summary>
        /// Return true if the user is logged in
        /// </summary>
        /// <param name="email">user email</param>
        /// <returns></returns>
        public bool IsUserLogged(string email)
        {
            return this.loggedUsers.CheckLoggedIn(email);
        }

    }
}