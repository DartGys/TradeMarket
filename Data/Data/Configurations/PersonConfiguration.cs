using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.Surname)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(p => p.BirthDate)
                .HasColumnType("date")
                .IsRequired();
        }
    }
}
