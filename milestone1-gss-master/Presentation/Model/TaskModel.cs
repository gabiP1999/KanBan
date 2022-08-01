using IntroSE.Kanban.Backend.ServiceLayer.Objects;
using System;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private string _color;
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                RaisePropertyChanged("Color");
            }
        }
        private string is_visible;
        public string IsVisible
        {
            get => is_visible; set
            {
                is_visible = value;
                RaisePropertyChanged("IsVisible");
            }
        }

        private bool is75percent()
        {
            TimeSpan diff1 = DueDate.Subtract(CreationTime);
            TimeSpan diff2 = DateTime.Now.Subtract(CreationTime);
            return (diff1.TotalHours * 0.75 < diff2.TotalHours);
        }

        private bool isOverDue()
        {
            int res = DateTime.Compare(DueDate, DateTime.Now);
            return res < 0;
        }

        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set
            {
                this._creationTime = value;
                RaisePropertyChanged("CreationTime");
            }
        }
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                this._title = value;
                RaisePropertyChanged("Title");
            }
        }
        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                this._description = value;
                RaisePropertyChanged("Description");
            }
        }
        private DateTime _dueDate;
        public DateTime DueDate
        {
            get => _dueDate;
            set
            {
                this._dueDate = value;
                RaisePropertyChanged("DueDate");
            }
        }
        private string _emailAssignee;
        public string EmailAssignee
        {
            get => _emailAssignee;
            set
            {
                this._emailAssignee = value;
                RaisePropertyChanged("EmailAssignee");
            }
        }
        private string UserEmail;
        public TaskModel(BackendController controller, int id, DateTime creationTime, string title, string description, DateTime dueDate, string emailAssignee, string user_email) : base(controller)
        {
            Id = id;
            CreationTime = creationTime;
            Title = title;
            Description = description;
            DueDate = dueDate;
            EmailAssignee = emailAssignee;
            UserEmail = user_email;
            IsVisible = "Visible";
            Color = "Transparent";
            SelectColor();
        }

        private void SelectColor()
        {
            if (isOverDue())
            {
                Color = "Red";
                return;
            }
            if (is75percent())
            {
                Color = "Orange";
                return;
            }
        }
    }
}
