namespace Crowbar.Demo.Mvc.Raven.Application.Models
{
    public class User
    {
        // RavenDB.
        protected User() { }

        public User(string username, string password)
        {
            Username = username;
            Password = Password.Generate(password);
        }

        public Password Password { get; private set; }
        public string Username { get; private set; }
    }
}