using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmCritic.Models
{
    public class Film
    {
        public int ID { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Title { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Required]
        [StringLength(30)]
        public string Genre { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string Link { get; set; }
    }
}
