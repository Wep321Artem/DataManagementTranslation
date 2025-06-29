using Microsoft.EntityFrameworkCore;
using DataManagementTranslation.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataManagementTranslation.Configurations
{
    public class СlientsConfiguration : IEntityTypeConfiguration<Clients>
    {
       public void Configure(EntityTypeBuilder<Clients> builder)
        {
            builder.HasKey(a => a.Id); // Первичный ключ
        }

    }
}
