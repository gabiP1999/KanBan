using System.Collections.Generic;
using System;
using System.Linq;
using IntroSE.Kanban.Backend.BusinessLayer;
using log4net;
using System.Reflection;
using System.IO;
using log4net.Config;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly UserController userController;
        public UserService(LoggedInUser U)
        {
            userController = new(U);
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
        }
        public Response Register(string email, string password)
        {
            try
            {
                userController.Register(email, password);
                log.Debug("User successfully created!");
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }
        public Response<Objects.User> Login(string email, string password)
        {
            try
            {
                BusinessLayer.User user = userController.Login(email, password);
                log.Debug("Logged in to user successfully!");
                return Response<Objects.User>.FromValue(new Objects.User(email));

            }
            catch(Exception e)
            {
                return Response<Objects.User>.FromError(e.Message);
            }
        }
        public Response Logout(string email)
        {
            try
            {
                userController.Logout(email);
                log.Debug("Logged out to user successfully!");
                return new Response();
            }
            catch (Exception e)
            {
                return new Response(e.Message);
            }
        }

        internal Response DeleteData()
        {
            try
            {
                userController.DeleteData();
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }

        public Response LoadData()
        {
            try
            {
                userController.LoadData();
                return new Response();
            }
            catch(Exception e)
            {
                return new Response(e.Message);
            }
        }
    }
}