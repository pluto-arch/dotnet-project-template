using System;
using Microsoft.AspNetCore.Mvc;


namespace PlutoNetCoreTemplate.API.Models
{
	/// <summary>
	/// 接口response
	/// </summary>
	public class ApiResponse
	{
		/// <summary>
		/// 执行成功的返回值
		/// </summary>
		/// <param name="data"></param>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static object Success<T>(T data)
		{
			return new
			{
				Data=data
			};
		}
		
		/// <summary>
		/// 执行失败的返回值
		/// </summary>
		/// <param name="msg"></param>
		/// <returns></returns>
		public static object Error(string msg)
		{
			return new
			{
				Message=msg
			};
		}
	}
}