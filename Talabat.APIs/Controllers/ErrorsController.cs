using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers
{
    //This class Handel NotfoundEndpoint 

    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi =true)] //I say to Swagger don't to this controller Docummentation ,because sagger showed error because you was not define (verb)
    public class ErrorsController : ControllerBase
    {
        //EndPoint that Will Redirect to it incase that execute Not found Endpoint  

        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }


    }
}
