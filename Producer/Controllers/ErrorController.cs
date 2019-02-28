using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Producer.Models;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        private readonly ErrorModel _model;

        public ErrorController(ErrorModel model)
        {
            _model = model;
        }

        [HttpGet("{errorType}")]
        public ErrorModel Get([Required] ErrorType errorType)
        {
            _model.Type = errorType;
            return _model;
        }

               
    }
}