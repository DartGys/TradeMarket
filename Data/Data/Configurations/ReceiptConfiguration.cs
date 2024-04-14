using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
    {
        public void Configure(EntityTypeBuilder<Receipt> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.OperationDate)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(r => r.IsCheckedOut)
                .HasColumnType("bit")
                .IsRequired();

            builder.HasOne(r => r.Customer)
                .WithMany(r => r.Receipts)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(r => r.ReceiptDetails)
                .WithOne(r => r.Receipt)
                .HasForeignKey(r => r.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
