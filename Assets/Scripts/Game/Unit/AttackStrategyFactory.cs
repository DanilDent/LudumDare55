using Misc;
using Unity.VisualScripting;

public class AttackStrategyFactory : MonoSingleton<AttackStrategyFactory>
{
    public AttackStrategyCompBase Create(UnitComp unitComp, AttackTypeEnum attackType)
    {
        if (attackType == AttackTypeEnum.Melee)
        {
            return unitComp.AddComponent<AttackStrategyMelee>();
        }

        if (attackType == AttackTypeEnum.Range)
        {
            return unitComp.AddComponent<AttackStrategyRange>();
        }

        return null;
    }
}
