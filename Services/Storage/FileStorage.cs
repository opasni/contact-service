using contact.Models.Settings;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace contact.Services;

public class FileStorage : IContactStorage
{
	private readonly string filePath;

	public char Separator = '|';
	public string FileName { get; set; } = "contact";
	public string FileType { get; set; } = ".csv";

	public FileStorage(IOptions<StorageSettings> options)
	{
		string path = string.IsNullOrWhiteSpace(options.Value.Access) ? Path.GetTempPath() : options.Value.Access;
		filePath = GetFile(path);
		if (!File.Exists(filePath))
		{
			File.WriteAllText(filePath, $"id{Separator}data\n");
		}
	}

	public T Get<T>(Guid contactId)
	{
		var lines = File.ReadAllLines(filePath);
		string id = contactId.ToString();
		foreach (var line in lines)
		{
			var columns = line.Split(Separator);
			if (columns[0] == id)
			{
				if (!string.IsNullOrWhiteSpace(columns[1]))
				{
					return JsonSerializer.Deserialize<T>(columns[1]);
				}
			}
		}
		return default;
	}

	public void Set<T>(Guid contactId, T contactData)
	{
		var lines = File.ReadAllLines(filePath).ToList();

		bool updated = false;
		string id = contactId.ToString();
		string data = JsonSerializer.Serialize(contactData);

		for (int i = 0; i < lines.Count; i++)
		{
			var columns = lines[i].Split(Separator);
			if (columns[0] == id)
			{
				lines[i] = $"{id}{Separator}{data}";
				updated = true;
				break;
			}
		}

		if (!updated)
		{
			lines.Add($"{id}{Separator}{data}");
		}

		File.WriteAllLines(filePath, lines);

	}

	private string GetFile(string path) => Path.Combine(path, FileName + FileType);
}