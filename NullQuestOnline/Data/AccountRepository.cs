using System;
using System.IO;
using System.Text;
using NullQuestOnline.Game;
using XSerializer;

namespace NullQuestOnline.Data
{
    public class AccountRepository : IAccountRepository
    {
        private static readonly XmlSerializer<GameWorld> Serializer;
        static AccountRepository()
        {
            Serializer = new XmlSerializer<GameWorld>(options => options.SetRootElementName("Character").Indent());
        }

        private readonly string saveFolder;
        public AccountRepository()
        {
            saveFolder = Path.Combine(AppDomain.CurrentDomain.GetData("DataDirectory").ToString(), "saves\\");
            Directory.CreateDirectory(Path.GetDirectoryName(saveFolder));
        }

        public bool IsCharacterCreated(string characterName)
        {
            var character = LoadWorld(characterName);
            if (character != null)
            {
                return character.Created;
            }
            return false;
        }

        public void SaveCharacter(GameWorld gameWorld)
        {
            if (!string.IsNullOrWhiteSpace(gameWorld.Character.Name))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(saveFolder));
                var saveFile = Path.Combine(saveFolder, string.Format("{0}.xml", gameWorld.Character.Name));
                using (var writer = new StreamWriter(saveFile, false, Encoding.UTF8))
                {
                    Serializer.Serialize(writer, gameWorld);
                }
            }
            else
            {
                throw new ArgumentException("Character name cannot be null");
            }
        }

        public GameWorld LoadWorld(string characterName)
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