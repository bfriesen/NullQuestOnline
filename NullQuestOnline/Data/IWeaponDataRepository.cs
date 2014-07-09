using System.Collections.Generic;
using NullQuest.Game.Combat;

namespace NullQuestOnline.Data
{
    public interface IWeaponDataRepository
    {
        IEnumerable<WeaponArchetype> GetAllWeapons();
    }
}