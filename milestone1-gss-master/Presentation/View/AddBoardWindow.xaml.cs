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
    /// Interaction logic for AddBoardWindow.xaml
    /// </summary>
    public partial class AddBoardWindow : Window
    {
        AddBoardViewModel viewModel;
        public AddBoardWindow(UserModel model)
        {
            InitializeComponent();
            viewModel = new AddBoardViewModel(model);
            this.DataContext = viewModel;
        }
        public void AddBoard_click(Object sender,RoutedEventArgs e)
        {
            viewModel.AddBoard();
        }
        public void TitleUpdated(Object sender, DependencyPropertyChangedEventArgs e)
        {
            
            viewModel.TitleChanged();
        }
        public void Return_click(Object sender, RoutedEventArgs e)
        {
           
                BoardsView boardView = new BoardsView(viewModel.User);
                boardView.Show();
                this.Close();
            
        }
    }
}
