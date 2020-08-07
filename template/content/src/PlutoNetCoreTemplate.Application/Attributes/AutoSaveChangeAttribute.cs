using System;

namespace PlutoNetCoreTemplate.Application.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AutoSaveChangeAttribute: Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Attribute" /> class.</summary>
		public AutoSaveChangeAttribute(bool isEnable=false)
		{
			IsEnable = isEnable;
		}
		public bool IsEnable { get; set; }
	}
}