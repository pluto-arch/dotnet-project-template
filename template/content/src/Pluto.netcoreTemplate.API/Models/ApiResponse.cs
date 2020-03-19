using System;
using Microsoft.AspNetCore.Mvc;


namespace Pluto.netcoreTemplate.API.Models
{

    /// <summary>
    /// 接口response
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>

        public ApiResponse(bool successed, string msg)
        {
            Successed = successed;
            Msg = msg;
        }

        /// <summary>
        /// 接口调用是否成功
        /// </summary>
        public bool Successed { get; set; }

        /// <summary>
        /// 失败的提示信息
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// 默认成功
        /// </summary>
        /// <returns></returns>
        public static ApiResponse DefaultSuccess(string msg=null) => new ApiResponse(true, msg);

        /// <summary>
        /// 默认失败
        /// </summary>
        /// <returns></returns>
        public static ApiResponse DefaultFail(string msg) => new ApiResponse(false, msg);


        /// <summary>
        /// 带数据的成功
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponse Success<T>(T data) => new ApiResponse<T>(true, string.Empty,data);

    }




    /// <summary>
    /// 接口统一返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>:ApiResponse
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="success"></param>
        /// <param name="msg"></param>
        public ApiResponse(bool success, string msg,T data) : base(success, msg)
        {
            this.Data = data;
        }

        /// <summary>
        /// 数据域
        /// </summary>
        public T Data { get; set; }

    }
}