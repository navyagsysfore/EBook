namespace EBook.Models
{
    public class Users
    {
        public Users()
        {

        }

        public Users(UserDTO users)
        {
            this.Username = users.Username;
            this.Password = users.Password;
        }
        public string Username { get; set; }  
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
