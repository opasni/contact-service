using contact.Models.Web;
using Microsoft.Extensions.Caching.Memory;

namespace contact.Services;

public interface IContactStorage
{
  void Set(Guid contactId, ContactData contactData);

  ContactData Get(Guid contactId);
}

public class ContactStorage : IContactStorage
{
  private readonly IMemoryCache memoryCache;

  public ContactStorage(IMemoryCache memoryCache)
  {
    this.memoryCache = memoryCache;
  }

  public ContactData Get(Guid contactId)
  {
    if (!memoryCache.TryGetValue(contactId, out ContactData contactData))
    {
      throw new KeyNotFoundException();
    }
    return contactData;
  }

  public void Set(Guid contactId, ContactData contactData)
  {
    if (contactData != null)
    {
      memoryCache.Set(contactId, contactData);
    }
  }
}