using System.Net;

namespace DataAccess.DTO
{
    public class ResponseDTO
    {
        public int Code { get; set; }
        public string Message { get; set; } = null!;
        public object? Data { get; set; }

        public ResponseDTO()
        {

        }
        public ResponseDTO(object? data, string message, int code)
        {
            Data = data;
            Message = message;
            Code = code;
        }

        public ResponseDTO(object? data, string message)
        {
            Data = data;
            Message = message;
            Code = (int)HttpStatusCode.OK;
        }

    }
}
