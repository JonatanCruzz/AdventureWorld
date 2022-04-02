using Unity;
using UnityEngine;


[CreateAssetMenu(menuName = "Buffs/HealthBuff")]
public class HealthBuff : ScriptableBuff
{
    public float HealthIncrease;

    public override TimedBuff InitializeBuff(GameObject obj)
    {
        return new TimedHealthBuff(Duration, this, obj);
    }
}