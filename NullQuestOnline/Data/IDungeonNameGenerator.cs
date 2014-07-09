namespace NullQuestOnline.Data
{
    public interface IDungeonNameGenerator
    {
        string GenerateName(string characterName, int dungeonLevel);
    }
}
