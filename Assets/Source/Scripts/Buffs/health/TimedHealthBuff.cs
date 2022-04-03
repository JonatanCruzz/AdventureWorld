using UnityEngine;

public class TimedHealthBuff : TimedBuff
{
    private readonly HealthUnit _healthComponent;

    public TimedHealthBuff(float duration, ScriptableBuff buff, GameObject obj) : base(duration, buff, obj)
    {
        //Getting MovementComponent, replace with your own implementation
        _healthComponent = obj.GetComponent<HealthUnit>();
    }

    protected override void ApplyEffect()
    {
        //Add Health increase to MovementComponent
        HealthBuff HealthBuff = (HealthBuff)Buff;
        _healthComponent.AddictiveHP += HealthBuff.HealthIncrease;
        Debug.Log("HealthBuff: " + _healthComponent.AddictiveHP);
    }

    public override void End()
    {
        //Revert Health increase
        HealthBuff HealthBuff = (HealthBuff)Buff;
        _healthComponent.AddictiveHP -= HealthBuff.HealthIncrease * EffectStacks;
        EffectStacks = 0;
        Debug.Log("HealthBuffEnd: " + _healthComponent.AddictiveHP);
    }
}