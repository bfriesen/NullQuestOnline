using NullQuestOnline.Game;

namespace NullQuestOnline.Data
{
    public interface IAccountRepository
    {
        bool IsCharacterCreated(string characterName);
        void SaveWorld(GameWorld gameWorld);
        GameWorld LoadWorld(string characterName);
    }
}