using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.ValueObjects;

namespace PaymentContext.Tests
{
    [TestClass]
    public class StudentTests
    {
        private readonly Name _name;
        private readonly Document _document;
        private readonly Address _address;
        private readonly Email _email;
        private readonly Student _student;
        private readonly Subscription _subscription;

        public StudentTests()
        {
            _name = new Name("Felipe", "Augusto");
            _document = new Document("12312312312", EDocumentType.CPF);
            _email = new Email("felipe@hotmail.com");
            _address = new Address("Rua 1", "1234", "Bairro legal", "Rio de Janeiro", "RJ", "BR", "123");
            _student = new Student(_name, _document, _email);
            _subscription = new Subscription(null);
        }

        [TestMethod]
        public void ShoulReturnErroWhenHadActiveSubscription()
        {
            var payment = new PayPalPayment("12346", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Nome do pagador", _address, _email);

            _subscription.AddPayment(payment);

            _student.AddSubscription(_subscription);
            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShoulReturnErroWhenSubscriptionHasNoPayment()
        {
            _student.AddSubscription(_subscription);
            Assert.IsTrue(_student.Invalid);
        }

        [TestMethod]
        public void ShoulReturnSuccessWhenHadNoActiveSubscription()
        {
             var payment = new PayPalPayment("1234678", DateTime.Now, DateTime.Now.AddDays(5), 10, 10, _document, "Nome do pagador", _address, _email);

            _subscription.AddPayment(payment);

            _student.AddSubscription(_subscription);

            Assert.IsTrue(_student.Invalid);
        }
    }
}