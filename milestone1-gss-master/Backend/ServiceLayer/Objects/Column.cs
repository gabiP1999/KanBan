using System;

namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct Column
    {
        public readonly int Id;
        public readonly string Name;
        public readonly int Limit;
        
        internal Column(int id, string name, int limit)
        {
            this.Id = id;
            this.Name = name;
            this.Limit = limit;
        }
    }
}
