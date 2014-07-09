using System.Collections.Generic;
using NullQuestOnline.Models;

namespace NullQuestOnline.Game
{
    public class Monster : Combatant
    {
        private string _bareHandsAttackName;

        public bool IsBoss { get; set; }

        public override string BareHandsAttackName
        {
            get { return _bareHandsAttackName ?? "Monster Bare Hands Attack"; }
        }

        public void SetBareHandsAttackName(string attackName)
        {
            _bareHandsAttackName = attackName;
        }
    }
}
