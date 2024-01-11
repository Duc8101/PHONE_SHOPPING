using System.Net;

namespace DataAccess.DTO
{
    public class ResponseDTO<T>
    {
        public int Code { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }

        public ResponseDTO()
        {

        }
        public ResponseDTO(T data, string message, int code)
        {
            Data = data;
            Message = message;
            Code = code;
        }

        public ResponseDTO(T data, string message)
        {
            Data = data;
            Message = message;
            Code = (int)HttpStatusCode.OK;
        }

    }
}
