using System;

namespace PlutoNetCoreTemplate.Infrastructure.Commons
{



	/// <summary>
	/// 接口返回类型
	/// </summary>
	public record ActionResponse<T> where T:class,new()
	{
		
	}
}