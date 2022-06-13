using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Square.Connect.Model;

namespace RetailApp.Data
{
    public class ApiResponse
    {
        public List<ApiError> Errors { get; set; }
        public bool IsSuccess { get; set; }

        public ApiResponse() { }

        public ApiResponse(List<ApiError> errors, bool success)
        {
            Errors = errors;
            IsSuccess = success;
        }

        public ApiResponse(List<Error> errors, bool success)
        {
            IsSuccess = success;

            if(errors != null && errors.Count>0)
                Errors = errors.ConvertAll(new Converter<Error, ApiError>(ApiError.SquareErrorConverter));
        }

        public ApiResponse(string errorMessage, bool success)
        {
            IsSuccess = success;
            Errors = new List<ApiError>() { new ApiError(errorMessage) };
        }
    }
}
