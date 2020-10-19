using System;
using System.Collections.Generic;
using System.Text;
using EfficientDesigner_Service.Models;
using Microsoft.EntityFrameworkCore;

namespace EfficientDesigner_Service.Contexts
{
    class LayoutContext : DbContext
    {
        public DbSet<Layout> Layouts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = Efficient_Service.db");
        }
    }
}
