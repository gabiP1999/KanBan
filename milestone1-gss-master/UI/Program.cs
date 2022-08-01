using System;
using System.IO;
using IntroSE.Kanban.Backend.ServiceLayer;
using UI;

namespace IntroSE.Kanban.Frontend
{
    class Program
    {

        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome!");
            Service userService = new Service();
            GabiTests gabi = new GabiTests(userService);
            //gabi.LoadDataTest();
            //gabi.RunTests();
            //gabi.DeleteData();
            

        }
    }
}
