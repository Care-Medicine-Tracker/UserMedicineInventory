using System.Threading.Tasks;
using Care.Common;
using Care.MedicineInventory.Contracts;
using MassTransit;
using Care.UserMedicineInventory.Service.Models;


namespace Care.UserMedicineInventory.Service.Consumers
{
    public class MedicineItemDeletedConsumer : IConsumer<MedicineDeleted>
    {
        private readonly IRepository<MedicineItem> repository;
        private readonly IRepository<UserMedicineInventoryItem> userMedicineRepository;

        public MedicineItemDeletedConsumer(IRepository<MedicineItem> repository, IRepository<UserMedicineInventoryItem> userMedicineRepository)
        {
            this.repository = repository;
            this.userMedicineRepository = userMedicineRepository;
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


            if (medicine == null)
            {
                return;
            }
            else
            {
                var users = (await userMedicineRepository.GetAllAsync(userMedicine => userMedicine.MedicineId == medicine.Id));
                await userMedicineRepository.GetAllRemoveAsync(userMedicine => userMedicine.MedicineId == medicine.Id);
            }
        }
    }
}