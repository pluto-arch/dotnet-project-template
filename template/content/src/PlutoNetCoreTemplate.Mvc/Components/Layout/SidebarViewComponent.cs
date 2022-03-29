using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Mvc.Components.Layout
{
    public class SidebarViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Yield();
            return View();
        }
    }
}