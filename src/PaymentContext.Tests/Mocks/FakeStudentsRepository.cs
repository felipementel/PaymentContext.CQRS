using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Repositories;

namespace PaymentContext.Tests.Mocks
{
    public class FakeStudentRepository : IStudentRepository
    {
        public void CreateSubscription(Student student)
        {

        }

        public bool DocumentExists(string document)
        {
            if (document == "99999999999")
                return true;
            else
                return false;
        }

        public bool EmailExists(string email)
        {
            if (email == "felipe@cqrs.com")
                return true;
            else
                return false;

        }
    }
}