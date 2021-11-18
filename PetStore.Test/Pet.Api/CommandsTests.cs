using PetStore.Pet.Api.Infrastructure;
using PetStore.Pet.Api.Model;
using PetStore.Pet.Api.Model.Commands;

namespace PetStore.Test.Pet.Api;

public class CommandsTests
{
    private Container _container;

    [SetUp]
    public void Setup()
    {
        _container = new Container(new ApiRegistry());
    }

    [Test]
    public void Should_delete_pet()
    {
        var deletePet = _container.GetInstance<IDeletePet>();

        deletePet.Execute(1);
    }
        
    [Test]
    public void Should_insert_pet()
    {
        var insertPet = _container.GetInstance<IInsertPet>();

        var pet = new PetStore.Pet.Api.Model.Pet
        {
            Category = PetCategory.Cat,
            Name = "Peter",
            Status = PetStatus.Sold,
            Tags = "lobster, not_really_a_cat, evil"
        };

        var petId = 0;
        using (var scope = new TransactionScope())
        {
            petId = insertPet.Execute(pet);
            scope.Complete();
        }

        petId.Should().NotBe(0);
    }
        
    [Test]
    public void Should_insert_pet_photo()
    {
        var insertPetPhoto = _container.GetInstance<IInsertPetPhoto>();

        insertPetPhoto.Execute(Guid.NewGuid(), 123, "wak wak", $"/photos/{Guid.NewGuid()}");
    }
        
    [Test]
    public void Should_update_pet()
    {
        var updatePet = _container.GetInstance<IUpdatePet>();

        var pet = new PetStore.Pet.Api.Model.Pet
        {
            Id = 1234,
            Category = PetCategory.Cat,
            Name = "Peter",
            Status = PetStatus.Sold,
            Tags = "lobster, not_really_a_cat, evil"
        }; 
            
        updatePet.Execute(pet);
    }
}