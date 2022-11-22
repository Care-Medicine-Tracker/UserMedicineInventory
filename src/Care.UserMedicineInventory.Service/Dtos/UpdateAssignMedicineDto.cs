using System;

namespace Care.UserMedicineInventory.Service.Dtos
{
    //DTO for updating assigning medicine to a user
    public record UpdateAssignMedicineDto(
        Guid UserId,
        Guid MedicineId,
        string MeasurementType,
        double DoseAmount,
        int IntakeHour,
        int IntakeMinute
        );
}