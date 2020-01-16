using System;


namespace Pluto.netcoreTemplate.API.Models
{
    /// <summary>
    /// 接口统一返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> where T:class,new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ApiResponse(bool success,string msg="",T data=null)
        {
            IsSuccess = success;
            Message = msg;
            Data = data;
        }

        /// <summary>
        /// 是否成功标识
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// 数据域
        /// </summary>
        public T Data { get; set; }

        public static ApiResponse<T> Fail(string msg)
        {
            return new ApiResponse<T>(false, msg);
        }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>(true, "", data);
        }
    }
}