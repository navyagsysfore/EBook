namespace EBook.Models
{
    public class Genre
    {
        public int GenreID { get; set; }  
        public string Name { get; set; }
        public DateTime Updated_At { get; set; }


        public ICollection<Book> Books { get; set; }
    }
}
