using System;
using IntroSE.Kanban.Backend.BusinessLayer;
using NUnit.Framework;

namespace IntroSE.Kanban.NUnitTestProject1
{
    public class Tests
    {
        Column column;

        [SetUp]
        public void Setup()
        {
            column = new Column(0, "backlog", 5);
        }

        [Test]
        public void AddTask_TaskLimit_Success()
        {
            Column column = new Column(0, "backlog",5);
            Task t = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
            column.AddTask(t);
            Assert.AreEqual(1, column.Tasks.Count, "adding task to the column seccess");
        }

        [Test]
        public void AddTask_TaskLimit_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 1);
                Task t1 = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
                Task t2 = new Task(3, 4, DateTime.Parse("03-05-2019"), DateTime.Parse("03-05-2022"), "home work", "do home work in hebrew", "b");
                column.AddTask(t1);
                column.AddTask(t2);
                Assert.Fail( "adding task to the column fail beacuse is pass the limit of the column");
            }
            catch(Exception)
            {

            }
        }
        [Test]
        public void AddTask_TaskIsExsist_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 1);
                Task t1 = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
                column.AddTask(t1);
                column.AddTask(t1);
                Assert.Fail("adding task to the column is fail beause the task is already in this column");
            }
            catch (Exception)
            {

            }
        }
        [Test]
        public void AddTask_NullTask_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 5);
                Task t = null;
                column.AddTask(t);
                Assert.Fail("adding task to the column fail beacuse the task is null");
            }
            catch (Exception)
            {

            }
        }

        [Test]
        public void RemoveTask_TaskContain_Success()
        {
            Column column = new Column(0, "backlog", 5);
            Task t = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
            column.AddTask(t);
            column.RemoveTask(t);
            Assert.AreEqual(false, column.Tasks.ContainsKey(t.ID), "removing task to the column seccess");
        }
       
        [Test]
        public void RemoveTask_TaskContain_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 2);
                Task t = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
                column.RemoveTask(t);
                Assert.Fail("removing task to the column is fail beause the task is not in this column");
            }
            catch (Exception)
            {

            }
        }
        [Test]
        public void RemoveTask__NullTask_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 5);
                Task t = null;
                column.RemoveTask(t);
                Assert.Fail("removing task to the column fail beacuse the task is null");
            }
            catch (Exception)
            {

            }
        }
        [Test]
        public void SetmaxTasks_ValidMaxTasks_Success()
        {
            Column column = new Column(0, "backlog", 5);
            int maxTasks = 2;
            column.SetmaxTasks(maxTasks);
            Assert.AreEqual(column.Limit, 2 , "changing max tasks is seccess");
        }

        [Test]
        public void SetmaxTasks_ValidMaxTasks_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 5);
                int maxTasks = -2;
                column.SetmaxTasks(maxTasks);
                Assert.Fail("changing max tasks is fail because the max taks number is invalid");
            }
            catch(Exception)
            {

            }
        }


        [Test]
        public void SetmaxTasks_PassLimit_Fail()
        {
            try
            {
                Column column = new Column(0, "backlog", 5);
                Task t1 = new Task(2, 2, DateTime.Parse("05-05-2019"), DateTime.Parse("05-05-2022"), "home work", "do home work in math", "a");
                Task t2 = new Task(3, 4, DateTime.Parse("03-05-2019"), DateTime.Parse("03-05-2022"), "home work", "do home work in hebrew", "b");
                column.AddTask(t1);
                column.AddTask(t2);
                int maxTasks = 1;
                column.SetmaxTasks(maxTasks);
                Assert.Fail("changing max tasks is fail because you pass the limit taks");
            }
            catch (Exception)
            {

            }
        }

    }
}