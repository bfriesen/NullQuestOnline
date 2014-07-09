using System.Collections.Generic;
using NullQuestOnline.Game.Combat;

namespace NullQuestOnline.Data
{
    public interface IMonsterDataRepository
    {
        IEnumerable<MonsterArchetype> GetAllMonsterArchetypes();
        IEnumerable<MonsterModifier> GetAllMonsterModifiers();
    }
}
