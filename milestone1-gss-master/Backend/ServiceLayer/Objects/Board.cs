using System;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct Board
    {
        public readonly int Id;
        public readonly string BoardName;
        public readonly string CreatorEmail;

        internal Board(int id, string boardName, string creatorEmail)
        {
            this.Id = id;
            this.BoardName = boardName;
            this.CreatorEmail = creatorEmail;
        }
    }
}
