using NullQuestOnline.Models;

namespace NullQuestOnline.Data
{
    public interface IAccountRepository
    {
        bool IsCharacterCreated(string characterName);
        void SaveCharacter(Character character);
        Character LoadCharacter(string characterName);
    }
}