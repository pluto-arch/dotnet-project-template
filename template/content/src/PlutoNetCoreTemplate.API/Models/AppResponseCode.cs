namespace PlutoNetCoreTemplate.API.Models
{
	/// <summary>
	/// 响应码集合
	/// </summary>
	public static class AppResponseCode
	{
		
		/// <summary>
		/// 成功
		/// </summary>
		public const string Success = "00000";
		/// <summary>
		/// 失败
		/// </summary>
		public const string Error = "90000";
		
		#region 数据验证
		/// <summary>
		/// 无效参数
		/// </summary>
		public const string InvalidParameter = "A0010";
		

		#endregion

		#region 资源信息
		/// <summary>
		/// 资源异常
		/// </summary>
		public const string ResourceException = "B0010";
		

		#endregion
	}
}