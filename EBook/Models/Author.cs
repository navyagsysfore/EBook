namespace EBook.Models
{
     public class Author
    {
        DateTime current = DateTime.Now;
        public Author()
        {
            
        }

        public Author(AuthorDTO author)
        {
            ID = Guid.NewGuid();
            FirstName = author.FirstName;
            LastName = author.LastName;
            Biography = author.Biography;
            BirthDate = author.BirthDate;
            Country = author.Country;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public Guid ID { get; set; }  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public DateTime BirthDate { get; set; }
        public string Country { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public DateTime getCreated_At()
        {
            return this.Created_At;
        }

        
        public ICollection<Book> Books { get; set; }
        public ICollection<AuthorBookMappings> AuthorBookMappings { get; set; }
    }
}

