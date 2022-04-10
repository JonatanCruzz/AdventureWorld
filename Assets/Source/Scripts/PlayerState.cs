#pragma warning disable // All.

/// <summary>An <see cref="Animancer.ControllerState"/> for the 'Player' Animator Controller.</summary>
public sealed class PlayerState : Animancer.ControllerState
{
    #region Hash Constants

    /// <summary>Player_Idle</summary>
    public const int Player_IdleHash = 1485882831;

    /// <summary>Player_Walk</summary>
    public const int Player_WalkHash = 153422479;

    /// <summary>Player_Attack</summary>
    public const int Player_AttackHash = 1813181734;

    /// <summary>Player_Slash</summary>
    public const int Player_SlashHash = 1632421963;

    /// <summary>Aura_Idle</summary>
    public const int Aura_IdleHash = -1809965874;

    /// <summary>Aura_Play</summary>
    public const int Aura_PlayHash = 380860257;

    /// <summary>Dash</summary>
    public const int DashHash = 995914302;

    /// <summary>Player_Damage</summary>
    public const int Player_DamageHash = 974818417;

    /// <summary>Receive_Item</summary>
    public const int Receive_ItemHash = -1707504048;

    /// <summary>movX</summary>
    public const int movXHash = -1219069079;

    /// <summary>movY</summary>
    public const int movYHash = -1068413953;

    /// <summary>Walking</summary>
    public const int WalkingHash = 1744665739;

    /// <summary>attacking</summary>
    public const int attackingHash = 60754182;

    /// <summary>loading</summary>
    public const int loadingHash = 1465923764;

    /// <summary>dash</summary>
    public const int dashHash = -1687233280;

    /// <summary>dead</summary>
    public const int deadHash = -316323548;

    /// <summary>receive item</summary>
    public const int receiveitemHash = 1505267080;

    /// <summary>damage</summary>
    public const int damageHash = 298341484;

    #endregion

    #region Parameter Wrappers

    /// <summary>Creates a new <see cref="PlayerState"/>.</summary>
    public PlayerState(UnityEngine.RuntimeAnimatorController controller, bool keepStateOnStop = false)
        : base(controller, keepStateOnStop)
    {
#if UNITY_EDITOR
        new Animancer.ControllerState.ParameterID("movX", movXHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Float);
        new Animancer.ControllerState.ParameterID("movY", movYHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Float);
        new Animancer.ControllerState.ParameterID("Walking", WalkingHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Bool);
        new Animancer.ControllerState.ParameterID("attacking", attackingHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Trigger);
        new Animancer.ControllerState.ParameterID("loading", loadingHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Trigger);
        new Animancer.ControllerState.ParameterID("dash", dashHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Bool);
        new Animancer.ControllerState.ParameterID("dead", deadHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Trigger);
        new Animancer.ControllerState.ParameterID("receive item", receiveitemHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Bool);
        new Animancer.ControllerState.ParameterID("damage", damageHash).ValidateHasParameter(controller, UnityEngine.AnimatorControllerParameterType.Bool);
#endif
    }

    /// <summary>The value of the 'movX' parameter in the Animator Controller.</summary>
    public float movX
    {
        get => Playable.GetFloat(movXHash);
        set => Playable.SetFloat(movXHash, value);
    }

    /// <summary>The value of the 'movY' parameter in the Animator Controller.</summary>
    public float movY
    {
        get => Playable.GetFloat(movYHash);
        set => Playable.SetFloat(movYHash, value);
    }

    /// <summary>The value of the 'Walking' parameter in the Animator Controller.</summary>
    public bool Walking
    {
        get => Playable.GetBool(WalkingHash);
        set => Playable.SetBool(WalkingHash, value);
    }

    /// <summary>Sets the 'attacking' trigger in the Animator Controller.</summary>
    public void Setattacking() => Playable.SetTrigger(attackingHash);

    /// <summary>Resets the 'attacking' trigger in the Animator Controller.</summary>
    public void Resetattacking() => Playable.ResetTrigger(attackingHash);

    /// <summary>Sets the 'loading' trigger in the Animator Controller.</summary>
    public void Setloading() => Playable.SetTrigger(loadingHash);

    /// <summary>Resets the 'loading' trigger in the Animator Controller.</summary>
    public void Resetloading() => Playable.ResetTrigger(loadingHash);

    /// <summary>The value of the 'dash' parameter in the Animator Controller.</summary>
    public bool dash
    {
        get => Playable.GetBool(dashHash);
        set => Playable.SetBool(dashHash, value);
    }

    /// <summary>Sets the 'dead' trigger in the Animator Controller.</summary>
    public void Setdead() => Playable.SetTrigger(deadHash);

    /// <summary>Resets the 'dead' trigger in the Animator Controller.</summary>
    public void Resetdead() => Playable.ResetTrigger(deadHash);

    /// <summary>The value of the 'receive item' parameter in the Animator Controller.</summary>
    public bool receiveitem
    {
        get => Playable.GetBool(receiveitemHash);
        set => Playable.SetBool(receiveitemHash, value);
    }

    /// <summary>The value of the 'damage' parameter in the Animator Controller.</summary>
    public bool damage
    {
        get => Playable.GetBool(damageHash);
        set => Playable.SetBool(damageHash, value);
    }

    /// <summary>ParameterCount</summary>
    public override int ParameterCount
    {
        get => 9;
    }

    /// <summary>GetParameterHash</summary>
    public override int GetParameterHash(int index)
    {
        switch (index)
        {
            case 0: return movXHash;// movX.
            case 1: return movYHash;// movY.
            case 2: return WalkingHash;// Walking.
            case 3: return attackingHash;// attacking.
            case 4: return loadingHash;// loading.
            case 5: return dashHash;// dash.
            case 6: return deadHash;// dead.
            case 7: return receiveitemHash;// receive item.
            case 8: return damageHash;// damage.
            default: throw new System.ArgumentOutOfRangeException(nameof(index));
        }
    }

    #endregion


}
