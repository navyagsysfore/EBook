namespace EBook.Models
{
    public class UpdateBook
    {
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

        public DateTime Updated_At = DateTime.Now;

        public int GenreID { get; set; }
    }
}
