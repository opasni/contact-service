
using contact.Models.Web;

namespace contact.Services;

public interface IContactStorage
{
  void Set<T>(Guid contactId, T contactData);

  T Get<T>(Guid contactId);
}