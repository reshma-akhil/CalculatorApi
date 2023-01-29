using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalculatorApi.Controllers
{
    [Route("api/[controller]")]
    [AuthFilterAttribute]
    [ApiController]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger logger;

        public CalculatorController(ILogger<CalculatorController> logger) 
        {
            this.logger = logger;
        }
        [HttpGet("Add")]
        public decimal Add( decimal value1, decimal value2)
        {
            try
            {
                return value1 + value2;
            }
            catch(Exception ex)
            {
                logger.LogError(ex.ToString());
                return 0;
            }
        }
        [HttpGet("Substract")]
        public decimal Substract(decimal value1, decimal value2)
        {
            return value1 - value2;
        }
        [HttpGet("Multiply")]
        public decimal Multiply(decimal value1, decimal value2)
        {
            return value1 * value2;
        }
        [HttpGet("Divide")]
        public decimal Divide(decimal value1, decimal value2)
        {
            try
            {
                return value1 / value2;
            }
            catch (DivideByZeroException ex)
            {
                logger.LogError(ex.ToString());
                throw new Exception("Divide by zero error", null);
            }
        }
    }
}
