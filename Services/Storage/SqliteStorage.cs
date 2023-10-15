using contact.Models.Settings;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace contact.Services;

public class SqliteStorage : IContactStorage
{
	private readonly string connectionString = "Data Source=data.db";

	public string Table { get; set; } = "contacts";

	public SqliteStorage(IOptions<StorageSettings> options)
	{
		if (!string.IsNullOrWhiteSpace(options.Value.Access))
		{
			connectionString = options.Value.Access;
		}
		InitializeDatabase();
	}

	private void InitializeDatabase()
	{
		using var connection = new SqliteConnection(connectionString);
		connection.Open();

		var createTableCommand = connection.CreateCommand();
		createTableCommand.CommandText = @$"
        CREATE TABLE IF NOT EXISTS {Table}(
            id TEXT PRIMARY KEY,
            data TEXT
        )";
		createTableCommand.ExecuteNonQuery();
	}

	public T Get<T>(Guid contactId)
	{
		string id = contactId.ToString();
		using var connection = new SqliteConnection(connectionString);
		connection.Open();

		var selectCommand = connection.CreateCommand();
		selectCommand.CommandText = $"SELECT data FROM {Table} WHERE id = $id";
		selectCommand.Parameters.AddWithValue("$id", id);

		string result = selectCommand.ExecuteScalar() as string;
		if (!string.IsNullOrEmpty(result))
		{
			return JsonSerializer.Deserialize<T>(result);
		}
		return default;
	}

	public void Set<T>(Guid contactId, T contactData)
	{
		string id = contactId.ToString();
		string data = JsonSerializer.Serialize(contactData);

		using var connection = new SqliteConnection(connectionString);
		connection.Open();

		var upsertCommand = connection.CreateCommand();
		upsertCommand.CommandText = @$"
        INSERT OR REPLACE INTO {Table}(id, data)
        VALUES($id, $data)";
		upsertCommand.Parameters.AddWithValue("$id", id);
		upsertCommand.Parameters.AddWithValue("$data", data);

		upsertCommand.ExecuteNonQuery();
	}
}