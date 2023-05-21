using System.ComponentModel;

namespace services.Enums;

/// <summary>
/// Enumeration providing generic response description on errors.
/// </summary>
public enum ErrorType
{
  /// <summary>
  /// The unhandled error
  /// </summary>
  [Description("unhandled")]
  Unhandled,
  /// <summary>
  /// The invalid authentication token
  /// </summary>
  [Description("unauthorized")]
  UnauthorizedAccess,
  /// <summary>
  /// The item not found
  /// </summary>
  [Description("not-found")]
  NotFound,
  /// <summary>
  /// The not implemented exception.
  /// </summary>
  [Description("not-implemented")]
  NotImplemented,
  /// <summary>
  /// The invalid payload
  /// </summary>
  [Description("invalid-id")]
  InvalidId,
  /// <summary>
  /// The invalid payload error response.
  /// </summary>
  [Description("invalid-payload")]
  InvalidPayload
}