using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Data.Configurations
{
    public class ReceiptDetailConfiguration : IEntityTypeConfiguration<ReceiptDetail>
    {
        public void Configure(EntityTypeBuilder<ReceiptDetail> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(rd => rd.DiscountUnitPrice)
                .IsRequired();

            builder.Property(rd => rd.UnitPrice)
                .IsRequired();

            builder.Property(rd => rd.Quantity)
                .IsRequired();

            builder.HasOne(rd => rd.Receipt)
                .WithMany(r => r.ReceiptDetails)
                .HasForeignKey(rd => rd.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rd => rd.Product)
                .WithMany(p => p.ReceiptDetails)
                .HasForeignKey(rd => rd.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
