namespace MultiShop.Models
{
	public class UserRepository 
	{
		ShopConnection context= new ShopConnection();
		
		public User LoginControl(User user)
		{
			User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password && u.IsAdmin == true && u.Active == true);
			return usr;
		}

		public static User? SelectMemberInfo(string Email)
		{
			using(ShopConnection context=new ShopConnection())
			{
				User? user = context.Users.FirstOrDefault(u => u.Email == Email);
				return user;

            }
				  
		}
		public static string MemberControl(User user)
		{
			using (ShopConnection context=new ShopConnection())
			{
				string answer = "";
				try
				{
					User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
					if (usr==null)
					{
						answer = "error";
					}
					else
					{
						if (usr.IsAdmin==true)
						{
							answer = "admin";
						}
						else
						{
							answer = usr.Email;
						}
					}
					
				}
				
				catch (Exception)
				{
					return "HATA";
					
				}
				return answer;
			}
		}

		public static bool loginEmailControl(User user)
		{
			using (ShopConnection context=new ShopConnection())
			{
				User? usr = context.Users.FirstOrDefault(u => u.Email == user.Email);
				if (usr == null) return false;
				return true;
            }
		}

		public static bool AddUser(User user)
		{
			using (ShopConnection context= new ShopConnection())
			{
				try
				{
					user.Active = true; //formdan gelmediğiinden
					user.IsAdmin = false; // formdan gelmediğinden
					//MD5 şifreleme yapsaydım password için user.password=md5sfirele(user.password) yapardım
					context.Users.Add(user);
					context.SaveChanges();
					return true;
				}
				catch (Exception)
				{

					return false;
				}
			}
			
		}
    }
}
