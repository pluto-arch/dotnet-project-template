using Microsoft.AspNetCore.Mvc.ModelBinding;

using PlutoNetCoreTemplate.Application.AppServices.Generics;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Api
{
    using Newtonsoft.Json;

    using System.Linq;

    public class SortingModelBinder : IModelBinder
    {

        private static readonly Dictionary<string, SortingOrder> SortingDirectionMap = new()
        {
            { "asc", SortingOrder.Ascending },
            { "desc", SortingOrder.Descending }
        };

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            bindingContext = bindingContext ?? throw new ArgumentNullException(nameof(bindingContext));

            string modelName = bindingContext.ModelName;

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            string value = valueProviderResult.FirstValue;

            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            var sorter = JsonConvert.DeserializeObject<IDictionary<string, string>>(value);

            if (sorter is not null)
            {
                var effectSorter = sorter.Where(item => SortingDirectionMap.ContainsKey(item.Value));
                var sorting = effectSorter.Select(item => new SortingDescriptor
                {
                    PropertyName = item.Key,
                    SortDirection = SortingDirectionMap[item.Value]
                });

                bindingContext.Result = ModelBindingResult.Success(sorting);
            }

            return Task.CompletedTask;
        }
    }
}