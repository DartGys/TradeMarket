using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.DiscountValue)
                .IsRequired();

            builder.HasOne(c => c.Person)
                .WithMany()
                .HasForeignKey(c => c.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Receipts)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);
        }
    }
}
