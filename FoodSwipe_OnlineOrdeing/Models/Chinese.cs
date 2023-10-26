using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FoodOrderingSystem.Models
{
    public partial class Chinese
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
        public string PhotoPath { get; set; }

    }
}
