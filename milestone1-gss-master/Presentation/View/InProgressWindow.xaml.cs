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
    /// Interaction logic for InProgressWindow.xaml
    /// </summary>
    public partial class InProgressWindow : Window
    {
        InProgressTasksViewModel viewModel;
        public InProgressWindow(UserModel user)
        {
            viewModel = new(user);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Search();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ResetSearch();
        }

        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            BoardsView window = new(viewModel.User);
            this.Close();
            window.Show();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)sender;
            if (row == null) return;
            TaskDetailsWindow window = new TaskDetailsWindow((TaskModel)(row.Item));
            window.Show();
        }
    }
}
