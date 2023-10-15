using contact.Models.Web;
using Microsoft.Extensions.Caching.Memory;

namespace contact.Services;

public class MemoryStorage : IContactStorage
{
  private readonly IMemoryCache memoryCache;

  public MemoryStorage(IMemoryCache memoryCache)
  {
    this.memoryCache = memoryCache;
  }

  public T Get<T>(Guid contactId)
  {
    if (!memoryCache.TryGetValue(contactId, out T contactData))
    {
      throw new KeyNotFoundException();
    }
    return contactData;
  }

  public void Set<T>(Guid contactId, T contactData)
  {
    if (contactData != null)
    {
      memoryCache.Set(contactId, contactData);
    }
  }
}