using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square.Connect.Model;

namespace RetailApp.Data
{
    public class ApiError
    {
        public Error.CategoryEnum? Category { get; set; }
        public Error.CodeEnum? Code { get; set; }
        public System.Net.HttpStatusCode HttpCode { get; set; }
        public string Message { get; set; }
        public string ErrorField { get; set; }

        public ApiError(System.Net.HttpStatusCode httpCode, string message)
        {
            HttpCode = httpCode;
            Message = message;
        }

        public ApiError(string message)
        {
            Message = message;
        }

        public ApiError(Error squareError)
        {
            Category = squareError.Category;
            Code = squareError.Code;
            Message = squareError.Detail;
            ErrorField = squareError.Field;
        }

        public static ApiError SquareErrorConverter(Error err)
        {
            return new ApiError(err);
        }
    }
}
