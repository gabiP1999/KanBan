using Frontend.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Frontend.ViewModel
{
    public class AddBoardViewModel : NotifiableObject
    {
        private readonly UserModel user;
        public UserModel User { get { return user; }  }
        private string _title;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Title { get=>_title; set
            { _title = value;
                OnPropertyChanged("Title");
            } }



        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        
        public AddBoardViewModel(UserModel m)
        {
            _title = "";
            user = m;
        }
        public void AddBoard()
        {
            user.Controller.AddBoard(user, Title);
            Title = "";
        }

        internal void TitleChanged()
        {
            RaisePropertyChanged("Title");
        }
    }
}
