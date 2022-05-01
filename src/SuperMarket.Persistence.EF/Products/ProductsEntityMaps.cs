using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Products
{
    public class ProductsEntityMaps : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(_ => _.Name)
                .IsRequired();
            builder.Property(_ => _.Price)
                .IsRequired();
            builder.Property(_ => _.MinimumStock)
                .IsRequired();
            builder.Property(_ => _.MaximumStock).
                IsRequired();
            builder.Property(_ => _.CategoryId)
                .IsRequired();

          




        }
    }
}
