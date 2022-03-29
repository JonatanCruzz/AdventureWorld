using Unity;
using UnityEngine;
[System.Serializable]
public abstract class TimedBuff
{
    [SerializeField] protected float Duration;
    [SerializeField] protected int EffectStacks;
    [SerializeField] public ScriptableBuff Buff { get; }
    protected readonly GameObject Obj;
    public bool IsFinished;

    public TimedBuff(float duration, ScriptableBuff buff, GameObject obj)
    {
        Duration = duration;
        Buff = buff;
        Obj = obj;
    }

    public void Tick(float delta)
    {
        Duration -= delta;
        if (Duration <= 0)
        {
            End();
            IsFinished = true;
        }
    }

    /**
     * Activates buff or extends duration if ScriptableBuff has IsDurationStacked or IsEffectStacked set to true.
     */
    public void Activate()
    {
        if (Buff.IsEffectStacked || Duration <= 0)
        {
            ApplyEffect();
            EffectStacks++;
        }

        if (Buff.IsDurationStacked || Duration <= 0)
        {
            Duration += Buff.Duration;
        }
    }
    protected abstract void ApplyEffect();
    public abstract void End();
}