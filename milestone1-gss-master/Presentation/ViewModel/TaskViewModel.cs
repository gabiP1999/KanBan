using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class TaskViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
        public UserModel User
        {
            get => user;
        }
        private BoardModel board;
        public BoardModel Board
        {
            get => board;
        }
        private ColumnModel column;
        public ColumnModel Column
        {
            get => column;
        }
        private string searchbox_text;
        public string SearchBox_Text
        {
            get => searchbox_text; set
            {
                searchbox_text = value;
                RaisePropertyChanged("SearchBox_Text");
            }
        }
        private string update_title;
        public string Update_Title
        {
            get => update_title; set
            {
                update_title = value;
                RaisePropertyChanged("Update_Title");
            }
        }
        private string update_assignee;
        public string Update_Assignee
        {
            get => update_assignee; set
            {
                update_assignee = value;
                RaisePropertyChanged("Update_Assignee");
            }
        }
        private string update_description;
        public string Update_Description
        {
            get => update_description; set
            {
                update_description = value;
                RaisePropertyChanged("Update_Description");
            }
        }
        private DateTime update_duedate;
        public DateTime Update_Duedate
        {
            get => update_duedate; set
            {
                update_duedate = value;
                RaisePropertyChanged("Update_Duedate");
            }
        }

        public TasksModel Tasks { get; private set; }
        private TaskModel _selectedTask;
        public TaskModel SelectedTask
        {
            get
            {
                return _selectedTask;
            }
            set
            {
                
                _selectedTask = value;
                Update_Title = _selectedTask.Title;
                Update_Duedate = _selectedTask.DueDate;
                Update_Description = _selectedTask.Description;
                Update_Assignee = _selectedTask.EmailAssignee;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedTask");
            }
        }
        internal void UpdateAssignee()
        {
            controller.UpdateTaskAssignee(User, Board, Column, SelectedTask, Update_Assignee);
        }

        internal void UpdateTitle()
        {
            controller.UpdateTaskTitle(User, Board, Column, SelectedTask, Update_Title);
        }

        internal void UpdateDescription()
        {
            controller.UpdateTaskDescription(User, Board, Column, SelectedTask, Update_Description);
        }

        internal void UpdateDueDate()
        {
            controller.UpdateTaskDueDate(User, Board, Column, SelectedTask, Update_Duedate);
        }

        private bool _enableForward = false;
        public bool EnableForward
        {
            get => _enableForward;
            private set
            {
                _enableForward = value;
                RaisePropertyChanged("EnableForward");
            }
        }

        internal void Logout()
        {
            
        }
        

        public TaskViewModel(UserModel user, BoardModel board, ColumnModel column)
        {
            this.controller = user.Controller;
            this.user = user;
            this.board = board;
            this.column = column;
            Tasks = new TasksModel(controller, user, board, column);
            SearchBox_Text = "";
        }
        public void Search()
        {
            foreach (TaskModel t in Tasks.Tasks)
            {
                if(!t.Title.Contains(SearchBox_Text) & !t.Description.Contains(SearchBox_Text))
                {
                    t.IsVisible = "Collapsed";
                }
            }
        }
        public void ResetSearch()
        {
            foreach (TaskModel t in Tasks.Tasks)
            {
                t.IsVisible = "Visible";
            }
        }

        public void AdvanceTask()
        {

            try
            {
                
               Tasks.RemoveTask(SelectedTask);
            }
            catch (Exception e )
            {
                MessageBox.Show(e.Message);

            }
            
        }
    }
}