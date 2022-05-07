using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SuperMarket.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Persistence.EF.PurchaseVouchers
{
    public class PurchasesEntityMap : IEntityTypeConfiguration<PurchaseVoucher>
    {
        public void Configure(EntityTypeBuilder<PurchaseVoucher> builder)
        {
            builder.ToTable("PurchaseVoucher");
            builder.HasKey(_ => _.Id);
            builder.Property(_ => _.Id)
                .ValueGeneratedOnAdd()
                .IsRequired();
            builder.Property(_ => _.Name)
                .IsRequired();
            builder.Property(_ => _.TotalPrice)
                .IsRequired();
            builder.Property(_ => _.ProductId)
                .IsRequired();

            builder.HasOne(_ => _.Product)
               .WithOne(_ => _.PurchaseVoucher)
               .HasForeignKey<PurchaseVoucher>(_ => _.ProductId)
               .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
