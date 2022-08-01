using IntroSE.Kanban.DataAccessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Column
    {
   
        
        internal DColumn dColumn;
        private string name;
        public string Name { get => name; set { name = value; dColumn.Name = value; } }
        private int limit = -1;
        public int Limit { get => limit; set { limit = value; dColumn.Limit = value; } }
        public Dictionary<int, Task> Tasks;//Tasks dictionary by id
       
        private int id;
        public int ID { get => id; set { id = value; dColumn.ColumnId = value; } }

        public Column(int id, string name, DColumn dcolumn) // Defualt Constructor (without max tasks)
        {
            this.id = id;
            this.name = name;
            Tasks = new Dictionary<int, Task>();
            this.dColumn = dcolumn;
            dColumn.insertColumn();
        }
        public Column(int id, string name, int MAX_TASKS)// Constructor (with max tasks)
        {
            
            this.id = id;
            this.name = name;
            this.limit = MAX_TASKS;
            Tasks = new Dictionary<int, Task>();
        }
        public Column(DColumn dcolumn)// Constructor 
        {
            this.dColumn = dcolumn;
            this.id = dcolumn.ColumnId;
            this.name = dcolumn.Name;
            this.limit = dcolumn.Limit;
            Tasks = new Dictionary<int, Task>();
        }


        /// <summary>
        /// loading the data from the data base
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <param name="dTaskController">Data Task Controller</param>
        /// <returns>int </returns>
        public int LoadData(int id, DataAccessLayer.DTaskController dTaskController)
        {
            int output = 0;
            List<DTask> tasks = dTaskController.GetTasks(id, this.id);
            foreach(DTask t in tasks)
            {
                Tasks.Add((int)t.Id, new Task(t));
                output++;
                id = (int)t.Id + 1;
            }
            return output;
        }

        /// <summary>
        /// Returns true if task belong to this column
        /// </summary>
        /// <param name="task">Task</param>
        /// <returns></returns>
        public bool IsBelong(Task task)
        {
            return Tasks.ContainsKey(task.ID);
        }

        /// <summary>
        /// Add a task to the column 
        /// </summary>
        /// <param name="task">Task</param>
        public void AddTask(Task task)
        {
            if (task == null)
                throw new Exception("the task is null");
            if(Tasks.ContainsKey(task.ID))
                throw new Exception("the task is already in this column");
            if  (Tasks.Count < limit | limit == -1)
                Tasks[task.ID] = task; 
            else
                throw new Exception("you've passed the task limit");
        }

        /// <summary>
        /// Remove a task from the column if it contains it
        /// </summary>
        /// <param name="task">Task</param>
        /// <returns></returns>
        public Task RemoveTask(Task task)
        {
            if (task == null)
                throw new Exception("the task is null");
            if (Tasks.ContainsKey(task.ID))
            {
                this.Tasks.Remove(task.ID);
            }
            else
                throw new Exception("ID not found");
            return task;

        }

        /// <summary>
        /// Set the maximum amount of tasks in a column
        /// </summary>
        /// <param name="maxTasks">maximum tasks</param>
        public void SetmaxTasks(int maxTasks)
        {
            if (!ValidateMaxTasks(maxTasks))
                throw new Exception("invalid max task number");
            if (Tasks.Count < maxTasks | maxTasks == -1)
                this.limit = maxTasks;
            else
                throw new Exception("you've passed the task limit");
        }

        /// <summary>
        /// Returns  true if maxTasks was set to a certain number
        /// </summary>
        /// <param name="maxTasks"></param>
        /// <returns></returns>
        private bool ValidateMaxTasks(int maxTasks)
        {
            return maxTasks > 0;
        }

        /// <summary>
        /// Returns all Tasks in the column in a List
        /// </summary>
        /// <returns></returns>
        public IList<Task> GetTasksList()
        {
            if (Tasks.Count == 0) return new List<Task>();
            return Tasks.Values.ToList();

        }

        /// <summary>
        /// Returns a task provided its ID
        /// </summary>
        /// <param name="id">Task ID</param>
        /// <returns></returns>
        public Task GetTask(int id)
        {
            if (!this.Tasks.ContainsKey(id)) throw new Exception("Task not found");
            return Tasks[id];
        }

        /// <summary>
        /// deleting the data of the task
        /// </summary>
        /// <returns>void </returns>
        internal void Delete()
        {
            foreach (Task t in Tasks.Values)
            {
                t.Delete();
            }
        }

        //Gettrs:
    
        public Dictionary<int,Task> GetTasks() { return this.Tasks; }


    }

}