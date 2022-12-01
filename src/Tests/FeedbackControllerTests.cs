using Care.UserMedicineInventory.Service.Controller;
using Care.UserMedicineInventory.Service.Dtos;
using Care.UserMedicineInventory.Service.Interfaces;
using Care.UserMedicineInventory.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace Tests.Controller;

public class FeedbackControllerTests
{
    private readonly Mock<IUserMedicineInventoryService> _mockService;
    
    public FeedbackControllerTests()
    {
        _mockService = new Mock<IUserMedicineInventoryService>();
    }
    
    // Get All Feedback by specific Patient - Happy Flow 
    [Fact]
    public void GetAsync_ReturnsAllMedicineByUser()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockService.Setup(service => service.GetAsync(testSessionGuid))
            .ReturnsAsync(GetPatientFeedbacksById(testSessionGuid));

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.GetAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<UserMedicineInventoryDto>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"), feedback!.Id);
        // Assert.Equal("Hello World", feedback!.Comment);
    }

    // All Feedback by specific Patient - Sad Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsNoMedicineByPatient_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B04DA");
        _mockService.Setup(service => service.GetAsync(testSessionGuid))
            .ReturnsAsync(new List<UserMedicineInventoryDto>());

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.GetAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<UserMedicineInventoryDto>>(actionResult.Value);
        Assert.Empty(returnValue);
    }
    
    // Get All Feedback by specific Patient - Happy Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsAllFeedbackByPatient()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");

        _mockService.Setup(service => service.GetMedicineByUsersAsync(testSessionGuid))
            .ReturnsAsync(GetPatientFeedbacksById(testSessionGuid));

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.GetMedicineByUsersAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>>>(result);
        var okResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<UserMedicineInventoryDto>>(okResult.Value);
        Assert.NotEmpty(returnValue);
        var feedback = returnValue.FirstOrDefault();
        Assert.Equal(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"), feedback!.Id);
        // Assert.Equal("Hello World", feedback!.Comment);
    }

    // All Feedback by specific Patient - Sad Flow 
    [Fact]
    public void GetPatientFeedbackById_ReturnsNoFeedbackByPatient_WhenNoFeedbackFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B04DA");
        _mockService.Setup(service => service.GetMedicineByUsersAsync(testSessionGuid))
            .ReturnsAsync(new List<UserMedicineInventoryDto>());

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.GetMedicineByUsersAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<ActionResult<IEnumerable<UserMedicineInventoryDto>>>>(result);
        var actionResult = Assert.IsType<OkObjectResult>(task.Result.Result);
        var returnValue = Assert.IsType<List<UserMedicineInventoryDto>>(actionResult.Value);
        Assert.Empty(returnValue);
    }

    // Create a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.PostAsync(It.IsAny<AssignMedicineDto>()))
            .ReturnsAsync(userMedicineInventoryItem(testSessionGuid));

        var controller = new MedicinesController(_mockService.Object);
        AssignMedicineDto assignMedicineDto = new AssignMedicineDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
            new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
            "Grams",
            5.0,
            3,
            30);

        // Act
        var result = controller.PostAsync(assignMedicineDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        Assert.IsType<OkResult>(task.Result);
    }
  
    // Create a Feedback for specific Patient and checks what occurs when a feedback can not be made- Sad Flow 
    [Fact]
    public void CreateFeedback_ReturnsNewlyCreatedFeedback_ReturnsBadRequest_WhenFeedbackCanNotBeMade()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.PostAsync(It.IsAny<AssignMedicineDto>()))
            .ReturnsAsync((UserMedicineInventoryItem)null!);

        var controller = new MedicinesController(_mockService.Object);

        AssignMedicineDto assignMedicineDto = new AssignMedicineDto(new Guid(), new Guid(), "", 0, 0, 0);

        // Act
        var result = controller.PostAsync(assignMedicineDto);

        // Assert
        var task = Assert.IsType<Task<ActionResult>>(result);
        var actionResult = task.Result;
        Assert.IsType<BadRequestResult>(actionResult);
    }

    // Updates a Feedback for specific Patient - Happy Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.PutAsync(testSessionGuid, It.IsAny<UpdateAssignMedicineDto>()))
            .ReturnsAsync(userMedicineInventoryItem(testSessionGuid));

        var controller = new MedicinesController(_mockService.Object);
        UpdateAssignMedicineDto updateAssignMedicineDto = new UpdateAssignMedicineDto(new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
            new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0AAA"),
            "Grams",
            5.0,
            3,
            30);

        // Act
        var result = controller.PutAsync(testSessionGuid, updateAssignMedicineDto);

        // Assert
        var task = Assert.IsType<Task<IActionResult>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }
    
    // Updates a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void UpdateFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017D");
        _mockService.Setup(service => service.PutAsync(testSessionGuid, It.IsAny<UpdateAssignMedicineDto>()))
            .ReturnsAsync((UserMedicineInventoryItem)null!);

        var controller = new MedicinesController(_mockService.Object);
        UpdateAssignMedicineDto updateAssignMedicineDto = new UpdateAssignMedicineDto(new Guid(), new Guid(), "", 0,0, 0);

        // Act
        var result = controller.PutAsync(testSessionGuid, updateAssignMedicineDto);

        // Assert
        var task = Assert.IsType<Task<IActionResult>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
    }

    // Deletes a Feedback for specific Patient and checks if a new feedback has been created- Happy Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNoContent()
    {
        // Arrange
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");
        _mockService.Setup(service => service.DeleteAsync(testSessionGuid))
            .ReturnsAsync(userMedicineInventoryItem(testSessionGuid));

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.DeleteAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<IActionResult?>>(result);
        Assert.IsType<NoContentResult>(task.Result);
    }

    
    // Delete a Feedback with wrong values and checks if the controller handles the error- Sad Flow 
    [Fact]
    public void DeleteFeedback_ReturnsNotFound_WhenFeedbackNotFound()
    {
        Guid testSessionGuid = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B017A");

        _mockService.Setup(service => service.DeleteAsync(testSessionGuid))
            .ReturnsAsync((UserMedicineInventoryItem)null!);

        var controller = new MedicinesController(_mockService.Object);

        // Act
        var result = controller.DeleteAsync(testSessionGuid);

        // Assert
        var task = Assert.IsType<Task<IActionResult?>>(result);
        Assert.IsType<NotFoundResult>(task.Result);
    }


    private IEnumerable<UserMedicineInventoryDto> GetPatientFeedbacksById(Guid patientId)
    {
        var medicine_1 = new UserMedicineInventoryItem()
        {
            Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
            UserId = patientId,
            MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
            MeasurementType = "Grams",
            DoseAmount = 5.0,
            IntakeHour = 3,
            IntakeMinute = 30
        };
        var medicine_2 = new UserMedicineInventoryItem()
        {
            Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
            UserId = patientId,
            MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
            MeasurementType = "Grams",
            DoseAmount = 5.0,
            IntakeHour = 3,
            IntakeMinute = 30
        };
        List<UserMedicineInventoryDto> userWithMedicineInventoryItemEntities = new List<UserMedicineInventoryDto>
        {
            new UserMedicineInventoryItem()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
                UserId = patientId,
                MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
                MeasurementType = "Grams",
                DoseAmount = 5.0,
                IntakeHour = 3,
                IntakeMinute = 30
            }.AsDto(medicine_1, "Jown", "Helps patient fall asleep"),
            new UserMedicineInventoryItem()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0DEF"),
                UserId = patientId,
                MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
                MeasurementType = "Grams",
                DoseAmount = 5.0,
                IntakeHour = 3,
                IntakeMinute = 30
        }.AsDto(medicine_2, "James", "Helps patient fall asleep"),
        };
        return userWithMedicineInventoryItemEntities;
        // return ((IEnumerable<UserMedicineInventoryDto>)userWithMedicineInventoryItemEntities);
    }
    private UserMedicineInventoryItem userMedicineInventoryItem(Guid patientId)
    {
        // var medicine_1 = new UserMedicineInventoryItem()
        // {
        //     Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
        //     UserId = patientId,
        //     MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
        //     MeasurementType = "Grams",
        //     DoseAmount = 5.0,
        //     IntakeHour = 3,
        //     IntakeMinute = 30
        // };
        return
            new UserMedicineInventoryItem()
            {
                Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0123"),
                UserId = patientId,
                MedicineId = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B0789"),
                MeasurementType = "Grams",
                DoseAmount = 5.0,
                IntakeHour = 3,
                IntakeMinute = 30
            };
        // }.AsDto(medicine_1, "Jown", "Helps patient fall asleep"),
    }
}