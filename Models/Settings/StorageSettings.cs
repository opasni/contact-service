namespace contact.Models.Settings;

public class StorageSettings
{
	public const string Name = "Storage";

	/// <summary>
	/// Define the storage type. Can be "memory", "file" or "database"
	/// </summary>
	public string Type { get; set; }
	/// <summary>
	/// Define the access to the storage. In case of "file" it represents the file location
	/// In case of a database, it represents the connection string.
	/// </summary>
	public string Access { get; set; }
}