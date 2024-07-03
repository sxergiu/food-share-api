using FoodShareNet.Application.Interfaces;
using FoodShareNet.Application.Services;
using Moq;

namespace FoodShareNet.UnitTests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private Mock<IFoodShareDbContext> _contextMock;
        private IOrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _contextMock = new Mock<IFoodShareDbContext>();
            _orderService = new OrderService(_contextMock.Object);            
        }


        [TearDown]
        public void TearDown()
        {
            _contextMock = null;
        }
        

        [Test]
        public void TestUpdateOrder()
        {
            Assert.Pass();
        }
    }
}