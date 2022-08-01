using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class ColumnsModel : NotifiableModelObject
    {
        private readonly UserModel user;
        private readonly BoardModel board;
        public ObservableCollection<ColumnModel> Columns { get; set; }      

        public ColumnsModel(BackendController controller, UserModel user, BoardModel board) : base(controller)
        {
            this.user = user;
            this.board = board;
            Columns = new ObservableCollection<ColumnModel>(controller.GetColumns(user, board));
            Columns.CollectionChanged += HandleChange;
        }
        public void SetColumnName(string name,ColumnModel col)
        {
            Controller.SetColumnName(user, board, col,name);
        }
        public void SetColumnLimit(int limit, ColumnModel col)
        {
            Controller.SetColumnLimit(user, board, limit, col);
        }


        public void RemoveColumn(ColumnModel c)
        {

            Columns.Remove(c);

        }

        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ColumnModel y in e.OldItems)
                {

                    Controller.RemoveColumn(user.Email, board.CreatorEmail, board.BoardName, y.Id);
                }

            }
        }
    }
}
