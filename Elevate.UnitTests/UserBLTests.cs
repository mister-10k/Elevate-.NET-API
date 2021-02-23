using Elevate.Business;
using Elevate.IOC;
using Elevate.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using System;
using System.Threading.Tasks;
using Unity;

namespace Elevate.UnitTests
{
    [TestClass]
    public class UserBLTests
    {
        private UserBL userBL;

        #region mock DL's
        private IUserDL MockUserDL;
        #endregion

        public UserBLTests()
        {
            MockUserDL = MockRepository.GenerateMock<IUserDL>();
        }

        [TestInitialize]

        public void Initialize()
        {
            userBL = new UserBL(MockUserDL);
        }

        [TestMethod]
        public async Task CreateUserAsync_WithValidNotTakenEmail_ReturnsNewlyCreatedUserDTO()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                FirstName = "kwabena",
                LastName = "Ohemeng",
                Email = "kwabe@gmail.com",
                Password = "Welcome4",
                UserTypeId = 1,
                CompanyId = 1,
            };

            MockUserDL
              .Stub(x => x.CreateUserAsync(Arg<UserDTO>.Is.Anything))
              .Return(Task.FromResult(userDTO));

            // Act
            var result = await userBL.CreateUserAsync(userDTO);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task LoginAsyncTest_WithCorrectUserNameAndPassWord_ReturnsNotNullUserDTO()
        {
            MockUserDL
              .Stub(x => x.GetUserByEmailAsync(Arg<string>.Is.Anything))
              .Return(Task.FromResult(new UserDTO {
                Email = "kwabeohemeng@gmail.com",
                Password = "$2a$10$90t/k6JnH1/VDpPiHJ3J.OS/whzww2cLRhdUKsUPUn4Rb9fVHyZ0y" 
              }));

            var email = "kwabeohemeng@gmail.com";
            var password = "Welcome4";

            var result = await userBL.LoginAsync(email, password);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetSignUpMasterDataAsync_NoParameters_ReturnsNotNullMasterData()
        {
            MockUserDL
             .Stub(x => x.GetSignUpMasterDataAsync())
             .Return(Task.FromResult(new SignUpMasterDataDTO { }));

            var result = await userBL.GetSignUpMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UserAlreadyHasEmailAsync_WithEmailAlreadyInUse_ReturnsTrue()
        {
            MockUserDL
             .Stub(x => x.UserAlreadyHasEmailAsync(Arg<string>.Is.Anything))
             .Return(Task.FromResult(true));

            var email = "kwabeohemeng@gmail.com";

            var result = await userBL.UserAlreadyHasEmailAsync(email);

            Assert.IsTrue(result);
        }
    }
}
