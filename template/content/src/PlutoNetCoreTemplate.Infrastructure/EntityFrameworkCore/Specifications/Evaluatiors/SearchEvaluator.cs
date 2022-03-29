﻿using Pluto.Specifications;
using Pluto.Specifications.Evaluators;

using System.Linq;

namespace PlutoNetCoreTemplate.Infrastructure.EntityFrameworkCore.Specifications.Evaluatiors
{
    public class SearchEvaluator : IEvaluator
    {
        private SearchEvaluator() { }

        public static SearchEvaluator Instance { get; } = new SearchEvaluator();

        public bool IsCriteriaEvaluator { get; } = true;

        public IQueryable<T> GetQuery<T>(IQueryable<T> query, ISpecification<T> specification) where T : class
        {
            foreach (var searchCriteria in specification.SearchCriterias.GroupBy(x => x.SearchGroup))
            {
                var criterias = searchCriteria.Select(x => (x.Selector, x.SearchTerm));
                query = query.Search(criterias);
            }

            return query;
        }
    }
}