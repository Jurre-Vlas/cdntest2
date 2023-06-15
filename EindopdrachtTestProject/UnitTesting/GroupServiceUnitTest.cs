using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EindopdrachtTestProject
{
    public class GroupServiceTests : IDisposable
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IGroupUserService _groupUserService;
        private readonly ILogger<GroupService> _logger;

        public GroupServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            var loggerMock = new Mock<ILogger<GroupService>>();
            _groupUserService = new GroupUserService(_dbContext);
            _logger = loggerMock.Object;
        }

        [Fact]
        public async Task GetGroupsByUserAsync_ReturnsGroups()
        {
            // Arrange
            var user = "testuser";
            var userId = "testuserid";
            var expectedGroups = new List<GroupDTO>
            {
                new GroupDTO { Id = 1, Name = "Group 1", Description = "Description 1" },
                new GroupDTO { Id = 2, Name = "Group 2", Description = "Description 2" },
                new GroupDTO { Id = 3, Name = "Group 3", Description = "Description 3" }
            };

            await CreateUserAsync(userId, user, "peterjan123");


            var group1 = new Group { Id = 1, Name = "Group 1", Description = "Description 1" };
            var group2 = new Group { Id = 2, Name = "Group 2", Description = "Description 2" };
            var group3 = new Group { Id = 3, Name = "Group 3", Description = "Description 3" };
            _dbContext.Groups.AddRange(group1, group2, group3);
            _dbContext.GroupUsers.AddRange(
                new GroupUser { UserId = userId, GroupId = 1, Role = "GroupAdmin" },
                new GroupUser { UserId = userId, GroupId = 2, Role = "GroupAdmin" },
                new GroupUser { UserId = userId, GroupId = 3, Role = "GroupAdmin" }
            );
            await _dbContext.SaveChangesAsync();

            var groupService = new GroupService(_dbContext, _groupUserService, _logger);

            // Act
            var result = await groupService.GetGroupsByUserAsync(user);

            // Assert
            Assert.Equal(ErrorCodeEnum.Success, result.ErrorCode);

            Assert.Equal(expectedGroups.Count, result.Result.Count); 

            for (int i = 0; i < expectedGroups.Count; i++)
            {
                Assert.Equal(expectedGroups[i].Id, result.Result[i].Id); 
                Assert.Equal(expectedGroups[i].Name, result.Result[i].Name); 
                Assert.Equal(expectedGroups[i].Description, result.Result[i].Description); 
            }
        }

        [Fact]
        public async Task CreateGroupAsync_SuccessfullyCreatesGroup()
        {
            // Arrange
            var groupDTO = new GroupDTO { Name = "Test Group", Description = "Test Description" };
            var userName = "testuser";

            await CreateUserAsync("testuserid", userName, "peter");

            var groupService = new GroupService(_dbContext, _groupUserService, _logger);

            // Act
            var result = await groupService.CreateGroupAsync(groupDTO, userName);

            // Assert
            Assert.Equal(ErrorCodeEnum.Success, result.ErrorCode);
        }

        [Fact]
        public async Task CreateGroupAsync_ReturnsBadRequestWhenUserNotFound()
        {
            // Arrange
            var groupDTO = new GroupDTO { Name = "Test Group", Description = "Test Description" };
            var userName = "nonexistinguser";

            var groupService = new GroupService(_dbContext, _groupUserService, _logger);

            // Act
            var result = await groupService.CreateGroupAsync(groupDTO, userName);

            // Assert
            Assert.Equal(ErrorCodeEnum.BadRequest, result.ErrorCode);
        }


        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        private async Task<User> CreateUserAsync(string userId, string userName, string name)
        {
            var user = new User { Id = userId, UserName = userName, Email = "testuser@example.com", Name = name};

            var userStore = new UserStore<User>(_dbContext);
            var userManager = new UserManager<User>(userStore, null, null, null, null, null, null, null, null);

            await userManager.CreateAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }


    }
}
