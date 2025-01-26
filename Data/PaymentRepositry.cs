using FinalProjAPI.Dto;
using FinalProjAPI.Migrations;
using FinalProjAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinalProjAPI.Data
{
    public class PaymentRepositry : IPaymentRepository
    {
        private readonly DataContextEF dataContextEF;

        public PaymentRepositry(DbContextOptions<DataContextEF> options, IConfiguration configuration)
        {
            dataContextEF = new DataContextEF(options, configuration);
        }
        public bool SaveChanges()
        {
            return dataContextEF.SaveChanges() > 0;
        }
        public bool RemoveEntity<T>(T entityToRemove)
        {
            if (entityToRemove != null)
            {
                dataContextEF.Remove(entityToRemove);
                return true;
            }
            return false;
        }


        public List<PaymentDto> GetPaymentCards()
        {
            return dataContextEF.Payments.Select(x => new PaymentDto()
            {
                cardHolder = x.cardHolder,
                CardNumber = x.CardNumber,
                cvv = x.cvv,
                expirationDate = x.expirationDate,
            }).ToList();
        }

        public async Task AddPaymentCardAsync(Payments payments)
        {
            await dataContextEF.Payments.AddAsync(payments);
        }

        
    }

}
