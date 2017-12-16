using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShoppingCartProject.Models
{
    public class Review
    {
        public Review()
        {
            reviewDate = DateTime.Now;

        }
        public int Id { get; set; }
        [Display(Name = "Header")]
        public string subjectLine { get; set; }
        [Display(Name = "Description")]
        public string description { get; set; }

        public DateTime reviewDate { get; set; }
        public virtual int ShoppingItemId { get; set; }

        public virtual ShoppingItem shopItem { get; set; }
    }
}