using contact.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nelibur.ObjectMapper;

namespace contact.Controllers;

/// <summary>
/// Class describing controller for contact requests.
/// </summary>
[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
	private readonly UserData userData;
	private readonly string secret;

  public UserController(
		IOptions<UserData> options,
		IConfiguration configuration
		)
  {
    this.userData = options.Value;
		this.secret = configuration["Secret"];

		TinyMapper.Bind<UserData, UserDataBasic>();
  }

    /// <summary>
    /// Endpoint allowing retrieving the personal data of the user.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [HttpGet(Name = "GetUserData")]
    public IActionResult GetBasicData([FromQuery] string password)
            => Ok(string.Equals(password, secret) ? userData : TinyMapper.Map<UserDataBasic>(userData));
}