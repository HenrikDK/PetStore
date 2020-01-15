using FluentAssertions;
using Lamar;
using NUnit.Framework;
using PetStore.Pet.Api.Infrastructure;
using PetStore.Pet.Api.Model;
using PetStore.Pet.Api.Model.Queries;

namespace PetStore.Test.Pet.Api
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
        public void Should_get_pet()
        {
            var getPet = _container.GetInstance<IGetPet>();

            var pet = getPet.Execute(1);

            pet.Should().NotBeNull();
        }
        
        [Test]
        public void Should_get_pet_by_status()
        {
            var getPet = _container.GetInstance<IGetPetByStatus>();

            var pet = getPet.Execute(PetStatus.Available);

            pet.Should().NotBeNull();
            pet.Count.Should().BeGreaterThan(0);
        }
    }
}