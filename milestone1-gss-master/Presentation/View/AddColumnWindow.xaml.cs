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
    /// Interaction logic for AddColumnWindow.xaml
    /// </summary>
    public partial class AddColumnWindow : Window
    {
        ColumnViewModel viewModel;
        public AddColumnWindow(ColumnViewModel v)
        {
            this.viewModel = v;
            this.DataContext = this.viewModel;
            InitializeComponent();
        }

        private void Add_Column_Click(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumn();
        }
        private void Return_Click(object sender, RoutedEventArgs e)
        {
            ColumnsView window = new ColumnsView(viewModel.User, viewModel.Board);
            this.Close();
            window.Show();
        }
    }
}
