using Frontend.Model;
using System;
using System.Windows;
using System.Windows.Media;

namespace Frontend.ViewModel

{
    public class BoardViewModel : NotifiableObject
    {
        private Model.BackendController controller;
        private UserModel user;
     
        public BoardsModel Boards { get; private set; }
        private BoardModel _selectedBoard;
        public BoardModel SelectedBoard
        {
            get
            {
                return _selectedBoard;
            }
            set
            {
                _selectedBoard = value;
                EnableForward = value != null;
                RaisePropertyChanged("SelectedBoard");
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

        public BoardViewModel(UserModel user)
        {
            this.controller = user.Controller;
            this.user = user;
            Boards = user.GetBoards();
        }

        public void RemoveBoard()
        {
            try
            {
                Boards.RemoveBoard(SelectedBoard);
            }
            catch (Exception e )
            {
                MessageBox.Show( e.Message);
            }
            
        }

        internal void DeleteData()
        {
            controller.DeleteData();
        }

        public void JoinBoard()
        {
            try
            {
                Boards.JoinBoard(SelectedBoard);
            }
            catch(Exception e)
            {
                MessageBox.Show("Cannot remove board. " + e.Message);
            }
        }
    }
    
}