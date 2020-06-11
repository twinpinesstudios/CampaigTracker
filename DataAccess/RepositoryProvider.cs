using Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DataAccess
{
    public class RepositoryProvider
        : IRepositoryProvider
    {
        private DbContext context;
        private Dictionary<string, object> repositoryCache;

        public RepositoryProvider(DbContext context)
        {
            this.context = context;
            this.repositoryCache = new Dictionary<string, object>();
        }

		/// <summary>
		/// Returns the repository instance for the specified runtime type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The runtime type of the repository to retrieve.</typeparam>
		/// <returns>A <see cref="IRepository{T}"/> instance, when supported by the provider.</returns>
		/// <exception cref="System.NotSupportedException">When the runtime type <typeparamref name="T"/> is not supported.</exception>
		public IRepository<T> RepositoryFor<T>()
			where T : class
		{
			if (this.Supports(typeof(T)))
			{
				if (!this.repositoryCache.ContainsKey(typeof(T).FullName))
				{
					this.repositoryCache.Add(typeof(T).FullName, new Repository<T>(context));
				}

				return this.repositoryCache[typeof(T).FullName] as Repository<T>;
			}
			throw new NotSupportedException();
		}

		/// <summary>
		/// Determines whether the specified runtime <paramref name="type"/> is supported by the current provider.
		/// </summary>
		/// <param name="type">The runtime type to determine support for.</param>
		/// <returns><c>true</c> if a repository for the specified <paramref name="type"/> is supported, otherwise <c>false</c>.</returns>
		public bool Supports(Type type) => null != context.Model.FindRuntimeEntityType(type);
	}
}
