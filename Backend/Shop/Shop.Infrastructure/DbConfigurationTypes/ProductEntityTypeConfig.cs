﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shop.Domain.Domain;

namespace Shop.Infrastructure.DbConfigurationTypes
{
    internal class ProductEntityTypeConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();

            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.CodeNumber).IsRequired().HasMaxLength(6);
            builder.Property(p => p.SeriesNumber).IsRequired().HasMaxLength(8);
            builder.Property(p => p.Description).HasMaxLength(5000);
            builder.Property(p => p.ReleaseDate).HasColumnType("date");
            builder.Property(p => p.CreationDate).IsRequired().HasColumnType("date").HasDefaultValue(DateTime.UtcNow);

            builder.HasData(SeedDataProvider.ProductsSeed);
        }
    }
}