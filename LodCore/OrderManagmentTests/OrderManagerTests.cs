using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using OrderManagement.Infrastructure;
using OrderManagement.Domain;
using OrderManagement.Domain.Events;

namespace OrderManagmentTests
{
    [TestClass]
    public class OrderManagerTests
    {
        [TestInitialize]
        public void Setup()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _mailer = new Mock<IMailer>();
            _eventSink = new Mock<IEventSink>();

            _orderManagment = new OrderManagment(_orderRepository.Object, _eventSink.Object, _mailer.Object);
        }

        [TestMethod]
        public void AddingOrderAddsOrder()
        {
            //arrange
            var orderMock = new Mock<Order>();
            orderMock.Setup(mock => mock.Id).Returns(42);
            orderMock.Setup(mock => mock.Header).Returns("TestHead");
            orderMock.Setup(mock => mock.Description).Returns("TestDesc");


            _eventSink.Setup(
                mock =>
                    mock.ConsumeEvent(new OrderPlaced(orderMock.Object.Id, orderMock.Object.Header,
                        orderMock.Object.Description)));

            _mailer.Setup(mock => mock.SendNewOrderEmail(orderMock.Object));

            //act
            _orderManagment.AddOrder(orderMock.Object);

            //assert
            _orderRepository.Verify(mock => mock.SaveOrder(orderMock.Object), Times.Once);
        }

        [TestMethod]
        public void GetOrderReturnsOrders()
        {
            //arrange
            var orderMock = new Mock<Order>();
            orderMock.Setup(mock => mock.Id).Returns(42);
            orderMock.Setup(mock => mock.Header).Returns("TestHead");
            orderMock.Setup(mock => mock.Description).Returns("TestDesc");


            _eventSink.Setup(
                mock =>
                    mock.ConsumeEvent(new OrderPlaced(orderMock.Object.Id, orderMock.Object.Header,
                        orderMock.Object.Description)));

            _mailer.Setup(mock => mock.SendNewOrderEmail(orderMock.Object));

            _orderRepository.Setup(mock => mock.GetOrder(orderMock.Object.Id)).Returns(orderMock.Object);

            //act
            _orderManagment.AddOrder(orderMock.Object);
            _orderManagment.GetOrder(orderMock.Object.Id);

            //assert
            _orderRepository.Verify(mock => mock.GetOrder(orderMock.Object.Id), Times.Once);
        }

        private Mock<IOrderRepository> _orderRepository;
        private Mock<IMailer>_mailer;
        private Mock<IEventSink> _eventSink;
        private OrderManagment _orderManagment;
    }
}
