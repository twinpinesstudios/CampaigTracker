using Factions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace DataAccess.Configs
{
    internal class FactionEntityConfiguration
        : BaseEntityConfig<Faction>
    {
        public override void Configure(EntityTypeBuilder<Faction> builder)
        {
            builder.UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

            builder.ToTable("Factions", "dbo")
                .HasKey(x => x.Id)
                .IsClustered(false);

            builder.Property(x => x.Id)
                .HasValueGenerator<GuidValueGenerator>()
                .HasColumnName("Id")
                .IsRequired();

            builder.Property(x => x.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(x => x.Keywords)
                .HasColumnName("KeyWords")
                .HasConversion(ListStringStringConverter)
                .HasMaxLength(Int32.MaxValue)
                .IsUnicode();
        }
    }
}
