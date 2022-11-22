using System;

namespace Care.UserMedicineInventory.Service.Dtos
{
    //DTO for retrieving medicine items from the MedicineInventory service
    public record MedicineDto(
        Guid Id,
        string Name,
        string Description
    );
}