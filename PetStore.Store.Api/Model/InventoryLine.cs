using System.Collections.Generic;

namespace PetStore.Store.Api.Model
{
    public class InventoryLine
    {
        public PetStatus Status { get; set; }
        public int Count { get; set; }
    }
}
