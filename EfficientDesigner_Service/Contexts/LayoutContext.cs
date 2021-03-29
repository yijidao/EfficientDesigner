using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EfficientDesigner_Service.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EfficientDesigner_Service.Contexts
{
    class LayoutContext : DbContext
    {
        public DbSet<Layout> Layouts { get; set; }

        public DbSet<PropertyBinding> PropertyBindings { get; set; }

        public DbSet<DataSource> DataSources { get; set; }

        public DbSet<ServiceInfo> ServiceInfos { get; set; }

        public static JsonConfig Config { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Config == null)
            {
                using (var stream = new StreamReader("config.json", Encoding.UTF8))
                {
                    var json = stream.ReadToEnd();
                    Config = JsonConvert.DeserializeObject<JsonConfig>(json);
                }
            }
            var connectString = Config?.DbAddress ?? "";
            optionsBuilder.UseSqlite(connectString);
        }


        public override int SaveChanges()
        {

            var addedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            addedEntities.ForEach(e =>
            {
                e.Property("CreateTime").CurrentValue = DateTime.Now;
                e.Property("UpdateTime").CurrentValue = DateTime.Now;
            });

            var modifiedEntities = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            modifiedEntities.ForEach(e =>
            {
                e.Property("UpdateTime").CurrentValue = DateTime.Now;
            });

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ServiceInfo>()
                .HasAlternateKey(x => x.Name);
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    //base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Layout>()
        //        .Property(l => l.CreateTime)
        //        .ValueGeneratedOnAdd()
        //        .HasDefaultValue("datetime('now', 'localtime')");
        //    modelBuilder.Entity<Layout>()
        //        .Property(l => l.UpdateTime)
        //        .ValueGeneratedOnAddOrUpdate()
        //        .HasDefaultValue("datetime('now', 'localtime')");
        //    modelBuilder.Entity<PropertyBinding>()
        //        .Property(p => p.CreateTime)
        //        .ValueGeneratedOnAdd()
        //        .HasDefaultValue("datetime('now', 'localtime')");
        //    modelBuilder.Entity<PropertyBinding>()
        //        .Property(p => p.UpdateTime)
        //        .ValueGeneratedOnAddOrUpdate()
        //        .HasDefaultValue("datetime('now', 'localtime')");

        //}
    }

    class JsonConfig
    {
        public string DbAddress { get; set; }
    }
}
