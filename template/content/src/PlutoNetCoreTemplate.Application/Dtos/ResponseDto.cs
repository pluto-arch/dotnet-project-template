namespace PlutoNetCoreTemplate.Application.Dtos
{
    /// <summary>
    /// 响应结构
    /// </summary>
    public struct ResponseDto<T>
    {
        public string ResponseCode { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }


        public static ResponseDto<T> Success(T data)
        {
            return new ResponseDto<T>{ResponseCode = "SUCCESSED", Data = data};
        }


        public static  ResponseDto<T> SuccessNone()
        {
            return new ResponseDto<T>{ResponseCode = "SUCCESSED"};
        }

        public static ResponseDto<T> Fail(string message)
        {
            return new ResponseDto<T>{ResponseCode = "FAILURE", Message = message};
        }



    }

}