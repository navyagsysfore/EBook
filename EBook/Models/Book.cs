namespace EBook.Models
{
    public class Book
    {
        DateTime current = DateTime.Now;
        public Book()
        {
        }

        public Book(BookDTO book)
        {
            BookID = Guid.NewGuid();
            Title = book.Title;
            Description = book.Description;
            ISBN = book.ISBN;
            PublicationDate = book.PublicationDate;
            Price = book.Price;
            Language = book.Language;
            Publisher = book.Publisher;
            PageCount = book.PageCount;
            AverageRating = book.AverageRating;
            GenreID = book.GenreID;
            Created_At = DateTime.Now;
            Updated_At = DateTime.Now;
        }

        public Guid BookID { get; set; }  
        public string Title { get; set; }
        public string Description { get; set; }
        public int ISBN { get; set; }
        public DateTime PublicationDate { get; set; }
        public int Price { get; set; }
        public string Language { get; set; }
        public string Publisher { get; set; }
        public int PageCount { get; set; }
        public double? AverageRating { get; set; }

      //  public int TotalRating { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
        public int GenreID { get; set; }  

        
        public ICollection<Author> Authors { get; set; }
        public ICollection<AuthorBookMappings> AuthorBookMappings { get; set; }
    }
}
