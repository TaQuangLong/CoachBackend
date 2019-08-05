using EverCoach.Domain.AggregatesModel.CoachAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverCoach.Infrastructure.EntityConfigurations
{
    public class CoachEntityTypeConfiguration : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.ToTable("Coaches");
            builder.HasKey(x => x.Id);
            builder.Ignore(c => c.DomainEvents);
            //var navigation = builder.Metadata.FindNavigation(nameof(Coach.Coaches));

            // DDD Patterns comment:
            //Set as field (New since EF 1.1) to access the OrderItem collection property through its field
            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
