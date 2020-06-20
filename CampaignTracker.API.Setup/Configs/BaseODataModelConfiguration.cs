using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization;
using System.Reflection;

namespace CampaignTracker.API.Setup.Configs
{
	public abstract class BaseODataModelConfiguration
		: IModelConfiguration
	{
		/// <summary>
		/// Apply method
		/// </summary>
		/// <param name="builder">model builder instance</param>
		/// <param name="apiVersion">api version</param>
		public abstract void Apply(ODataModelBuilder builder, ApiVersion apiVersion);

		/// <summary>
		/// Configure the OData entity set for the specified type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The runtime type to configure the entity set for.</typeparam>
		/// <param name="builder">ODataModelBuilder instance</param>
		/// <returns>EntitySetConfiguration</returns>
		protected EntitySetConfiguration<T> ConfigureEntitySet<T>(ODataModelBuilder builder)
			where T : class
		{
			EntitySetConfiguration<T> config = builder.EntitySet<T>(typeof(T).Name);
			this.ConfigureEntityType<T>(builder);
			return config;
		}

		/// <summary>
		/// Configure the OData entity type for the specified type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The runtime type to configure the entity set for.</typeparam>
		/// <param name="builder">ODataModelBuilder instance</param>
		/// <returns>The <see cref="EntityTypeConfiguration{T}"/>.</returns>
		protected EntityTypeConfiguration<T> ConfigureEntityType<T>(ODataModelBuilder builder)
			where T : class
		{
			builder.Namespace = "fn";

			EntityTypeConfiguration<T> entityType = builder.EntityType<T>();
			if (typeof(T).IsAbstract)
			{
				entityType.Abstract();
			}

			/* ..first, set the namespace and name for the object based on its
             * data contract, else its runtime namespace and class name..
             */
			DataContractAttribute attrib = typeof(T).GetCustomAttribute<DataContractAttribute>();
			entityType.Namespace = attrib?.Namespace ?? typeof(T).Namespace;
			entityType.Name = attrib?.Name ?? typeof(T).Name;
			entityType.OrderBy()
				.Expand()
				.Select()
				.Filter()
				.Count()
				.Page();

			return entityType;
		}
	}
}
