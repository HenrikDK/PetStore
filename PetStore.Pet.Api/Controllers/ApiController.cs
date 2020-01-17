using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStore.Pet.Api.Model;
using PetStore.Pet.Api.Model.Commands;
using PetStore.Pet.Api.Model.Queries;

namespace PetStore.Pet.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IGetPet _getPet;
        private readonly IUpdatePet _updatePet;
        private readonly IInsertPet _insertPet;
        private readonly IDeletePet _deletePet;
        private readonly IInsertPetPhoto _insertPetPhoto;
        private readonly IGetPetByStatus _getPetByStatus;

        public ApiController(IGetPet getPet, IUpdatePet updatePet, IInsertPet insertPet, IDeletePet deletePet, 
            IInsertPetPhoto insertPetPhoto, IGetPetByStatus getPetByStatus)
        {
            _getPet = getPet;
            _updatePet = updatePet;
            _insertPet = insertPet;
            _deletePet = deletePet;
            _insertPetPhoto = insertPetPhoto;
            _getPetByStatus = getPetByStatus;
        }

        /// <summary>
        /// Find pet by ID
        /// </summary>
        /// <param name="petId">ID of pet to return</param>
        /// <response code="404">Pet was not found</response>
        [HttpGet("/v1/pet/{petId}")]
        public ActionResult<Model.Pet> Get(int petId)
        {
            var pet = _getPet.Execute(petId);
            if (pet == null)
            {
                return NotFound();
            }
            
            return Ok(pet);
        }
        
        /// <summary>
        /// Updates a pet in the store with form data
        /// </summary>
        /// <param name="petId">ID of pet that needs to be updated</param>
        /// <param name="name">Updated name of the pet</param>
        /// <param name="status">Updated status of the pet</param>
        /// <response code="405">Invalid status</response>
        [HttpPost("/v1/pet/{petId}")]
        public ActionResult Get(int petId, [FromForm] string name, [FromForm] string status)
        {
            if (!Enum.TryParse(typeof(PetStatus), status, true, out var petStatus))
            {
                return StatusCode(405);
            }

            using (var scope = new TransactionScope())
            {
                _updatePet.Execute(petId, name, (PetStatus)petStatus);
                scope.Complete();
            }

            var pet = _getPet.Execute(petId);
            
            return Ok(pet);
        }
        
        /// <summary>
        /// Deletes a pet
        /// </summary>
        /// <param name="petId">ID of pet that needs to be deleted</param>
        /// <response code="404">Pet was not found</response>
        [HttpDelete("/v1/pet/{petId}")]
        public ActionResult Delete(int petId)
        {
            var pet = _getPet.Execute(petId);
            if (pet == null)
            {
                return NotFound();
            }
            
            using (var scope = new TransactionScope())
            {
                _deletePet.Execute(petId);
                scope.Complete();
            }
            
            return Ok();
        }
        
        /// <summary>
        /// Uploads an image
        /// </summary>
        /// <param name="petId">ID of pet to update</param>
        /// <param name="additionalMetadata">Additional data to pass to server</param>
        /// <param name="file">file to upload</param>
        [HttpPost("/v1/pet/{petId}/uploadImage")]
        public ActionResult Upload(int petId, [FromForm] string additionalMetadata, [FromForm] IFormFile file)
        {
            var photoId = Guid.NewGuid();
            var filePath = "/some/path" + photoId;
            var fileUrl = "/some/url/" + photoId;
            using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                file.CopyTo(fileStream);
            }

            using (var scope = new TransactionScope())
            {
                _insertPetPhoto.Execute(photoId, petId, additionalMetadata, fileUrl);
                scope.Complete();
            }
            
            return Ok();
        }
        
        /// <summary>
        /// Add a new pet to the store
        /// </summary>
        /// <param name="pet">Pet object that needs to be added to the store</param>
        [HttpPost("/v1/pet")]
        public ActionResult<Model.Pet> Insert([FromBody] Model.Pet pet)
        {
            using (var scope = new TransactionScope())
            {
                var petId = _insertPet.Execute(pet);
                scope.Complete();

                pet.Id = petId;
            }
            
            return Ok(pet);
        }
        
        /// <summary>
        /// Update an existing pet
        /// </summary>
        /// <param name="pet">Pet object that needs to be updated to the store</param>
        [HttpPut("/v1/pet")]
        public ActionResult<Model.Pet> Update([FromBody] Model.Pet pet)
        {
            using (var scope = new TransactionScope())
            {
                _updatePet.Execute(pet);
                scope.Complete();
            }
            
            return Ok(pet);
        }
        
        /// <summary>
        /// Finds Pets by status
        /// </summary>
        /// <param name="status">Status value that needs to be considered for filter: available pending or sold</param>
        /// <response code="405">Invalid status</response>
        [HttpGet("/v1/pet/findByStatus")]
        public ActionResult<List<Model.Pet>> Get(string status)
        {
            if (!Enum.TryParse(typeof(PetStatus), status, true, out var petStatus))
            {
                return StatusCode(405);
            }
            
            var pet = _getPetByStatus.Execute((PetStatus) petStatus);
            if (!pet.Any())
            {
                return NotFound();
            }
            
            return Ok(pet);
        }
    }
}
