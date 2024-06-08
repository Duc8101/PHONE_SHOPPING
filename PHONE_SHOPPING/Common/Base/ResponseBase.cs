using System.Net;

namespace Common.Base
{
    public class ResponseBase<T>
    {
        public int Code { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }

        public ResponseBase()
        {

        }
        public ResponseBase(T data, string message, int code)
        {
            Data = data;
            Message = message;
            Code = code;
        }

        public ResponseBase(T data, string message)
        {
            Data = data;
            Message = message;
            Code = (int)HttpStatusCode.OK;
        }

        public ResponseBase(string message, int code)
        {
            Message = message;
            Code = code;
        }

    }
}
