using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Frontend.Model
{
    public class TasksModel : NotifiableModelObject
    {
        private readonly UserModel user;
        private readonly BoardModel board;
        private readonly ColumnModel column;
        public ObservableCollection<TaskModel> Tasks { get; set; }      

        public TasksModel(BackendController controller, UserModel user, BoardModel board, ColumnModel column) : base(controller)
        {
            this.user = user;
            this.board = board;
            this.column = column;
            Tasks = new ObservableCollection<TaskModel>(controller.GetTasks(user, board, column));
            Tasks.CollectionChanged += HandleChange;
            ColorAssignedTasks(Tasks);
        }
        private void ColorAssignedTasks(ObservableCollection<TaskModel> tasks)
        {
            foreach(TaskModel t in tasks)
            {
                if (t.EmailAssignee.Equals(user.Email))
                {
                    if(t.Color.Equals("Transparent")) t.Color = "Blue";
                }
            }
        }


        public void RemoveTask(TaskModel t)
        {
            Tasks.Remove(t);
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TaskModel y in e.OldItems)
                {
                    string res = Controller.AdvanceTask(user, board, column, y);
                    if (res!=null) throw new Exception(res);
                }
            }
        }

        internal void AddTask(TaskModel selectedTask)
        {
            Tasks.Add(selectedTask);
        }
    }
}
