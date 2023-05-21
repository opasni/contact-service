namespace contact.Models.Settings;

public class EmailSettings
{
  public const string Email = "EmailSettings";

  /// <summary>
  /// Gets or sets the host.
  /// </summary>
  /// <value>The host.</value>
  public string Host { get; set; }
  /// <summary>
  /// Gets or sets the port.
  /// </summary>
  /// <value>The port.</value>
  public int Port { get; set; }
  /// <summary>
  /// Gets or sets the user name.
  /// </summary>
  /// <value>The user name.</value>
  public string Username { get; set; }
  /// <summary>
  /// Gets or sets the password.
  /// </summary>
  /// <value>The password.</value>
  public string Password { get; set; }
  /// <summary>
  /// Gets or sets the display name.
  /// </summary>
  /// <value>The display name.</value>
  public string DisplayName { get; set; }
}
