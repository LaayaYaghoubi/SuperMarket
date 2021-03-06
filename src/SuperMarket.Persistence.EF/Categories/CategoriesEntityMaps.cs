using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.Categories
{
    public class CategoriesEntityMaps : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd().
                IsRequired();

            builder.Property(_ => _.Name).IsRequired();

            builder.HasMany(_ => _.products)
              .WithOne(_ => _.Category)
              .HasForeignKey(_ => _.CategoryId)
              .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
