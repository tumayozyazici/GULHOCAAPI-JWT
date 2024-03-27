using CodeFirstAPI.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CodeFirstAPI.EntityTypeConfiguration
{
    public class UserEntityConfig : BaseEntityConfig<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasData(new User() {Id=1, Password = "123", UserName = "gul" });
        }
    }
}
