using FluentAssertions;
using Lamar;
using NUnit.Framework;
using PetStore.User.Api.Infrastructure;
using PetStore.User.Api.Model;
using PetStore.User.Api.Model.Commands;

namespace PetStore.Test.User.Api
{
    public class CommandsTests
    {
        private Container _container;

        [SetUp]
        public void Setup()
        {
            _container = new Container(new ApiRegistry());
        }

        [Test]
        public void Should_delete_user()
        {
            var deleteUser = _container.GetInstance<IDeleteUser>();

            deleteUser.Execute("PETE");
        }
        
        [Test]
        public void Should_insert_user()
        {
            var insertUser = _container.GetInstance<IInsertUser>();

            var user = new PetStore.User.Api.Model.User
            {
                Email = "spam@real.com",
                Phone = "12345678",
                FirstName = "Mayo",
                LastName = "Pete",
                Status = UserStatus.Active,
                UserName = "PETE",
            };
            
            var petId = insertUser.Execute(user);

            petId.Should().NotBe(0);
        }
        
        [Test]
        public void Should_update_user()
        {
            var updateUser = _container.GetInstance<IUpdateUser>();

            var user = new PetStore.User.Api.Model.User
            {
                Email = "spam@real.com",
                Phone = "12345678",
                FirstName = "Mayo",
                LastName = "Pete",
                Status = UserStatus.Inactive,
                UserName = "PETE",
            };
            
            updateUser.Execute(user);
        }
    }
}