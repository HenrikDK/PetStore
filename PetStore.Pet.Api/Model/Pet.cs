namespace PetStore.Pet.Api.Model;

public class Pet
{
    public int Id { get; set; }
    public PetCategory Category { get; set; }
    public string Name { get; set; }
    public IList<string> PhotoUrls { get; set; }
    public string Tags { get; set; }
    public PetStatus Status { get; set; }
}