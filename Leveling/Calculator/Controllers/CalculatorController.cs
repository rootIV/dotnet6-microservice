using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace Calculator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ILogger<CalculatorController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/calculator/{num1}/{num2}")]
        public IActionResult Get(string num1, string num2)
        {
            double _num1 = ConvertToDouble(num1);
            double _num2 = ConvertToDouble(num2);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Calculator in URL");
            sb.AppendLine($"Add Op: {Add(_num1, _num2)}");
            sb.AppendLine($"Sub Op: {Sub(_num1, _num2)}");
            sb.AppendLine($"Mult Op: {Mult(_num1, _num2)}");
            sb.AppendLine($"Div Op: {Div(_num1, _num2)}");
            sb.AppendLine($"Avg Op: {Avg(_num1, _num2)}");
            sb.AppendLine($"Root Op: {Root(_num1)}");
            sb.AppendLine($"Root Op: {Root(_num2)}");

            return Ok(sb.ToString());
        }

        private double ConvertToDouble(string num)
        {
            return double.Parse(num, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo);
        }
        private double Add(double num1, double num2)
        {
            return num1 + num2;
        }
        private double Sub(double num1, double num2)
        {
            return num1 - num2;
        }
        private double Mult(double num1, double num2)
        {
            return num1 * num2;
        }
        private double Div(double num1, double num2)
        {
            return num1 / num2;
        }
        private double Avg(double num1, double num2)
        {
            return (num1 + num2) / 2;
        }
        private double Root(double num1)
        {
            return Math.Sqrt(num1);
        }
    }
}
