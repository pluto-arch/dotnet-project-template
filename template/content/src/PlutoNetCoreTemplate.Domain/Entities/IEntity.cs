namespace PlutoNetCoreTemplate.Domain.Entities
{
    public interface IEntity
    {
        /// <summary>
        /// Returns an array of ordered keys for this entity.
        /// </summary>
        /// <returns></returns>
        object[] GetKeys();
    }

    public interface IEntity<TKey>
    {
        /// <summary>
        /// Unique identifier for this entity.
        /// </summary>
        TKey Id { get; }
    }
}