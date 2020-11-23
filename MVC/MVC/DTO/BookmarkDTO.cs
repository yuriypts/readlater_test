using System.ComponentModel.DataAnnotations;

namespace MVC.DTO
{
    public class BookmarkDTO
    {
        public int ID { get; set; }

        [Required]
        [StringLength(maximumLength: 500)]
        public string URL { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        public string CategoryName { get; set; }
    }
}