using Frontend.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.ViewModel
{
    class InProgressTasksViewModel : NotifiableObject
    {
        public UserModel User { get; set ; }
        public List<TaskModel> Tasks { get; set; }
        private string searchbox_text;
        public string SearchBox_Text { get => searchbox_text; set { searchbox_text = value; RaisePropertyChanged("SearchBox_Text"); } }
        public InProgressTasksViewModel(UserModel user)
        {
            User = user;
            Tasks = user.GetInProgressTasks();
            SearchBox_Text = "";
            IninializeColors();
        }

        private void IninializeColors()
        {
            foreach(TaskModel t in Tasks)
            {
                if (t.Color.Equals("Transparent")) t.Color = "Blue";
            }
        }
        public void Search()
        {
            foreach(TaskModel t in Tasks)
            {
                if (!t.Title.Contains(SearchBox_Text) & !t.Description.Contains(SearchBox_Text))
                {
                    t.IsVisible = "Collapsed";
                }
            }
        }
        public void ResetSearch()
        {
            foreach (TaskModel t in Tasks)
            {
                t.IsVisible = "Visible";
            }
        }
    }
}
