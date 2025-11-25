using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class SeedDiscountRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Regra 1: 4-9 itens = 10% desconto
            migrationBuilder.Sql(@"
                INSERT INTO ""DiscountRules"" (""Id"", ""MinQuantity"", ""MaxQuantity"", ""DiscountPercentage"", ""IsActive"", ""CreatedAt"")
                VALUES (gen_random_uuid(), 4, 9, 10.00, true, CURRENT_TIMESTAMP);
            ");

            // Regra 2: 10-20 itens = 20% desconto
            migrationBuilder.Sql(@"
                INSERT INTO ""DiscountRules"" (""Id"", ""MinQuantity"", ""MaxQuantity"", ""DiscountPercentage"", ""IsActive"", ""CreatedAt"")
                VALUES (gen_random_uuid(), 10, 20, 20.00, true, CURRENT_TIMESTAMP);
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove as regras de desconto inseridas
            migrationBuilder.Sql(@"
                DELETE FROM ""DiscountRules""
                WHERE (""MinQuantity"" = 4 AND ""MaxQuantity"" = 9 AND ""DiscountPercentage"" = 10.00)
                   OR (""MinQuantity"" = 10 AND ""MaxQuantity"" = 20 AND ""DiscountPercentage"" = 20.00);
            ");
        }
    }
}
