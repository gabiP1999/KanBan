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
    /// Interaction logic for TaskEditWindow.xaml
    /// </summary>
    

    public partial class TaskEditWindow : Window
    {
        TaskViewModel viewModel;
        public TaskEditWindow(TaskViewModel v)
        {
            viewModel = v;
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void apply_title_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateTitle();
        }

        private void apply_description_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateDescription();
        }

        private void apply_due_date_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateDueDate();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            TasksView window = new(viewModel.User, viewModel.Board, viewModel.Column);
            this.Close();
            window.Show();
        }

        private void apply_assignee_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdateAssignee();
        }
    }
}
