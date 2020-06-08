using Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccess
{
	public class Repository<T>
		: IRepository<T>
		where T : class
	{
		private DbContext dbContext;


		/// <summary>
		/// Provides functionality to evaluate queries against a specific data source wherein the 
		/// type of the data is known.
		/// </summary>
		/// <param name="queryOptions">Additional settings for the query.</param>
		/// <returns>A queryable data source.</returns>
		public IQueryable<T> CreateQuery()
		{
			IQueryable<T> query = null;
			var dbSet = dbContext.Set<T>();

			return query.AsNoTracking();
		}

		/// <summary>
		/// Creates a new object record in the data store.
		/// </summary>
		/// <param name="collection">The object instance to persist.</param>
		/// <typeparam name="T">The type of the object stored in the repository.</typeparam>
		/// <returns>The data object.</returns>
		public IEnumerable<T> Create(IEnumerable<T> collection)
		{
			DbSet<T> s = dbContext.Set<T>();

			// Add to the database
			s.AddRange(collection);

			// Trigger Save
			dbContext.SaveChanges();

			return collection;
		}

		/// <summary>
		/// Returns the object that is identified by the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key(s) used to identify the object record.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
		/// <typeparam name="T">The type of the object stored in the repository.</typeparam>
		/// <returns>The object instance.</returns>
		public T SingleAsync(Expression<Func<T, bool>> keyExpression)
		{
			try
			{
				IQueryable<T> query = dbContext.Set<T>();

				return query.Single<T>(keyExpression);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Returns the object that is identified by the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="key">The key(s) used to identify the object record.</param>
		/// <typeparam name="T">The type of the object stored in the repository.</typeparam>
		/// <returns>The object instance.</returns>
		public T Single(object[] key)
		{
			try
			{
				IQueryable<T> query = dbContext.Set<T>();

				var declaredKeys = dbContext.Model.FindEntityType(typeof(T))
					.GetKeys()
					.Where(x => x.IsPrimaryKey())
					.SelectMany(x => x.Properties);

				string[] whereClause = new string[declaredKeys.Count()];
				int i = 0;
				Dictionary<string, object> parameters = new Dictionary<string, object>();

				return query.FirstOrDefault(BuildLambda(declaredKeys, new ValueBuffer(key)));
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Updates an existing object in the data store.
		/// </summary>
		/// <param name="collection">The object instance to persist.</param>
		/// <typeparam name="T">The type of the object stored in the repository.</typeparam>
		/// <returns><c>True</c> if the update was successful, otherwise <c>false</c>.</returns>
		public bool UpdateAsync(IEnumerable<T> collection)
		{
			DbSet<T> s = dbContext.Set<T>();

			// Fire update on the repostiory
			s.UpdateRange(collection);

			// Save Changes
			var updateResult = dbContext.SaveChanges();

			return updateResult != 0;
		}

		/// <summary>
		/// Removes an existing data object from the underlying data store.
		/// </summary>
		/// <param name="collection">The object instance to delete.</param>
		/// <typeparam name="T">The type of the object stored in the repository.</typeparam>
		/// <returns><c>True</c> if the object was successfully deleted, otherwise <c>false</c>.</returns>
		public bool Delete(IEnumerable<T> collection)
		{
			DbSet<T> s = dbContext.Set<T>();

			s.RemoveRange(collection);

			// Save Changes
			var deleteResult = dbContext.SaveChanges();

			return deleteResult != 0;
		}

		#region private methods

		private static Expression<Func<T, bool>> BuildLambda(IEnumerable<IProperty> keyProperties, ValueBuffer keyValues)
		{
			var entityParameter = Expression.Parameter(typeof(T), "e");

			return Expression.Lambda<Func<T, bool>>(
				BuildPredicate(keyProperties, keyValues, entityParameter), entityParameter);
		}

		private static BinaryExpression BuildPredicate(IEnumerable<IProperty> keyProperties, ValueBuffer keyValues, ParameterExpression entityParameter)
		{
			var keyValuesConstant = Expression.Constant(keyValues);

			var predicate = GenerateEqualExpression(keyProperties.First(), 0);

			for (var i = 1; i < keyProperties.Count(); i++)
			{
				predicate = Expression.AndAlso(predicate, GenerateEqualExpression(keyProperties.ElementAt(i), i));
			}

			return predicate;

			BinaryExpression GenerateEqualExpression(IProperty property, int i) =>
				Expression.Equal(
					Expression.Call(
						typeof(EF).GetTypeInfo().GetDeclaredMethod(nameof(Property)).MakeGenericMethod(property.ClrType),
						entityParameter,
						Expression.Constant(property.Name, typeof(string))),
					Expression.Convert(
						Expression.Call(
							keyValuesConstant,
							typeof(ValueBuffer).GetRuntimeProperties().Single(p => p.GetIndexParameters().Length > 0).GetMethod,
							Expression.Constant(i)),
						property.ClrType));
		}
		#endregion
	}
}
