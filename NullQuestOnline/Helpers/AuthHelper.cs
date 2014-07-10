using System.Web.Security;

namespace NullQuestOnline.Helpers
{
    public class AuthHelper : IAuthHelper
    {
        public void SignIn(string characterName)
        {
            FormsAuthentication.SetAuthCookie(characterName, true);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}