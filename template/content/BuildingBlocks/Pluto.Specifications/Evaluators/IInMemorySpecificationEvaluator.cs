using System.Collections.Generic;

namespace Pluto.Specifications.Evaluators
{
    public interface IInMemorySpecificationEvaluator
    {
        IEnumerable<TResult> Evaluate<T, TResult>(IEnumerable<T> source, ISpecification<T, TResult> specification);
        IEnumerable<T> Evaluate<T>(IEnumerable<T> source, ISpecification<T> specification);
    }
}