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
    /// Interaction logic for ColumnsView.xaml
    /// </summary>
    public partial class ColumnsView : Window
    {
        ColumnViewModel viewModel;
        public ColumnsView(UserModel user, BoardModel board)
        {
            viewModel = new ColumnViewModel(user, board);
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void return_button_Click(object sender, RoutedEventArgs e)
        {
            BoardsView window = new BoardsView(viewModel.User);
            this.Close();
            window.Show();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            ColumnEditWindow window = new ColumnEditWindow(viewModel);
            this.Close();
            window.Show();

        }

        private void create_Click(object sender, RoutedEventArgs e)
        {
            AddColumnWindow window = new(viewModel);
            this.Close();
            window.Show();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveColumn();
        }

       

        private void DataGridRow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)(sender);
            if (row == null) return;
            viewModel.SelectedColumn = (ColumnModel)(row.Item);
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)(sender);
            if (row == null) return;
            TasksView window = new(viewModel.User, viewModel.Board, (ColumnModel)(row.Item));
            this.Close();
            window.Show();
        }

        private void addTask_Click(object sender, RoutedEventArgs e)
        {
            
            AddTaskWindow window = new(viewModel);
            this.Close();
            window.Show();

        }
    }
}
