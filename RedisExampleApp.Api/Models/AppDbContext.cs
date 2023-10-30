﻿using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.Api.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem", Price = 25M },
                new Product() { Id = 2, Name = "0.5 Uç", Price = 10M },
                new Product() { Id = 3, Name = "Silgi", Price = 15.5M }
                );


            base.OnModelCreating(modelBuilder);
        }
    }
}
