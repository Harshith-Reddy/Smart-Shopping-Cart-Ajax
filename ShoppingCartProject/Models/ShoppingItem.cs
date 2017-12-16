using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCartProject.Models
{
    public class ShoppingItem
    {
        public ShoppingItem()
        {
            DateAdded = DateTime.Now;
            Quantity = 0;
            IsAddedToCart = false;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "ItemName is required")]
        public string ItemName { get; set; }
        public DateTime DateAdded { get; set; }
        [Required(ErrorMessage = "Item Description is required")]
        public string Description { get; set; }
        public bool IsAddedToCart { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public float Price { get; set; }
        public int Quantity { get; set; }
        public virtual int CategoryId { get; set; }
        public virtual Category category { get; set; }
        
        public virtual List<Review> reviews { get; set; }

        public void updateQuantity(int n)
        {
            if (n >= 0)
            {
                IsAddedToCart = true;
                Quantity = n;
            }
            if (Quantity == 0)
            {
                IsAddedToCart = false;
            }
            
        }
        
    }
}