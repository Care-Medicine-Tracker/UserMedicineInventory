using System;

namespace Care.UserMedicineInventory.Service.Dtos
{
    //DTO for assigning medicine to a user
    public record AssignMedicineDto(Guid UserId, Guid MedicineId, string MeasurementType, double DoseAmount, int IntakeHour, int IntakeMinute);

    //DTO for returning medicines assign to a user's inventory
    public record UserMedicineInventoryDto(Guid Id, Guid MedicineId, string MeasurementType, double DoseAmount, int IntakeHour, int IntakeMinute);

    //DTO for updating assigning medicine to a user
    public record UpdateAssignMedicineDto(Guid UserId, Guid MedicineId, string MeasurementType, double DoseAmount, int IntakeHour, int IntakeMinute);

}