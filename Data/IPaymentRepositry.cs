using FinalProjAPI.Dto;
using FinalProjAPI.Models;

namespace FinalProjAPI.Data
{
    public interface IPaymentRepository
    {
        public bool SaveChanges();
        public bool RemoveEntity<T>(T entityToRemove);
        public Task AddPaymentCardAsync(Payments payments);
        public List<PaymentDto> GetPaymentCards();

    }
}