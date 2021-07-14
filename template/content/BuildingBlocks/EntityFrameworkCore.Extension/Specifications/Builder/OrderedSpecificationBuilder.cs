namespace EntityFrameworkCore.Extension.UnitOfWork.Specifications.Builder
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class OrderedSpecificationBuilder<T> : IOrderedSpecificationBuilder<T>
    {
        /// <summary>
        /// 
        /// </summary>
        public Specification<T> Specification { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="specification"></param>
        public OrderedSpecificationBuilder(Specification<T> specification) => Specification = specification;
    }
}
