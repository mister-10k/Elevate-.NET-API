﻿using Elevate.IOC;
using Elevate.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Unity;

namespace Elevate.IntegrationTests
{
    [TestClass]
    public class UserBLTests
    {
        private readonly IUserBL userBL;

        public UserBLTests()
        {
            // Arrange
            var container = new UnityContainer();
            IOCRegistration.Register(container);

            this.userBL = container.Resolve<IUserBL>();
        }

        [TestMethod]
        public async Task CreateUserAsyncTest()
        {
            // Arrange
            var userDTO = new UserDTO
            {
                FirstName = "kwabena",
                LastName = "Ohemeng",
                Email = null,
                Password = "Welcome4",
                UserTypeId = 1,
                CompanyId = 1,
            };

            // Act
            var result = await userBL.CreateUserAsync(userDTO);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task LoginAsyncTest()
        {
            var email = "kwabeohemeng@gmail.com";
            var password = "Welcome4";

            var result = await userBL.LoginAsync(email, password);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetSignUpMasterDataAsyncTest()
        {
            var result = await userBL.GetSignUpMasterDataAsync();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UserAlreadyHasEmailAsyncTest()
        {
            var email = "kwabeohemeng@gmail.com";

            var result = await userBL.UserAlreadyHasEmailAsync(email);

            Assert.IsTrue(result);
        }
    }
}
