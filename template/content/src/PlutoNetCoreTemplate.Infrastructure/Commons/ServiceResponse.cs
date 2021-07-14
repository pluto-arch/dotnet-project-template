namespace PlutoNetCoreTemplate.Infrastructure.Commons
{
    /// <summary>
    /// 接口返回类型
    /// </summary>
    public class ServiceResponse<T>
    {
        public string Code { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }


        public static ServiceResponse<T> Success(T data)
        {
            return new ServiceResponse<T> { Code = "SUCCESSED", Message = "执行成功", Data = data };
        }


        public static ServiceResponse<T> SuccessNone()
        {
            return new ServiceResponse<T> { Code = "SUCCESSED", Message = "执行成功" };
        }

        public static ServiceResponse<T> SuccessNone(T data, string code = "SUCCESSED")
        {
            return new ServiceResponse<T> { Code = code, Message = "执行成功", Data = data };
        }


        public static ServiceResponse<T> Failure(string message, string code = "FAILURE")
        {
            return new ServiceResponse<T> { Code = code, Message = message };
        }

        public static ServiceResponse<T> ValidateFailure(T data)
        {
            return new ServiceResponse<T> { Code = "FAILURE", Message = "无效的请求", Data = data };
        }
    }
}