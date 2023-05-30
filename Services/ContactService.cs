using Microsoft.Extensions.Caching.Memory;
using contact.Models.Web;
using contact.Services.Api;
using Microsoft.Extensions.Options;
using contact.Models.Settings;
using contact.Utility.Language;

namespace contact.Services;

public interface IContactService
{
  /// <summary>
  /// Creates a new request for contact in the memory cache until the request is verified by recaptcha.
  /// </summary>
  /// <param name="contactData"></param>
  /// <returns></returns>
  public Contact CreateContactRequest(ContactData contactData);

  /// <summary>
  /// Verifies if the request is present in memory, if the recaptcha is valid and if, sends out the email contact.
  /// </summary>
  /// <param name="validation"></param>
  /// <returns></returns>
  public Task VerifyAndSend(ReCaptchaValidation validation);
}

public class ContactService : IContactService
{
  private readonly EmailSettings settings;
  private readonly ILogger logger;
  private readonly IMemoryCache memoryCache;
  private readonly IEmailService emailService;
  private readonly IReCaptchaApiService reCaptchaApi;

  public ContactService(
     ILogger<ContactService> logger,
     IOptions<EmailSettings> options,
     IMemoryCache memoryCache,
     IEmailService emailService,
     IReCaptchaApiService reCaptchaApi)
  {
    this.logger = logger;
    this.memoryCache = memoryCache;
    this.emailService = emailService;
    this.reCaptchaApi = reCaptchaApi;
    this.settings = options.Value;
  }

  public Contact CreateContactRequest(ContactData contactData)
  {
    Guid contactId = Guid.NewGuid();
    memoryCache.Set(contactId, contactData);

    return new Contact
    {
      ContactId = contactId,
      Email = contactData.Email
    };
  }

  public async Task VerifyAndSend(ReCaptchaValidation validation)
  {
    if (!memoryCache.TryGetValue(validation.ContactId, out ContactData contactData))
    {
      throw new KeyNotFoundException();
    }
    if (!await reCaptchaApi.Verify(validation.Recaptcha))
    {
      throw new UnauthorizedAccessException();
    }

    var subject = CultureHelper.Translatable[contactData.LanguageId]["subject"][contactData.Subject];
    var messageSystem = CultureHelper.Translatable[contactData.LanguageId]["feedback"]["system"];
    messageSystem = messageSystem.Replace("{{name}}", contactData.Name);
    messageSystem = messageSystem.Replace("{{email}}", contactData.Email);
    messageSystem = messageSystem.Replace("{{subject}}", subject);
    messageSystem = messageSystem.Replace("{{message}}", contactData.Message);
    // Send the notification
    var notify = emailService.CreateMail(new Receiver
    {
      Email = settings.ContactEmail,
      Name = settings.DisplayName
    }, subject, messageSystem);

    // Set the reply_to header for easier communication.
    notify.ReplyTo.Add(new MimeKit.MailboxAddress(contactData.Name, contactData.Email));
    await emailService.SendAsync(notify);

    var messageUser = CultureHelper.Translatable[contactData.LanguageId]["feedback"]["user"];
    // Send confirmation email to the user.
    var confirm = emailService.CreateMail(new Receiver
    {
      Email = contactData.Email,
      Name = contactData.Name
    }, subject, messageUser);

    // Set the reply_to header for easier communication.
    notify.ReplyTo.Add(new MimeKit.MailboxAddress(settings.DisplayName, settings.ContactEmail));
    await emailService.SendAsync(confirm);
  }
}