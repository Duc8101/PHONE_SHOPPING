﻿using System.Net;

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

    public class ResponseBase : ResponseBase<object>
    {
        public ResponseBase(string message, int code) : base(message, code)
        {
            
        }

        public ResponseBase(object data, string message, int code) : base(data, message, code)
        {

        }

        public ResponseBase(object data, string message) : base(data, message)
        {
         
        }
    }
}
