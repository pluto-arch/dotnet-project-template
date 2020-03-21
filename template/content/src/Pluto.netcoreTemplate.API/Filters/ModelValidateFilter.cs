using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Pluto.netcoreTemplate.API.Models;


namespace Pluto.netcoreTemplate.API.Filters
{
    /// <summary>
    /// model 验证过滤器
    /// </summary>
    public class ModelValidateFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context) { }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                StringBuilder builder = new StringBuilder();
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        builder.Append(error.ErrorMessage);
                        builder.Append("|");
                    }
                }
                context.Result = new JsonResult(ApiResponse.DefaultFail(builder.ToString().TrimEnd('|')));
            }
        }
    }
}