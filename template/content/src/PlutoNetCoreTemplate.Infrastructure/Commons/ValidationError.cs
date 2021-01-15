using System;

namespace PlutoNetCoreTemplate.Infrastructure.Commons
{
    /// <summary>
    /// 验证错误
    /// </summary>
    public class ValidationError
    {
        public string Field { get; }
        public string Message { get; }
        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }
    }
}