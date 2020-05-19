using System;
using Flunt.Notifications;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Command;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
     Notifiable,
      IHandler<CreateBoletoSubscriptionCommand>,
      IHandler<CreatePaypalSubscriotionCommand>,
      IHandler<CreateCreditCardSubscriptionCommand>
    {
        private readonly IStudentRepository _repository;

        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar seu cadastro");
            }

            // verificar se o documento já esta cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("CreateBoletoSubscriptionCommand.Documento", "Este CPF jé esta em uso'");

            // verificar se o email ja esta cadastrado
            if (_repository.DocumentExists(command.Email))
                AddNotification("CreateBoletoSubscriptionCommand.Emai", "Este Email jé esta em uso'");

            //gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City,
            command.State, command.Country, command.ZipCode);

            //gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode,
                command.BoletoNumber,
                command.PaidDate,
                command.ExpireDate,
                command.TotalPaid,
                command.TotalPaid,
                new Document(command.PayerDocument, command.PayerDocumentType),
                command.Payer,
                address,
                email);

            //gerar as entidades - Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //agrupar as validacoes
            AddNotifications(name, document, email, address, student, subscription, payment);

            //checar as notificacoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //salvar informacoes
            _repository.CreateSubscription(student);

            // enviar email de boas vindas
            _emailService.Send(student.Name.FirstName, student.Email.Address, "Bem vindo", "Sua assinatura foi criada");
            //retornoar informacoes

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePaypalSubscriotionCommand command)
        {
            command.Validate();

            if (command.Invalid)
            {
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar seu cadastro");
            }

            // verificar se o documento já esta cadastrado
            if (_repository.DocumentExists(command.Document))
                AddNotification("CreateBoletoSubscriptionCommand.Documento", "Este CPF jé esta em uso'");

            // verificar se o email ja esta cadastrado
            if (_repository.DocumentExists(command.Email))
                AddNotification("CreateBoletoSubscriptionCommand.Emai", "Este Email jé esta em uso'");

            //gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
            var document = new Document(command.Document, EDocumentType.CPF);
            var email = new Email(command.Email);
            var address = new Address(command.Street, command.Number, command.Neighborhood, command.City,
            command.State, command.Country, command.ZipCode);

            //gerar as entidades
            var student = new Student(name, document, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,
                command.PaidDate,
                command.ExpireDate,
                command.TotalPaid,
                command.TotalPaid,
                new Document(command.PayerDocument, command.PayerDocumentType),
                command.Payer,
                address,
                email);

            //gerar as entidades - Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //agrupar as validacoes
            AddNotifications(name, document, email, address, student, subscription, payment);

            //checar as notificacoes
            if (Invalid)
                return new CommandResult(false, "Não foi possível realizar sua assinatura");

            //salvar informacoes
            _repository.CreateSubscription(student);

            // enviar email de boas vindas
            _emailService.Send(student.Name.FirstName, student.Email.Address, "Bem vindo", "Sua assinatura foi criada");
            //retornoar informacoes

            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreateCreditCardSubscriptionCommand command)
        {
            throw new NotImplementedException();
        }
    }
}