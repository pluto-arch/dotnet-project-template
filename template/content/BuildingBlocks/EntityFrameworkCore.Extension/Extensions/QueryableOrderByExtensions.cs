using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace System.Linq
{

    /// <summary>
    /// 排序扩展
    /// </summary>
    public static class QueryableOrderByExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, string propertyName)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName);
        }

        public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, string propertyName)
        {
            return QueryableHelper<T>.OrderByDescending(queryable, propertyName);
        }


        private static class QueryableHelper<T>
        {
            private static readonly ConcurrentDictionary<string, LambdaExpression> cached = new();

            public static IQueryable<T> OrderBy(IQueryable<T> queryable, string propertyName)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return Queryable.OrderBy(queryable, keySelector);
            }

            public static IQueryable<T> OrderByDescending(IQueryable<T> queryable, string propertyName)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);
                return Queryable.OrderByDescending(queryable, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(string propertyName)
            {
                if (cached.ContainsKey(propertyName))
                {
                    return cached[propertyName];
                }
                var param = Expression.Parameter(typeof(T));
                var body = Expression.Property(param, propertyName);
                var keySelector = Expression.Lambda(body, param);
                cached[propertyName] = keySelector;
                return keySelector;
            }
        }
    }
}
