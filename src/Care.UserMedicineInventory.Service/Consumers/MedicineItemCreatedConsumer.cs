using System.Threading.Tasks;
using Care.Common;
using Care.MedicineInventory.Contracts;
using MassTransit;
using Care.UserMedicineInventory.Service.Entities;


namespace Care.UserMedicineInventory.Service.Consumers
{
    public class MedicineItemCreatedConsumer : IConsumer<MedicineCreated>
    {
        private readonly IRepository<MedicineItem> repository;

        public MedicineItemCreatedConsumer(IRepository<MedicineItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MedicineCreated> context)
        {
            var message = context.Message;

            var medicine = await repository.GetAsync(message.Id);

            if (medicine != null)
            {
                return;
            }

            medicine = new MedicineItem
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description
            };

            await repository.CreateAsync(medicine);
        }
    }
}