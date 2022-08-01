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
    /// Interaction logic for AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        ColumnViewModel viewModel; 
        public AddTaskWindow(ColumnViewModel v)
        {
            viewModel = v;
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddTask();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            ColumnsView window = new(viewModel.User, viewModel.Board);
            this.Close();
            window.Show();
        }
    }
}
