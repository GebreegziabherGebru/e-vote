namespace CandidateManagementServiceTests;

using CandidateManagementService.Dtos;
using CandidateManagementService.Models;
using CandidateManagementService.Repository;
using Moq;
using CandidateManagementService.Profiles;
using AutoMapper;
using CandidateManagementService.Controllers;
using Microsoft.AspNetCore.Mvc;

public class CandidatesControllerTests : IDisposable
{
    Mock<ICandidateRepository> mockRepo;
    //CandidatesProfile profile;
    //MapperConfiguration configuration;
    //IMapper mapper;

    public CandidatesControllerTests()
    {
        mockRepo = new Mock<ICandidateRepository>();
        //profile = new CandidatesProfile();
        //configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
        //mapper = new Mapper(configuration);
    }

    public void Dispose()
    {
        mockRepo = null;
        //profile = null;
        //configuration = null;
        //mapper = null;
    }

    //**************************************************
    //*
    //GET   /api/candidates Unit Tests
    //*
    //**************************************************

    [Fact]
    public async Task GetCandidates_ReturnsZeroResources_WhenDBIsEmpty()
    {
        //Arrange
        mockRepo.Setup(repo => repo.FindAllAsync()).ReturnsAsync(GetCandidates(0));
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindAll();

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCandidates_ReturnsOneItem_WhenDBHasOneResource()
    {
        //Arrage
        mockRepo.Setup(repo => repo.FindAllAsync()).ReturnsAsync(GetCandidates(1));
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindAll();

        var okResult = result.Result as OkObjectResult;

        var candidates = okResult?.Value as List<CandidateResponseDto>;

        //Assert
        Assert.Single(candidates);
    }

    public async Task GetAllCandidates_Returns200K_WhenDBHasOneResource()
    {
        //Arrange
        mockRepo.Setup(repo => repo.FindAllAsync()).ReturnsAsync(GetCandidates(1));

        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindAll();

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetAllCandidates_ReturnsCorrectType_WhenDBHasOneResource()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindAllAsync()).ReturnsAsync(GetCandidates(1));
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindAll();

        //Assert
        Assert.IsType<ActionResult<IEnumerable<CandidateResponseDto>>>(result);
    }

    //**************************************************
    //*
    //GET   /api/candidates/{id} Unit Tests
    //*
    //**************************************************
    [Fact]
    public async Task GetCandidatesByID_Returns404NotFound_WhenNonExistentIDProvided()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(() => null);
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindById(1);

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetCandidatesByID_Returns200OK__WhenValidIDProvided()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindById(1);

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task GetCandidateByID_ReturnsCorrectResouceType_WhenValidIDProvided()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.FindById(1);

        //Assert
        Assert.IsType<ActionResult<CandidateResponseDto>>(result);
    }

    //**************************************************
    //*
    //POST   /api/candidates/ Unit Tests
    //*
    //**************************************************
    [Fact]
    public async Task SaveCandidates_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Save(new CandidateRequestDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            PhoneNumber = "123-456-7890",
            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            Status = Status.PENDING
        });

        //Assert
        Assert.IsType<ActionResult<CandidateResponseDto>>(result);
    }

    [Fact]
    public async Task SaveCandidate_Returns201Created_WhenValidObjectSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Save(new CandidateRequestDto { });

        //Assert
        Assert.IsType<CreatedResult>(result.Result);
    }

    [Fact]
    public async Task SaveCandidate_ReturnsBadRequest_WhenNullSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Save(null);

        //Assert
        Assert.IsType<BadRequestResult>(result.Result);
    }

    //**************************************************
    //*
    //PUT   /api/candidates/{id} Unit Tests
    //*
    //**************************************************
    [Fact]
    public async Task UpdateCandidate_Returns200OK_WhenValidObjectSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Update(1, new CandidateRequestDto { });

        //Assert
        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateCandidate_ReturnsCorrectResourceType_WhenValidObjectSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());
        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Update(1, new CandidateRequestDto { });

        //Assert
        Assert.IsType<ActionResult<CandidateResponseDto>>(result);
    }

    [Fact]
    public async Task UpdateCommand_Returns404NotFound_WhenNonExistentResourceIDSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(0)).ReturnsAsync(() => null);

        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Update(0, new CandidateRequestDto { });

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    //**************************************************
    //*
    //DELETE   /api/candidates/{id} Unit Tests
    //*
    //**************************************************
    [Fact]
    public async Task DeleteCandidate_ReturnsNoContent_WhenValidResourceIDSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(GetCandidate());

        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Delete(1);

        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCandidate_ReturnsNotFound_WhenInvalidResourceIDSubmitted()
    {
        //Arrange 
        mockRepo.Setup(repo => repo.FindByIdAsync(1)).ReturnsAsync(() => null);

        var controller = new CandidatesController(mockRepo.Object);

        //Act
        var result = await controller.Delete(1);

        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    private List<CandidateResponseDto> GetCandidates(int num)
    {
        var candidates = new List<CandidateResponseDto>();
        if (num > 0)
            candidates.Add(GetCandidate());
        return candidates;
    }

    private CandidateResponseDto GetCandidate()
    {
        return new CandidateResponseDto
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            PhoneNumber = "123-456-7890",
            Biography = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            Status = Status.PENDING
        };
    }
}