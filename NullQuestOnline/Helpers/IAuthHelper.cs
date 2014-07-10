namespace NullQuestOnline.Helpers
{
    public interface IAuthHelper
    {
        void SignIn(string characterName);
        void SignOut();
    }
}