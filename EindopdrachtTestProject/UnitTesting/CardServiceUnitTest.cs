using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Helper;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EindopdrachtTestProject.UnitTesting
{
    public class CardServiceTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mock<IGroupUserService> _groupUserServiceMock;
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _groupUserServiceMock = new Mock<IGroupUserService>();

            _cardService = new CardService(_dbContext, _groupUserServiceMock.Object);
        }

    

        [Fact]
        public async Task CreateCardAsync_SuccessfullyCreatesCard()
        {
            // Arrange
            var user = "testuser";
            var cardDTO = new CardDTO { Name = "New Card", Description = "New Description", Status = DbEnumeration.CardStatus.Todo, Deadline = DateTime.Now.AddDays(1), GroupId = 1, Sprint = 1 };

            _groupUserServiceMock.Setup(x => x.IsInGroup(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _cardService.CreateCardAsync(cardDTO, user);

            // Assert
            Assert.Equal(ErrorCodeEnum.Success, result.ErrorCode);

            var createdCard = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Name == cardDTO.Name);
            Assert.NotNull(createdCard);
            Assert.Equal(cardDTO.Name, createdCard.Name);
            Assert.Equal(cardDTO.Description, createdCard.Description);
            Assert.Equal(cardDTO.Status, createdCard.Status);
            Assert.Equal(cardDTO.Deadline, createdCard.Deadline);
            Assert.Equal(cardDTO.GroupId, createdCard.GroupId);
            Assert.Equal(cardDTO.Sprint, createdCard.Sprint);
        }

        [Fact]
        public async Task UpdateCardAsync_SuccessfullyUpdatesCard()
        {
            // Arrange
            var user = "testuser";
            var existingCard = new Card { Id = 1, Name = "Card 1", Description = "Description 1", Status = DbEnumeration.CardStatus.Todo, Deadline = DateTime.Now.AddDays(1), GroupId = 1, Sprint = 1 };
            _dbContext.Cards.Add(existingCard);
            _dbContext.SaveChanges();

            var updatedCardDTO = new CardDTO { Id = existingCard.Id, Name = "Updated Card", Description = "Updated Description", Status = DbEnumeration.CardStatus.Doing, Deadline = DateTime.Now.AddDays(2), GroupId = 1, Sprint = 2 };

            _groupUserServiceMock.Setup(x => x.IsInGroup(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _cardService.UpdateCardAsync(updatedCardDTO, user);

            // Assert
            Assert.Equal(ErrorCodeEnum.Success, result.ErrorCode);

            var updatedCard = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == existingCard.Id);
            Assert.NotNull(updatedCard);
            Assert.Equal(updatedCardDTO.Name, updatedCard.Name);
            Assert.Equal(updatedCardDTO.Description, updatedCard.Description);
            Assert.Equal(updatedCardDTO.Status, updatedCard.Status);
            Assert.Equal(updatedCardDTO.Deadline, updatedCard.Deadline);
            Assert.Equal(updatedCardDTO.GroupId, updatedCard.GroupId);
            Assert.Equal(updatedCardDTO.Sprint, updatedCard.Sprint);
        }

        [Fact]
        public async Task DeleteCardAsync_SuccessfullyDeletesCard()
        {
            // Arrange
            var user = "testuser";
            var existingCard = new Card { Id = 1, Name = "Card 1", Description = "Description 1", Status = DbEnumeration.CardStatus.Todo, Deadline = DateTime.Now.AddDays(1), GroupId = 1, Sprint = 1 };
            _dbContext.Cards.Add(existingCard);
            _dbContext.SaveChanges();

            _groupUserServiceMock.Setup(x => x.IsInGroup(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            // Act
            var result = await _cardService.DeleteCardAsync(existingCard.Id, user);

            // Assert
            Assert.Equal(ErrorCodeEnum.Success, result.ErrorCode);

            var deletedCard = await _dbContext.Cards.FirstOrDefaultAsync(c => c.Id == existingCard.Id);
            Assert.Null(deletedCard);
        }

        private async Task<User> CreateUserAsync(string userId, string userName, string name)
        {
            var user = new User { Id = userId, UserName = userName, Email = "testuser@example.com", Name = name };

            var userStore = new UserStore<User>(_dbContext);
            var userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

            await userManager.CreateAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}
