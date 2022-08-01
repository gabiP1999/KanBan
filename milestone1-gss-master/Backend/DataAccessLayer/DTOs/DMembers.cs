using System;

namespace IntroSE.Kanban.DataAccessLayer.DTOs
{
    public class DMembers : DTO
    {
        public const string MembersColumnName = "Email";


        private string _members;
        public string Members { get => _members; set { _members = value; _controller.Update(Id, MembersColumnName, value); } }



        public DMembers(long ID, string members) : base(new DMembersController())
        {
            Id = ID;
            _members = members;
        }

        internal void InsertMember()
        {
            ((DMembersController)_controller).Insert(this);
        }

        internal void RemoveMember()
        {
            ((DMembersController)_controller).Remove(this);
        }
    }
}
