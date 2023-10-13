using AuthenticationAndAuthorization.Models;

namespace AuthenticationAndAuthorization.Dataset
{
    public static class UserContext
    {
        public static List<User> DB()
        {
            return new List<User> 
                   { 
                        new User(){Id = 1,UserName="abc123",Password="123123",IsDisable=true},
                        new User(){Id = 2,UserName="xyz123",Password="123123",IsDisable=false},
                        new User(){Id= 3,UserName="prq123",Password="123123",IsDisable =false }
                   };
        }
    }
}
