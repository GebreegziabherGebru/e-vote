namespace CandidateManagementServiceTests;

using CandidateManagementService.Models;

public class CandidateTests : IDisposable
{
    Candidate candidate;
    public CandidateTests()
    {
        candidate = new Candidate
        {
            FirstName = "G Egziabher",
            LastName = "Gebru",
            Email = "3g.mit02@gmail.com",
            PhoneNumber = "2246399556",
            Biography = "",
            Status = Status.PENDING
        };
    }

    [Fact]
    public void CanChangeFirstName()
    {
        //Arrange

        //Act
        candidate.FirstName = "Gebreegziabher";

        //Assert
        Assert.Equal("Gebreegziabher", candidate.FirstName);
    }

    [Fact]
    public void CanChangeEmail()
    {
        //Arrange


        //Act
        candidate.Email = "gebreegziabher.g.gebru@gmail.com";

        //Assert
        Assert.Equal("gebreegziabher.g.gebru@gmail.com", candidate.Email);
    }

    [Fact]
    public void CanChangePhoneNumber()
    {
        //Arrange


        //Act
        candidate.PhoneNumber = "+12246399556";

        //Assert
        Assert.Equal("+12246399556", candidate.PhoneNumber);
    }

    [Fact]
    public void CanChangeBiography()
    {
        //Arrange


        //Act
        candidate.Biography = "A candidate championing for presidency.";

        //Assert
        Assert.Equal("A candidate championing for presidency.", candidate.Biography);
    }

    public void Dispose()
    {
        candidate = null;
    }
}