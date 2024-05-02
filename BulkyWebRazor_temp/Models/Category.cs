using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_temp.Models
{
    public class Category
    {
        // properties are the column names that is going to be created in the database
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }

    }
}
