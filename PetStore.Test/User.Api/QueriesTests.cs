using FluentAssertions;
using Lamar;
using NUnit.Framework;
using PetStore.User.Api.Infrastructure;
using PetStore.User.Api.Model.Queries;

namespace PetStore.Test.User.Api
{
    public class QueriesTests
    {
        private Container _container;

        [SetUp]
        public void Setup()
        {
            _container = new Container(new ApiRegistry());
        }

        [Test]
        public void Should_get_user()
        {
            var getUser = _container.GetInstance<IGetUser>();

            var user = getUser.Execute("PETE");

            user.Should().NotBeNull();
        }
    }
}