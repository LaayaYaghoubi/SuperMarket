using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarket.Migrations
{
    [Migration(202201050858)]
    public class _202201050858_InitialDataBase : Migration
    {
        public override void Up()
        {
            CreateCategoriesTable();
            CreateProductsTable();
            CreateSalesInvoicesTable();
            CreatePurchaseVouchersTable();
        }   
        public override void Down()
        {
            Delete.Table("Products");
            Delete.Table("Categories");
            Delete.Table("PurchaseVouchers");
            Delete.Table("SalesInvoices");
        }
        private void CreatePurchaseVouchersTable()
        {
           Create.Table("PurchaseVouchers")
            .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
            .Identity()
            .WithColumn("Name").AsString(50).NotNullable()
            .WithColumn("TotalPrice").AsDecimal().NotNullable()
            .WithColumn("Count").AsInt32().NotNullable()
            .WithColumn("DateOfPurchase").AsDateTime2().Nullable()
            .WithColumn("ProductId").AsInt32().NotNullable()
            .ForeignKey("FK_PurchaseVouchers_Products", "Products", "Id")
                     .OnDelete(System.Data.Rule.None);
        }
        private void CreateSalesInvoicesTable()
        {
            Create.Table("SalesInvoices")
              .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
              .Identity()
              .WithColumn("ClientName").AsString(50).NotNullable()
              .WithColumn("TotalPrice").AsDecimal().NotNullable()
              .WithColumn("Count").AsInt32().NotNullable()
               .WithColumn("DateOfSale").AsDateTime2().Nullable()
              .WithColumn("ProductId").AsInt32().NotNullable()
              .ForeignKey("FK_SalesInvoices_Products", "Products", "Id")
                          .OnDelete(System.Data.Rule.None);
        }
        private void CreateProductsTable()
        {
            Create.Table("Products")
              .WithColumn("Id").AsInt32().PrimaryKey().NotNullable().Identity()
              .WithColumn("Code").AsInt32().NotNullable()
              .WithColumn("Name").AsString(50).NotNullable()
              .WithColumn("Price").AsDecimal().NotNullable()
              .WithColumn("Stock").AsInt32().Nullable()
              .WithColumn("MinimumStock").AsInt32().NotNullable()
              .WithColumn("MaximumStock").AsInt32().NotNullable()
               .WithColumn("CategoryId").AsInt32().NotNullable()
              .ForeignKey("FK_Products_Categories", "Categories", "Id")
                 .OnDelete(System.Data.Rule.None);
        }

        private void CreateCategoriesTable()
        {
            Create.Table("Categories")
                       .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                       .Identity()
                       .WithColumn("Name").AsString(50).NotNullable();
        }

    }
}
