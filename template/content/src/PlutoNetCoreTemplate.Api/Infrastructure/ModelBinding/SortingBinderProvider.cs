using Microsoft.AspNetCore.Mvc.ModelBinding;
using PlutoNetCoreTemplate.Application.AppServices.Generics;

namespace PlutoNetCoreTemplate.Api
{
    public class SortingBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            if (context.Metadata.ModelType == typeof(IEnumerable<SortingDescriptor>))
            {
                return new SortingModelBinder();
            }

            return null;
        }
    }
}