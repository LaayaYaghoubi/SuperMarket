using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.SalesInvoices
{
    public class SalesInvoicesEntityMap : IEntityTypeConfiguration<SalesInvoice>
    {
        public void Configure(EntityTypeBuilder<SalesInvoice> builder)
        {
            builder.ToTable("SalesInvoices");

            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id).ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(_ => _.ClientName)
                .IsRequired();
            builder.Property(_ => _.NumberOfProducts)
                .IsRequired();
            builder.Property(_ => _.ProductId)
                .IsRequired();
            builder.Property(_ => _.Date)
                .IsRequired();
            builder.Property(_ => _.TotalPrice)
                .IsRequired();

            builder.HasMany(_ => _.Product)
               .WithOne(_ => _.SalesInvoice)
               .HasForeignKey(_ => _.SalesInvoiceId)
               .OnDelete(DeleteBehavior.ClientNoAction);

        }
    }
}
