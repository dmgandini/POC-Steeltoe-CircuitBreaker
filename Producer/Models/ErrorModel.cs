using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Producer.Models
{
    public class ErrorModel
    {

        public ErrorType Type { get; set; } = ErrorType.None;
        
        public bool Execute(out ObjectResult error)
        {
            switch (Type)
            {
                case ErrorType.E500:
                    return CreateError(500, out error);

                case ErrorType.E400:
                    return CreateError(400, out error);

                case ErrorType.E404:
                    return CreateError(404, out error);

                case ErrorType.T1s:
                    return Sleep(1000, out error);

                case ErrorType.T1m:
                    return Sleep(1000 * 60, out error);

                case ErrorType.T1h:
                    return Sleep(1000 * 60* 60, out error);

                case ErrorType.None:         
                default:
                    error = null;
                    return false;
            }
        }

        private static bool Sleep(int millisecondsTimeout, out ObjectResult error)
        {
            error = null;
            Thread.Sleep(millisecondsTimeout);
            return false;
        }

        private bool CreateError(int statusCode, out ObjectResult error)
        {
            error = new ObjectResult($"{Type} was injected")
            {
                StatusCode = statusCode,
            };
            return true;
        }
    }

    public enum ErrorType
    {
        E500,
        E400,
        E404,
        T1s,
        T1m,
        T1h,
        None
    }
}
