using Microsoft.EntityFrameworkCore;

using PlutoNetCoreTemplate.Domain.Aggregates.ProjectAggregate;

namespace PlutoNetCoreTemplate.Infrastructure.EntityTypeConfigurations
{
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(20);
        }
    }


    public class ProjectGroupEntityTypeConfiguration : IEntityTypeConfiguration<ProjectGroup>
    {
        public void Configure(EntityTypeBuilder<ProjectGroup> builder)
        {
            builder.ToTable("ProjectGroups", Constants.DbConstants.DefaultTableSchema);
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(20);
            builder.Property(e => e.Remark).HasMaxLength(100);
        }
    }
}