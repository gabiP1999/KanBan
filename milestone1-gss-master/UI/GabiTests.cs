using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI
{
    class GabiTests
    {
        private Service us;
        
        public GabiTests(Service u)
        {
            us = u;
        }
        public void LoadDataTest()
        {
            Console.WriteLine("####  Load Data Test  ####");
            us.LoadData();
            Console.WriteLine("Login:");
            Console.WriteLine(us.Login("fakemail@doar.fake", "123aA").Value.Email+" logged in");
            Console.WriteLine("Getting Board Names:");
            IList<string> boards = us.GetBoardNames("fakemail@doar.fake").Value;
            if (boards != null)
            {
                foreach (string s in boards)
                {
                    Console.WriteLine(s);
                }
            }
            
        }
        public void DeleteData()
        {
            Console.WriteLine(us.DeleteData().ErrorMessage);
        }
        public void RunTests()
        {
            Console.WriteLine("####  Registration Tests  ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Console.WriteLine(us.Register("fakemail@doar.fake", "123aA").ErrorMessage);
            Console.WriteLine(us.Register("kuala@zoo.il", "moooO1234").ErrorMessage);
            Console.WriteLine(us.Register("panda1999@dov.org", "pandA5000").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("Should Fail:");
            Console.WriteLine(us.Register("monkey1@walla.com", "123monkey").ErrorMessage);
            Console.WriteLine(us.Register("koff3@-com.dot", "12344545").ErrorMessage);
            Console.WriteLine(us.Register("", "hhhhhhhh").ErrorMessage);
            Console.WriteLine(us.Register("fakemail@doar.fake", "123").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("####  Login Tests  ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Response<User> res1=us.Login("fakemail@doar.fake", "123aA");
            Console.WriteLine(res1.Value.Email);
            Response<User> res2 = us.Login("kuala@zoo.il", "moooO1234");
            Console.WriteLine(res2.Value.Email);
            Response<User> res3 = us.Login("panda1999@dov.org", "pandA5000");
            Console.WriteLine(res3.Value.Email);
            Console.WriteLine(us.Logout("panda1999@dov.org").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("Should Fail:");
            Response<User> res4 = us.Login("monkey1@walla.com", "1234monkey");
            Console.WriteLine(res4.ErrorMessage);
            Response<User> res5 = us.Login("panda1999@do.org", "panda5000");
            Console.WriteLine(res5.ErrorMessage);
            Console.WriteLine(us.Logout("panda1999@dov.org"));
            Console.WriteLine();
            Console.WriteLine("#### Board Tests  ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Console.WriteLine(us.AddBoard("fakemail@doar.fake","Fake Board").ErrorMessage);
            Console.WriteLine(us.AddBoard("kuala@zoo.il","Real Board").ErrorMessage);
            Console.WriteLine(us.AddBoard("fakemail@doar.fake","Another Board").ErrorMessage);
            Console.WriteLine(us.RemoveBoard("fakemail@doar.fake", "fakemail@doar.fake", "Another Board").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("Should Fail:");
            Console.WriteLine(us.AddBoard("fakemail@doar.fake", "Fake Board").ErrorMessage);
            Console.WriteLine(us.AddBoard("panda1999@dov.org", "Fake Board").ErrorMessage);
            Console.WriteLine(us.AddBoard("notrealmail@fake.mail","Board").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("####  Join Board Test  ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Console.WriteLine(us.JoinBoard("kuala@zoo.il", "fakemail@doar.fake", "Fake Board").ErrorMessage);
            Console.WriteLine(us.JoinBoard("fakemail@doar.fake", "kuala@zoo.il", "Real Board").ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("Should Fail:");
            Console.WriteLine(us.JoinBoard("panda1999@dov.org", "fakemail@doar.fake", "Fake Board").ErrorMessage);
            Console.WriteLine("####  Task Test  ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Console.WriteLine(us.AddTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", "Task1 title", "Task1 desc", DateTime.Parse("2022-02-10")).Value.ToString());
            Console.WriteLine(us.AddTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", "Task2 title", "Task2 desc", DateTime.Parse("2022-02-12")).Value.ToString());
            Console.WriteLine(us.AddTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", "Task3 title", "Task3 desc", DateTime.Parse("2022-02-11")).Value.ToString());
            Console.WriteLine();
            Console.WriteLine("Should Fail:");
            Console.WriteLine(us.AddTask("kuaala@zoo.il", "fakemail@doar.fake", "Fake Board", "Task1 title", "Task1 desc", DateTime.Parse("2022-02-10")).ErrorMessage);
            Console.WriteLine(us.AddTask("panda1999@dov.org", "fakemail@doar.fake", "Fake Board", "Task2 title", "Task2 desc", DateTime.Parse("2022-02-12")).ErrorMessage);
            Console.WriteLine(us.AddTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", "Task3 title", "Task3 desc", DateTime.Parse("2021-02-02")).ErrorMessage);
            Console.WriteLine(us.AddTask("kuala@zoo.il", "fakemail@doar.fake", "Faek Board", "Task1 title", "Task1 desc", DateTime.Parse("2022-02-10")).ErrorMessage);
            Console.WriteLine();
            Console.WriteLine("#### Advance Task ####");
            Console.WriteLine();
            Console.WriteLine("Should Succeed:");
            Console.WriteLine(us.AdvanceTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", 0, 0).ErrorMessage);
            Console.WriteLine(us.AdvanceTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", 1, 0).ErrorMessage);
            Console.WriteLine(us.AdvanceTask("kuala@zoo.il", "fakemail@doar.fake", "Fake Board", 0, 1).ErrorMessage);
            Console.WriteLine();






        }
    }
}
