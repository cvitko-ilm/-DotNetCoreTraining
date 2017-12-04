using System;
using WebApp2_0.Services;
using Xunit;

namespace WebApp2Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var service = new DataService();

            var result = service.GetName();

            Assert.Same("Chris Vitko", result);
        }
    }
}
