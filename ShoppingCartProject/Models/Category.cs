using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCartProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        public string Name { get; set; }

        public virtual List<ShoppingItem> shoppingItems { get; set; }
    }
}