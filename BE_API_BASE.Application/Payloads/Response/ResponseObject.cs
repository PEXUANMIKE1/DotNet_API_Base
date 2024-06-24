using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Payloads.Response
{
    public class ResponseObject<T>
    {
        public int Status {  get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
        public ResponseObject() { }
        public ResponseObject(int status, string message, T? data)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
