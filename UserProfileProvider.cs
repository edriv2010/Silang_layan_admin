using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Security;
using Newtonsoft.Json;

public static class UserProfileProvider
{
	public class UserProfile
	{
		public string Id { get; set; }

		public string Name { get; set; }

		public string FullName { get; set; }

		public string RoleId { get; set; }

		public string BranchId { get; set; }
	}

	private static Dictionary<string, UserProfile> _cache { get; set; }

	public static UserProfile Current
	{
		get
		{
			lock (_cache)
			{
				try
				{
					if (HttpContext.Current != null)
					{
						string value = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName].Value;
						if (!_cache.ContainsKey(value))
						{
							UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(FormsAuthentication.Decrypt(value).UserData);
							if (userProfile == null)
							{
								return null;
							}
							if (_cache.Count > 49)
							{
								_cache = _cache.Skip(_cache.Count - 49).Take(49).ToDictionary((KeyValuePair<string, UserProfile> o) => o.Key, (KeyValuePair<string, UserProfile> o) => o.Value);
							}
							_cache.Add(value, userProfile);
						}
						return _cache[value];
					}
					IPrincipal currentPrincipal = Thread.CurrentPrincipal;
					string name = Current.Name;
					if (!string.IsNullOrEmpty(name))
					{
						TwoArrayList twoArrayList = new TwoArrayList();
						twoArrayList.Add("username", name);
						DataTable dataTable = Command.ExecDataAdapter("SELECT id,FullName,Role_Id,Branch_Id FROM USERS WHERE username = " + Connection.ParameterSymbol + "username", twoArrayList);
						if (dataTable.Rows.Count > 0)
						{
							UserProfile userProfile2 = new UserProfile();
							userProfile2.Id = dataTable.Rows[0]["id"].ToString();
							userProfile2.Name = name;
							userProfile2.FullName = dataTable.Rows[0]["FullName"].ToString();
							userProfile2.RoleId = dataTable.Rows[0]["Role_Id"].ToString();
							userProfile2.BranchId = dataTable.Rows[0]["Branch_Id"].ToString();
							return userProfile2;
						}
						return new UserProfile();
					}
					return new UserProfile();
				}
				catch
				{
					return null;
				}
			}
		}
	}

	static UserProfileProvider()
	{
		_cache = new Dictionary<string, UserProfile>();
	}
}
