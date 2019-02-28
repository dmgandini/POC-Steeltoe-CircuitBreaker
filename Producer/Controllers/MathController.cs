using Microsoft.AspNetCore.Mvc;
using Producer.Models;
using Producer.Services;

namespace Producer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MathController : ControllerBase
    {
        private readonly IBasicMathOperationService _basicMathOperationService;
        private readonly ErrorModel _error;

        public MathController(IBasicMathOperationService basicMathOperationService, ErrorModel error)
        {
            _basicMathOperationService = basicMathOperationService;
            _error = error;
        }

        // GET: api/Math
        [HttpGet()]
        public ObjectResult Get()
        {
            if (_error.Execute(out ObjectResult erro))
                return erro;

            return Ok(new OperationModel
            {
                FirstTerm = 1,
                SecondTerm = 1,
                Type = OperationType.Addition,
                Result = 2
            });
        }

        // GET: api/Math
        [HttpGet("{first}/{second}/{type}")]
        public ObjectResult Get(decimal first, decimal second, OperationType type)
        {
            if (_error.Execute(out ObjectResult erro))
                return erro;

            return Ok(_basicMathOperationService.Exec(
                new OperationModel
                {
                    FirstTerm = first,
                    SecondTerm = second,
                    Type = type
                }));
        }

        // POST: api/Math
        [HttpPost]
        public ObjectResult Post([FromBody] OperationModel model)
        {
            if (_error.Execute(out ObjectResult erro))
                return erro;

            return Ok(_basicMathOperationService.Exec(model));
        }
    }
}
