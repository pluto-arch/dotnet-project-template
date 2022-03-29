using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Mvc.Components.Layout
{

    public class NavbarProperty
    {
        public string ClassName { get; set; }
    }


    public class NavbarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string className)
        {
            await Task.Yield();
            return View(new NavbarProperty { ClassName = className });
        }
    }
}