using System.ComponentModel.DataAnnotations;

namespace websiteCoffee.Models.Coffee
{
    public class CoffeeItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Fadlan qor magaca qaxwada")]
        [Display(Name = "Magaca Qaxwada")]
        public string Name { get; set; } = string.Empty;

        // --- QAYBTII 'TYPE' WAA LAGA SAARAY HALKAN ---

        [Required(ErrorMessage = "Fadlan geli qiimaha")]
        [Range(0.01, 999.99, ErrorMessage = "Qiimuhu waa inuu ka sarreeyaa 0")]
        [Display(Name = "Qiimaha ($)")]
        public decimal Price { get; set; }

        [Display(Name = "Sawirka Qaxwada")]
        public string ImageUrl { get; set; } = "default-coffee.jpg";
    }
}