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
    /// Interaction logic for ColumnEditWindow.xaml
    /// </summary>
    public partial class ColumnEditWindow : Window
    {
        ColumnViewModel viewModel;
        public ColumnEditWindow(ColumnViewModel v)
        {
            this.viewModel = v; ;
            this.DataContext = this.viewModel;
            InitializeComponent();
        }

        private void Name_Apply_Click(object sender, RoutedEventArgs e)
        {
            viewModel.RenameColumn();
        }

        private void Limit_Apply_Click(object sender, RoutedEventArgs e)
        {
            viewModel.LimitColumn();
        }
        private void Shift_Apply_Click(object sender, RoutedEventArgs e)
        {
            viewModel.ShiftColumn();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            ColumnsView window = new ColumnsView(viewModel.User, viewModel.Board);
            this.Close();
            window.Show();
        }
    }
}
