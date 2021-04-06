using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace TestWebApi.Dto
{

    public class ResponseBase
    {
        public ErrorModel Error { get; set; }

        public static ResponseBase CreateErrorResponse(string message) => new ResponseBase { Error = new ErrorModel { Message = message } };
    }

    public class ResponseBase<T> : ResponseBase
    {
        public T Data { get; set; }
    }

    public class ErrorModel
    {
        public string Message { get; set; }
    }
}
