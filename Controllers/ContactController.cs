using Microsoft.AspNetCore.Mvc;
using services.Models;
using services.Models.Web;
using services.Services;

namespace services.Controllers;

/// <summary>
/// Class describing controller for contact requests.
/// </summary>
[ApiController]
[Route("contact")]
public class ContactController : ControllerBase
{
  private readonly ILogger logger;
  private readonly IContactService contactService;

  public ContactController(
     ILogger<ContactController> logger,
     IContactService contactService)
  {
    this.logger = logger;
    this.contactService = contactService;
  }

  /// <summary>
  /// Endpoint allowing contact POST requests.
  /// </summary>
  /// <param name="data"></param>
  /// <returns></returns>
  [HttpPost(Name = "ContactRequest")]
  public IActionResult ContactRequest(ContactData data)
  {
    // Send an email to the contact email first.
    return Ok(contactService.CreateContactRequest(data));
  }

  /// <summary>
  /// Endpoint allowing the recaptcha verification process.
  /// </summary>
  /// <param name="data"></param>
  /// <returns></returns>
  [HttpPost("recaptcha", Name = "CheckContactRequestReCaptcha")]
  public async Task<IActionResult> CheckContactRequest(ReCaptchaValidation data)
  {
    await contactService.VerifyAndSend(data);
    return NoContent();
  }
}