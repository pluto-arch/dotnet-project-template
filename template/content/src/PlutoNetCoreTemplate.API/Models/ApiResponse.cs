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
        /// 初始化ApiResponse的实例
        /// </summary>
        /// <param name="code"><see cref="AppResponseCode"/></param>
        /// <param name="msg"></param>
        public ApiResponse(
            string code,
            string msg)
        {
            Msg = msg;
            Code = code;
        }

        /// <summary>
        /// 响应码<see cref="AppResponseCode"/>
        /// </summary>
        public string Code { get; set; } 

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Msg { get; set; }


        /// <summary>
        /// 默认成功
        /// </summary>
        /// <returns></returns>
        public static ApiResponse DefaultSuccess(string msg="执行成功") => new ApiResponse(AppResponseCode.Success,msg);

        /// <summary>
        /// 默认失败
        /// </summary>
        /// <returns></returns>
        public static ApiResponse DefaultFail(string msg="执行失败") => new ApiResponse(AppResponseCode.Error,msg);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"><see cref="AppResponseCode"/></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static ApiResponse Fail(string code,string msg="执行失败") => new ApiResponse(code,msg);
    }




    /// <summary>
    /// 接口统一返回值
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T>:ApiResponse
    {
        /// <summary>
        /// 初始化<see>
        ///     <cref>ApiResponse:T</cref>
        /// </see>的实例
        /// </summary>
        /// <param name="code"><see cref="AppResponseCode"/></param>
        /// <param name="msg"></param>
        /// <param name="data"></param>
        public ApiResponse(
           string code=AppResponseCode.Success,
           string msg="",
           T data=default) : base(code,msg)
        {
            this.Data = data;
        }

        /// <summary>
        /// 数据域
        /// </summary>
        public T Data { get; set; }
        
        /// <summary>
        /// 带数据的成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ApiResponse<T> Success(T data) => new ApiResponse<T>(AppResponseCode.Success, "执行成功",data);

    }
}