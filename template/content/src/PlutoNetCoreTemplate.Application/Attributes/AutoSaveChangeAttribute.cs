using System;

namespace PlutoNetCoreTemplate.Application.Attributes
{
	/// <summary>
	/// 是否启用自动SaveChange
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class AutoSaveChangeAttribute: Attribute
	{ }
}