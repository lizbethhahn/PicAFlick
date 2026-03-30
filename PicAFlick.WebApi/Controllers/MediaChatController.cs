using Microsoft.AspNetCore.Mvc;
using PicAFlick.WebApi.Models;

namespace PicAFlick.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaChatController : ControllerBase
{
    [HttpPost]
    public ActionResult<string> Post([FromBody] MediaChatRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest("Message is required.");
        }

        return Ok($"PicAFlick heard you say: {request.Message}");
    }
}
