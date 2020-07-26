using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceRequestManagement.Domain.ServiceRequestAggregate;
using System;

namespace ServiceRequestManagement.Infrastructure.EntityConfigurations
{
    /// <summary>
    /// The entity type configuration implementation for migrating the ServiceRequest entity to SQLServer.
    /// </summary>
    public class ServiceRequestEntityTypeConfiguration : IEntityTypeConfiguration<ServiceRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceRequest> entityBuilder)
        {
            // Set the primary key of the ServiceRequest entity.
            entityBuilder
                .HasKey(serviceRequest => serviceRequest.Id);

            // Ignore the DomainEvents list. We don't need it, and we'd ignore it even if we did.
            entityBuilder
                .Ignore(serviceRequest => serviceRequest.DomainEvents);

            // Ensure the Id of our ServiceRequest entity is generated when added to the Db.
            entityBuilder
                .Property(serviceRequest => serviceRequest.Id)
                .ValueGeneratedOnAdd();

            // Map our properties to columns
            entityBuilder
                .Property<string>("_buildingCode")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("BuildingCode")
                .IsRequired();

            entityBuilder
                .Property<string>("_description")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Description")
                .IsRequired();

            entityBuilder
                .Property<string>("_createdBy")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CreatedBy")
                .IsRequired();

            entityBuilder
                .Property<DateTime>("_createdDate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("CreatedDate")
                .IsRequired();

            entityBuilder
                .Property<string>("_lastModifiedBy")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("LastModifiedBy")
                .IsRequired();

            entityBuilder
                .Property<DateTime?>("_lastModifiedDate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("LastModifiedDate")
                .IsRequired();
        }
    }
}
