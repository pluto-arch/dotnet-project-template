using System.Diagnostics.CodeAnalysis;

namespace PlutoNetCoreTemplate.Domain.Entities
{
    public abstract class BaseEntity : IEntity
    {
        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {string.Join(",", GetKeys())}";
        }

        public bool EntityEquals(IEntity other)
        {
            return EntityHelper.EntityEquals(this, other);
        }

        public abstract object[] GetKeys();
    }

    public abstract class BaseEntity<TKey> : BaseEntity, IEntity<TKey>
    {
        [AllowNull]
        public TKey Id { get; set; }

        public override object[] GetKeys()
        {
            return new object[] { Id };
        }
    }
}
