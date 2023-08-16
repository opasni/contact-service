namespace contact.Models.Settings;

public class UserData : UserDataBasic
{
	public const string Name = "User";

	public string Address { get;set; }
	public string Phone { get;set; }
	public string Email { get;set; }
}

public class UserDataBasic
{
	public string FirstName { get;set; }
	public string LastName { get;set; }
	public string NickName { get;set; }
	public string ImagePath { get;set; }
	public string Website { get;set; }
}