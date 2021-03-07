using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Domain.Entities
{
    public abstract class BaseEntity
    {
        public abstract object[] GetKeys();
    }

    public abstract class BaseEntity<TKey> : BaseEntity
    {
        [AllowNull]
        public TKey Id { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { Id! };
        }
    }
}
