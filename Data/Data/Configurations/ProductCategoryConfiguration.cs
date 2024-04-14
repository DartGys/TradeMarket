using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Configurations
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasKey(pc => pc.Id);

            builder.Property(pc => pc.CategoryName)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasMany(pc => pc.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
