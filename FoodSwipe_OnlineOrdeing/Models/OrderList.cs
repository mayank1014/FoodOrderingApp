using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace FoodOrderingSystem.Models
{
    public partial class OrderList
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public int Quantity { get; set; }
        public string TotalPrice { get; set; }

        public string Email { get; set; }

       
    }
}
