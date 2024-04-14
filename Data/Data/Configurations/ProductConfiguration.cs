using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProductName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ReceiptDetails)
                .WithOne(rd => rd.Product)
                .HasForeignKey(rd => rd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
