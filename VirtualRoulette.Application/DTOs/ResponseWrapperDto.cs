using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualRoulette.Application.DTOs
{
    public class ResponseWrapperDto
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

        public ResponseWrapperDto(int statusCode, string? message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public static ResponseWrapperDto Success(string? message, int statusCode = 200)
        {
            return new ResponseWrapperDto(statusCode, message);
        }

        public static ResponseWrapperDto Failure(string? message, int statusCode = 500)
        {
            return new ResponseWrapperDto(statusCode, message);
        }
    }

    public class ResponseWrapperDto<T> : ResponseWrapperDto
    {
        public T? Data { get; set; }

        private ResponseWrapperDto(int statusCode, T? data) : base(statusCode, null)
        {
            Data = data;
        }
        public static ResponseWrapperDto<T> Success(T data, int statusCode = 200)
        {
            return new ResponseWrapperDto<T>(statusCode, data);
        }

        public static ResponseWrapperDto<T> Failure(T data, int statusCode = 500)
        {
            return new ResponseWrapperDto<T>(statusCode, data);
        }
    }
}
