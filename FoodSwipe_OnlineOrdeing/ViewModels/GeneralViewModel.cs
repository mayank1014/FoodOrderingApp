using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.ViewModels
{
    public class GeneralViewModel
    {
        public int Id { get; set; }
        public string Item { get; set; }
        public string Price { get; set; }
        public IFormFile Photo { get; set; }
        public string  ExistingPhotoPath{get;set;}
    }
}
