using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCartProject.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<ShoppingCartProject.Models.ShoppingItem> ShoppingItems { get; set; }

        public System.Data.Entity.DbSet<ShoppingCartProject.Models.Category> Categories { get; set; }

        public System.Data.Entity.DbSet<ShoppingCartProject.Models.Review> Reviews { get; set; }
    }
}