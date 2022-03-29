﻿using System.Collections.Generic;

namespace Pluto.Specifications.Evaluators
{
    public interface IInMemoryEvaluator
    {
        IEnumerable<T> Evaluate<T>(IEnumerable<T> query, ISpecification<T> specification);
    }
}