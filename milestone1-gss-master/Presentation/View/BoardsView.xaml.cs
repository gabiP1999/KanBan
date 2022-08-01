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
    /// Interaction logic for BoardsView.xaml
    /// </summary>
    public partial class BoardsView : Window
    {
        private BoardViewModel viewModel;
        private UserModel u;

        public BoardsView(UserModel u)
        {
            InitializeComponent();
            this.u = u;
            this.viewModel = new(u);
            this.DataContext = viewModel;
        }
        private void RemoveBoard_Button(object sender, RoutedEventArgs e)
        {
            viewModel.RemoveBoard();
        }
        private void LogOut_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            MainWindow login = new();
            login.Show();
            this.Close();
        }
        
        private void JoinBoard_Button(object sender, RoutedEventArgs e)
        {
             viewModel.JoinBoard();
        }
        private void DeleteData_Button(object sender, RoutedEventArgs e)
        {
            viewModel.Logout();
            viewModel.DeleteData();
            MainWindow login = new();
            login.Show();
            this.Close();

        }
        private void OtherBoardsRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)(sender);
            if (row == null) return;
            viewModel.SelectedBoard = (BoardModel)(row.Item);
        }
        public void OpenAddBoard(object sender, RoutedEventArgs e)
        {
            AddBoardWindow window = new AddBoardWindow(u);
            this.Close();
            window.Show();
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = (DataGridRow)(sender);
            if (row == null) return;
            ColumnsView window = new(u,(BoardModel)(row.Item));
            this.Close();
            window.Show();
        }

        private void InProgress_Click(object sender, RoutedEventArgs e)
        {
            InProgressWindow window = new(u);
            this.Close();
            window.Show();
        }
    }
}
