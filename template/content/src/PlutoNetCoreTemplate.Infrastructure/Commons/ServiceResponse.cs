namespace Microsoft.AspNetCore.Mvc
{

    public class ServiceResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// 成功 - 空返回值
        /// </summary>
        /// <returns></returns>
        public static ServiceResponse Success()=>new () { Code = 200, Message = "执行成功" };


        /// <summary>
        /// 业务错误
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceResponse Error(string message)=>new() { Code = -100, Message = message};

    }

    /// <summary>
    /// 接口返回类型
    /// </summary>
    public class ServiceResponse<T> : ServiceResponse
    {
        /// <summary>
        /// 数据对象
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 执行成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResponse<T> Success(T data)=> new () { Code = 200, Message = "执行成功", Data = data };

        /// <summary>
        /// 业务错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResponse<T> Error(string message, T data=default)=>new() { Code = -100, Message = message, Data = data};

        /// <summary>
        /// 程序错误
        /// </summary>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResponse<T> Fatal(string message, T data = default) => new() { Code = 500, Message = message,Data = data};

        /// <summary>
        /// 数据验证错误
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ServiceResponse<T> ValidateFailure(T data = default)=> new() { Code = 400, Message = "无效的请求", Data = data };
    }
}