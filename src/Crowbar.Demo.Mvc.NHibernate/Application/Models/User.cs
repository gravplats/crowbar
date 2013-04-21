namespace Crowbar.Demo.Mvc.NHibernate.Application.Models
{
    public class User
    {
        // NHibernate.
        protected User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = Password.Generate(password);
        }

        public virtual int Id { get; set; }

        public virtual Password Password { get; protected set; }
        public virtual string Username { get; protected set; }
    }
}