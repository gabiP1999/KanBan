using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class ColumnViewModel : NotifiableObject
    {
        private string add_task_title;
        public string Add_Task_Title { get => add_task_title; set
            {
                add_task_title = value;
                RaisePropertyChanged("Add_Task_Title");
            } }
        private string add_task_description;
        public string Add_Task_Description
        {
            get => add_task_description; set
            {
                add_task_description = value;
                RaisePropertyChanged("Add_Task_Description");
            }
        }
        private DateTime add_task_due_date;
        public DateTime Add_Task_Due_Date
        {
            get => add_task_due_date; set
            {
                add_task_due_date = value;
                RaisePropertyChanged("Add_Task_Due_Date");
            }
        }

        internal void AddTask()
        {
            controller.AddTask(User, Board, Add_Task_Title, Add_Task_Description, Add_Task_Due_Date);
        }

        private int _shift_size;
        public int Column_Shift_ID
        {
            get => _shift_size; set
            {
                _shift_size = value;
                RaisePropertyChanged("Column_Shift_ID");
            }
        }
        private Model.BackendController controller;
        private UserModel user;
        public UserModel User { get => user; }
        private BoardModel board;
        public BoardModel Board { get => board; }
        private int _column_limit;
        private int _add_column_id;
        public int AddColumnID
        {
            get => _add_column_id; set
            {
                _add_column_id = value;
                RaisePropertyChanged("AddColumnID");
            }
        }
        private string _add_column_name;
        public string AddColumnName
        {
            get=> _add_column_name; set{
                _add_column_name = value;
                RaisePropertyChanged("AddColumnName");
            }
        }
        public int Column_Limit
        {
            get => _column_limit; set
            {
                _column_limit = value;
                RaisePropertyChanged("Column_Limit");
            }
        }
        private string _column_name;
        public string Column_Name
        {
            get => _column_name; set
            {
                _column_name = value;
                RaisePropertyChanged("Column_Name");
            }
        }

        internal void AddColumn()
        {
            controller.AddColumn(User, Board, AddColumnID, AddColumnName);
            AddColumnName = "";
            AddColumnID = 0;
        }

        public ColumnsModel Columns { get; private set; }
        private ColumnModel _selectedColumn;
        public ColumnModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedColumn");
            }
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

        public ColumnViewModel(UserModel user, BoardModel board)
        {
            this.controller = user.Controller;
            this.user = user;
            this.board = board;
            Columns = new ColumnsModel(controller, user, board);
            Column_Name = "Enter Name";
            Column_Limit = 0;
            AddColumnName = "Enter Name";
            AddColumnID = 0;
            Column_Shift_ID = 0;
            Add_Task_Title = "Enter Title";
            Add_Task_Description = "Enter Description";
            Add_Task_Due_Date = DateTime.Now;
        }
        public void LimitColumn()
        {
            try
            {
                Columns.SetColumnLimit(Column_Limit, SelectedColumn);
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot set column limit to" + Column_Limit + " " + e.Message);
            }
            Column_Limit = 0;
        }
        public void RenameColumn()
        {
            try
            {
                Columns.SetColumnName(Column_Name, SelectedColumn);
            }
            catch(Exception e)
            {
                MessageBox.Show("Cannot set column name to" + Column_Name + " "+ e.Message);
            }
            Column_Name = "";
        }
        public void RemoveColumn()
        {

            try
            {
                Columns.RemoveColumn(SelectedColumn);
            }
            catch (Exception e )
            {
                MessageBox.Show("Cannot remove column. " + e.Message);
            }
            
        }
        public void ShiftColumn()
        {
            controller.MoveColumn(User, Board, SelectedColumn.Id, Column_Shift_ID - SelectedColumn.Id);
        }
    }
}