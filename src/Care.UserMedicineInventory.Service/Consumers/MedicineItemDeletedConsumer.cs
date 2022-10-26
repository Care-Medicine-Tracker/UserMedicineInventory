using System.Threading.Tasks;
using Care.Common;
using Care.MedicineInventory.Contracts;
using MassTransit;
using Care.UserMedicineInventory.Service.Entities;


namespace Care.UserMedicineInventory.Service.Consumers
{
    public class MedicineItemDeletedConsumer : IConsumer<MedicineDeleted>
    {
        private readonly IRepository<MedicineItem> repository;

        public MedicineItemDeletedConsumer(IRepository<MedicineItem> repository)
        {
            this.repository = repository;
        }

        public async Task Consume(ConsumeContext<MedicineDeleted> context)
        {
            var message = context.Message;

            var medicine = await repository.GetAsync(message.Id);

            if (medicine == null)
            {
                return;
            }
            else
            {
                await repository.RemoveAsync(message.Id);
            }
        }
    }
}