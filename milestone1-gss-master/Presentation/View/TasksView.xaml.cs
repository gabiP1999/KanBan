using Frontend.Model;
using Frontend.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Frontend.View
{
    /// <summary>
    /// Interaction logic for TasksView.xaml
    /// </summary>
    public partial class TasksView : Window
    {
        TaskViewModel viewModel;
        public TasksView(UserModel user, BoardModel board, ColumnModel column)
        {
            viewModel = new(user, board, column);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            TaskEditWindow window = new(viewModel);
            this.Close();
            window.Show();

        }

      

        private void Advance_Click(object sender, RoutedEventArgs e)
        {
            
            viewModel.AdvanceTask();
        }

        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            ColumnsView window = new(viewModel.User, viewModel.Board);
            this.Close();
            window.Show();

        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            if (row == null) return;
            TaskDetailsWindow window = new(viewModel.SelectedTask);
            window.Show();
        }

        private void DataGridRow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            if (row == null) return;
            viewModel.SelectedTask = (TaskModel)row.Item;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Search();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ResetSearch();
        }
    }
}
