namespace EntityFrameworkCore.Extension.UnitOfWork.Specifications.Builder
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TProperty"></typeparam>
    public interface IIncludableSpecificationBuilder<T, out TProperty> : ISpecificationBuilder<T>
    {
    }
}
