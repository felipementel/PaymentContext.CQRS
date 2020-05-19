using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{
    [TestClass]
    public class SubscriptionHandlerTests
    {
        //Red, Grean, Refactor
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {
            var Handler = new SubscriptionHandler(
                new FakeStudentRepository(),
                new FakeEmailService());

            var command = new CreateBoletoSubscriptionCommand();
            command.FirstName = "Felipe";
            command.LastName = "Augusto";
            command.Document = "99999999999";
            command.Email = "felipe2@cqrs.com";

            command.BarCode = "25233453234";
            command.BoletoNumber = "52532453245";

            command.PaymentNumber = "2532456546464";
            command.PaidDate = DateTime.Now;
            command.ExpireDate = DateTime.Now.AddMonths(1);
            command.Total = 10;
            command.TotalPaid = 10;
            command.Payer = "Felipe Corp";
            command.PayerDocument = "1235346453";
            command.PayerDocumentType = EDocumentType.CPF;
            command.PayerEmail = "felipeboss@cqrs.com";

            command.Street = "rua";
            command.Number = "999";
            command.Neighborhood = "Bairro Feliz";
            command.City = "Rio de Janeiro";
            command.State = "RJ";
            command.Country = "BR";
            command.ZipCode = "21456984";

            Handler.Handle(command);

            Assert.AreEqual(true, Handler.Invalid);

        }
    }
}