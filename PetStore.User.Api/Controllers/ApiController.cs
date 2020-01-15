using System;
using System.Collections.Generic;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using MoreLinq;
using PetStore.User.Api.Model.Commands;
using PetStore.User.Api.Model.Queries;

namespace PetStore.User.Api.Controllers
{
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IDeleteUser _deleteUser;
        private readonly IInsertUser _insertUser;
        private readonly IGetUser _getUser;
        private readonly IUpdateUser _updateUser;
        private readonly IEncryptPassword _encryptPassword;

        public ApiController(IDeleteUser deleteUser, IInsertUser insertUser, IGetUser getUser, IUpdateUser updateUser,
            IEncryptPassword encryptPassword)
        {
            _deleteUser = deleteUser;
            _insertUser = insertUser;
            _getUser = getUser;
            _updateUser = updateUser;
            _encryptPassword = encryptPassword;
        }

        /// <summary>
        /// Get user by user name
        /// </summary>
        /// <param name="username">The users username</param>
        /// <response code="400">Invalid username</response>
        /// <response code="404">User not found</response>
        /// <returns></returns>
        [HttpGet("/v1/user/{username}")]
        public ActionResult<Model.User> Get(string username)
        {
            var user = _getUser.Execute(username);
            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }
        
        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="username">The users username</param>
        /// <param name="user">The user to update</param>
        /// <response code="400">Invalid user</response>
        /// <response code="404">User not found</response>
        /// <returns></returns>
        [HttpPut("/v1/user/{username}")]
        public ActionResult Update(string username, [FromBody] Model.User user)
        {
            var existingUser = _getUser.Execute(username);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.Status = user.Status;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;

            var (hash, salt) = _encryptPassword.Execute(user.PasswordHash);
            existingUser.PasswordHash = hash;
            existingUser.Salt = salt;
            
            using (var scope = new TransactionScope())
            {
                _updateUser.Execute(existingUser);
                scope.Complete();
            }
            
            return Ok();
        }

        /// <summary>
        /// Delete the user
        /// </summary>
        /// <param name="username">The users username</param>
        /// <response code="400">Invalid user</response>
        /// <response code="404">User not found</response>
        /// <returns></returns>
        [HttpDelete("/v1/user/{username}")]
        public ActionResult Delete(string username)
        {
            var existingUser = _getUser.Execute(username);
            if (existingUser == null)
            {
                return NotFound();
            }
            
            using (var scope = new TransactionScope())
            {
                _deleteUser.Execute(username);
                scope.Complete();
            }
            
            return Ok();
        }
        
        /// <summary>
        /// Logs user into the system
        /// </summary>
        /// <param name="username">The user name for login</param>
        /// <param name="password">The password for login in clear text</param>
        /// <response code="400">Invalid username password</response>
        /// <returns></returns>
        [HttpGet("/v1/user/login")]
        public ActionResult<string> Login([FromQuery] string username, [FromQuery] string password)
        {
            var user = _getUser.Execute(username);
            if (user == null)
            {
                return StatusCode(400);
            }

            var (hash, salt) = _encryptPassword.Execute(password, user.Salt);

            if (user.PasswordHash != hash)
            {
                return StatusCode(400);
            }
            
            return Ok(Guid.NewGuid().ToString());
        }
        
        /// <summary>
        /// Logs user out of the system
        /// </summary>
        /// <returns></returns>
        [HttpGet("/v1/user/logout")]
        public ActionResult Logout()
        {
            // Not implemented

            return Ok();
        }
        
        /// <summary>
        /// Create user
        /// </summary>
        /// <param name="user">The user to be created</param>
        /// <returns></returns>
        [HttpPost("/v1/user")]
        public ActionResult<Model.User> Create([FromBody] Model.User user)
        {
            var (hash, salt) = _encryptPassword.Execute(user.PasswordHash);

            user.PasswordHash = hash;
            user.Salt = salt;
            
            using (var scope = new TransactionScope())
            {
                var userid = _insertUser.Execute(user);
                scope.Complete();
                
                user.Id = userid;
            }
            
            return Ok(user);
        }
        
        /// <summary>
        /// Creates list of users with given input array
        /// </summary>
        /// <param name="users">The list of users</param>
        /// <returns></returns>
        [HttpPost("/v1/user/createWithList")]
        public ActionResult Create([FromBody] IList<Model.User> users)
        {
            using (var scope = new TransactionScope())
            {
                users.ForEach(u => _insertUser.Execute(u));
                scope.Complete();
            }

            return Ok();
        }
    }
}
