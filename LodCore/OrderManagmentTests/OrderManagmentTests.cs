using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NotificationService;
using OrderManagement.Domain;
using OrderManagement.Domain.Events;
using OrderManagement.Infrastructure;

namespace OrderManagmentTests
{
    // Не удаётся загрузить символы - причина ошибки

    [TestClass]
    public class OrderManagmentTests
    {
        private Mock<IEventSink> _eventSink;
        private OrderManagment _orderManagment;

        private Mock<IOrderRepository> _orderRepository;

        [TestInitialize]
        public void Setup()
        {
            _orderRepository = new Mock<IOrderRepository>();
            _eventSink = new Mock<IEventSink>();

            _orderManagment = new OrderManagment(_orderRepository.Object, _eventSink.Object);
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
                    mock.ConsumeEvent(new OrderPlaced(orderMock.Object.Id)));

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
                    mock.ConsumeEvent(new OrderPlaced(orderMock.Object.Id)));

            _orderRepository.Setup(mock => mock.GetOrder(orderMock.Object.Id)).Returns(orderMock.Object);

            //act
            _orderManagment.AddOrder(orderMock.Object);
            _orderManagment.GetOrder(orderMock.Object.Id);

            //assert
            _orderRepository.Verify(mock => mock.GetOrder(orderMock.Object.Id), Times.Once);
        }

        [TestMethod]
        public void AddingProjectThrowNotification()
        {
            //arrange
            var orderMock = new Mock<Order>();
            orderMock.Setup(mock => mock.Id).Returns(42);
            orderMock.Setup(mock => mock.Header).Returns("TestHead");
            orderMock.Setup(mock => mock.Description).Returns("TestDesc");

            _eventSink.Setup(
                mock =>
                    mock.ConsumeEvent(new OrderPlaced(orderMock.Object.Id)));

            //act
            _orderManagment.AddOrder(orderMock.Object);

            //assert
            _eventSink.Verify(mock => mock.ConsumeEvent(It.IsAny<OrderPlaced>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void UnsucceessfulyAddingOrderThrowsError()
        {
            //arrange
            Order nullOrder = null;

            _eventSink.Setup(
                mock =>
                    mock.ConsumeEvent(It.IsAny<OrderPlaced>()));

            //act
            _orderManagment.AddOrder(nullOrder);

            //assert
            _eventSink.Verify(mock => mock.ConsumeEvent(It.IsAny<OrderPlaced>()), Times.Never);
            Assert.Fail();
        }
    }
}