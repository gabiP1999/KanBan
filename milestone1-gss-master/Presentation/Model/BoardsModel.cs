using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace Frontend.Model
{
    public class BoardsModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }
        public ObservableCollection<BoardModel> Boards2 { get; set; }

        public BoardsModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>(controller.GetBoards(user));    
            Boards.CollectionChanged += HandleChange;
            Boards2 = new ObservableCollection<BoardModel>(controller.GetBoardsOther(user));
        }

        public void JoinBoard(BoardModel b)
        {

            Boards.Add(b);
            Boards2.Remove(b);
            
            
        }
        public void RemoveBoard(BoardModel b)
        {
            Boards.Remove(b);
        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {

                if (e.Action==NotifyCollectionChangedAction.Remove)
                {
                    if (e.OldItems != null)
                        foreach (BoardModel y in e.OldItems)
                        {
                            string res=Controller.RemoveBoard(user.Email, y.CreatorEmail, y.BoardName);
                            if (res != null) throw new Exception(res);
                        }
                }
                if (e.Action==NotifyCollectionChangedAction.Add)
                {
                if (e.NewItems != null)
                {
                    foreach (BoardModel y in e.NewItems)
                    {
                        
                         string res =Controller.JoinBoard(user.Email, y.CreatorEmail, y.BoardName);
                        if (res != null) throw new Exception(res);
                    }
                }
                }
            
        }
    }
}
