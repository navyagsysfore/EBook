namespace EBook.Models
{
    public class UpdateAuthor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }

        public DateTime Updated_At = DateTime.Now;

    }
}
