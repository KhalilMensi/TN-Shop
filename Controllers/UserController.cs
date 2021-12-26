using AngularAspCore.Extensions;
using AngularAspCore.Models.BLL;
using AngularAspCore.Models.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAspCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpGet("users/all")]
        public ActionResult<List<User>> GetAll()
        {
            return BLLUser.GetAllUsers();
        }
        [HttpPost("user/SignUp")]
        public string SignUp(User user)
        {
            return BLLUser.AddUser(user);
        }
        [HttpPost("user/UpdateUser")]
        public string UpdateUser(User user)
        {
            return BLLUser.UpdateUser(user);
        }
        [HttpDelete("user/DeleteUserBy/{IdUser}")]
        public string DeleteUser(long IdUser)
        {
            return BLLUser.DeleteUserBy("Id",IdUser.ToString());
        }

    }
}
