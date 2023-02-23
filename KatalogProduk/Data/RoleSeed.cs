using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace KatalogProduk.Data
{
    public class RoleSeed : IEntityTypeConfiguration<IdentityRole>
    {
        readonly String UserRule = "user";
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Id = "cbc43a8e-f7bb-4445-baaf-1add431ffbbf",
                Name = UserRule,
                NormalizedName = UserRule.ToUpper()
            }
            );
        }
    }
}

