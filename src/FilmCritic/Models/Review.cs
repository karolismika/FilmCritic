using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmCritic.Models
{
    public class Review
    {
        public int ID { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Name { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Review")]
        [StringLength(2000, MinimumLength = 150)]
        public string ReviewText { get; set; }

        [Required]
        [Display(Name = "Film")]
        public string FilmTitle { get; set; }
    }
}
