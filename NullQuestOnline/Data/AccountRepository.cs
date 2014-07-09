using System;
using System.IO;
using System.Text;
using NullQuestOnline.Models;
using XSerializer;

namespace NullQuestOnline.Data
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly XmlSerializer<Character> Serializer;
        static AccountRepository()
        {
            Serializer = new XmlSerializer<Character>(options => options.SetRootElementName("Character").Indent());
        }

        private readonly string saveFolder;
        public AccountRepository()
        {
            saveFolder = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "saves\\");
            Directory.CreateDirectory(Path.GetDirectoryName(saveFolder));
        }

        public bool IsCharacterCreated(string characterName)
        {
            var character = LoadCharacter(characterName);
            if (character != null)
            {
                return character.Created;
            }
            return false;
        }

        public void SaveCharacter(Character character)
        {
            if (!string.IsNullOrWhiteSpace(character.Name))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(saveFolder));
                var saveFile = Path.Combine(saveFolder, string.Format("{0}.xml", character.Name));
                using (var writer = new StreamWriter(saveFile, false, Encoding.UTF8))
                {
                    Serializer.Serialize(writer, character);
                }
            }
            else
            {
                throw new ArgumentException("Character name cannot be null");
            }
        }

        public Character LoadCharacter(string characterName)
        {
            var saveFile = Path.Combine(saveFolder, string.Format("{0}.xml", characterName));
            if (File.Exists(saveFile))
            {
                using (var reader = new StreamReader(saveFile, Encoding.UTF8))
                {
                    return Serializer.Deserialize(reader);
                }   
            }
            return null;
        }
    }
}