namespace IntroSE.Kanban.Backend.ServiceLayer.Objects
{
    public struct User
    {
        public readonly string Email;

        internal User(string email)
        {
            this.Email = email;
        }
    }
}
