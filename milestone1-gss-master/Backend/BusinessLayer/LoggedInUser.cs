using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class LoggedInUser
    {
        private List<String> loggedInUsers = new List<string>();

        public LoggedInUser()
        {

        }

        /// <summary>
        /// adding the user from the dectionary list
        /// </summary>
        /// <param name="email">smail</param>
        /// <returns></returns>
        public void Add(String email)
        {
            loggedInUsers.Add(email);
        }

        /// <summary>
        /// removing the user from the dectionary list
        /// </summary>
        /// <param name="email">smail</param>
        /// <returns>void </returns>
        public void Remove(String email)
        {
            if (loggedInUsers.Contains(email))
                loggedInUsers.Remove(email);
            else
                throw new Exception("The user " + email + " is not logged in");
        }

        /// <summary>
        /// checking if the user is logged in
        /// </summary>
        /// <param name="email">smail</param>
        /// <returns>true if the user logged in</returns>
        public bool CheckLoggedIn(string email)
        {
            return loggedInUsers.Contains(email);

        }


    }
}
