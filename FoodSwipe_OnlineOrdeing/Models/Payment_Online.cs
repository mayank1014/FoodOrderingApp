using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Models
{
    public class Payment_Online
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(1000000000000000,9999999999999999, ErrorMessage = "please enter valid card number")]
        public long CardNumber { get; set; }


        [Required]
        [Range(1, 12, ErrorMessage = "please enter valid month")]
        public int Month { get; set; }


        [Required]
        [Range(2021,2030 , ErrorMessage = "please enter valid year")]
        public int Year { get; set; }


        [Required]
        [Range(100, 999, ErrorMessage = "please enter valid cvv")]
        public int Cvv { get; set; }
    }
}
