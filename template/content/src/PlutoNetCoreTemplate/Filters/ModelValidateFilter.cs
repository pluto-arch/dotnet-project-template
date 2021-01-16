using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using PlutoNetCoreTemplate.Application.Dtos;
using PlutoNetCoreTemplate.Infrastructure;
using PlutoNetCoreTemplate.Infrastructure.Commons;

namespace PlutoNetCoreTemplate.Filters
{
	/// <summary>
	/// model 验证过滤器
	/// </summary>
	public class ModelValidateFilter : IActionFilter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuted(ActionExecutedContext context)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ModelState.IsValid)
			{
                var result = context.ModelState.Keys
                                    .SelectMany(key => context.ModelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                                    .ToList();
                context.HttpContext.Response.StatusCode=(int)HttpStatusCode.BadRequest;
				context.Result = new JsonResult(ServiceResponse<List<ValidationError>>.ValidateFailure(result));
			}
		}

    }
}