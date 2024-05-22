namespace EBook.Models
{
    public class AuthorBookMappings
    {
        public Guid AuthorID { get; set; }
        public Author Author { get; set; }

        public Guid BookID { get; set; }
        public Book Book { get; set; }
    }
}
