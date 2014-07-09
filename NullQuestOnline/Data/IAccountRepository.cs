using NullQuestOnline.Models;

namespace NullQuestOnline.Data
{
    public interface IAccountRepository
    {
        bool IsCharacterCreated(string characterName);
        void SaveCharacter(GameWorld gameWorld);
        GameWorld LoadWorld(string characterName);
    }
}