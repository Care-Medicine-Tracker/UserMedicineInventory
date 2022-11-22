using System.Threading.Tasks;
using Care.Common;
using Care.MedicineInventory.Contracts;
using MassTransit;
using Care.UserMedicineInventory.Service.Models;


namespace Care.UserMedicineInventory.Service.Consumers
{
    public class MedicineItemUpdatedConsumer : IConsumer<MedicineUpdated>
    {
        private readonly IRepository<MedicineItem> repository;

        public MedicineItemUpdatedConsumer(IRepository<MedicineItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MedicineUpdated> context)
        {
            var message = context.Message;

            var medicine = await repository.GetAsync(message.Id);

            if (medicine == null)
            {
                medicine = new MedicineItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description
                };

                await repository.CreateAsync(medicine);
            }
            else
            {
                medicine.Name = message.Name;
                medicine.Description = message.Description;

                await repository.UpdateAsync(medicine);
            }
        }
    }
}