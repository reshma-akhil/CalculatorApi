using CalculatorApi.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CalculatorApi.Test
{
    [TestClass]
    public class CalculatorTest
    {
        CalculatorController controller;
        Mock<ILogger<CalculatorController>> mockLogger;
        public CalculatorTest() 
        {
            mockLogger = new Mock<ILogger<CalculatorController>>();
            controller = new CalculatorController(mockLogger.Object);
        }

        [TestMethod]
        public void AddTest()
        {
            var expected = 7;
            var actual = controller.Add(2, 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SubstractTest()
        {
            var expected = -3;
            var actual = controller.Substract(2, 5);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void MultiplyTest()
        {
            var expected = 10;
            var actual = controller.Multiply(2, 5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DivideTest()
        {
            var expected = 2.5M;
            var actual = controller.Divide(5, 2);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DisionByZeroTest()
        {
            var expected = "Divide by zero error";
            try
            {
                var result = controller.Divide(2.5M, 0);
            }
            catch(Exception ex)
            {
                Assert.AreEqual(expected, ex.Message);
            }
            
        }
    }
}