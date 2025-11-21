using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/")]
public class RootController : ControllerBase
{
    [HttpGet]
    public string Get() => "Backend API is running!";
}
