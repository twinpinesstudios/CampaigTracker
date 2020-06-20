using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;

namespace DataAccess.Configs
{
    public abstract class BaseEntityConfig<T> :
        IEntityTypeConfiguration<T>
        where T : class
    {
        public abstract void Configure(EntityTypeBuilder<T> builder);

        internal static ValueConverter ListStringStringConverter = new ValueConverter<ICollection<string>, string>(
                v => (null != v && v.Count != 0) ? Newtonsoft.Json.JsonConvert.SerializeObject(v) : String.Empty,
                v => (null != v) ? Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(v) : new List<string>()
            );
    }
}
