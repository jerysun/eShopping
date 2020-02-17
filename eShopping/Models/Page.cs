using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace eShopping.Models
{
    public class Page
    {
        public int Id { get; set; } //will be automatically recognized as Primary Key once migration starts
        [Required]
        public string Title { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Content { get; set; }
        public int Sorting { get; set; }
    }
}
