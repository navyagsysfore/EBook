using System.ComponentModel.DataAnnotations;

namespace EBook.Models
{
    public class BookDTO
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description length can't be more than 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "ISBN is required")]

        public int ISBN { get; set; }

        [Required(ErrorMessage = "Publication Date is required")]
        public DateTime PublicationDate { get; set; }

        [Required(ErrorMessage = "Price is required")]

        public int Price { get; set; }

        [Required(ErrorMessage = "Language is required")]
        [StringLength(50, ErrorMessage = "Language length can't be more than 50 characters.")]
        public string Language { get; set; }

        [StringLength(100, ErrorMessage = "Publisher length can't be more than 100 characters.")]
        public string Publisher { get; set; }

        [Required(ErrorMessage = "Page Count is required")]

        public int PageCount { get; set; }

        [Required(ErrorMessage = "Range is required")]

        public double? AverageRating { get; set; }

        [Required(ErrorMessage = "TotalRating is required")]
        public int TotalRating { get; set; }

        [Required(ErrorMessage = "Genre ID is required")]
        public int GenreID { get; set; }

        [Required(ErrorMessage = "At least one Author ID is required")]
        public List<Guid> ID { get; set; }
    }
}
