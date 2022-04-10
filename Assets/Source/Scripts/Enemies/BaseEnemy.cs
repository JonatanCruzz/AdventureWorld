using System.Collections;
using Animancer;
using NodeCanvas.BehaviourTrees;
using Sirenix.OdinInspector;
using UnityEngine;

public enum EnemyAttackInstacementMode
{
    Lineal,
    Random,
    Spread
}
[System.Serializable]
public class EnemyAttackDefinition
{
    public int instaceAmount;
    public int knockback;
    public int damage;
    public float chargeTime;
    public bool reloadPerInstance;
    public float range;
    public float speed;
    public bool modifyProjectileSpeed = false;
    [ShowIf("modifyProjectileSpeed")]
    public float projectileSpeed;
    public float cooldown;
    [EnumToggleButtons]
    public EnemyAttackInstacementMode instacementMode;
    [AssetsOnly]
    public GameObject prefab;
}

[CreateAssetMenu(menuName = "AdventureWorld/BaseEnemy")]
public class BaseEnemy : ScriptableObject
{
    [AssetsOnly]
    public GameObject prefab;
    public AnimationClip idleAnimation;
    public DirectionalAnimationSet moveAnimation;
    [HideIf("attackFreezeAnimation")]
    public DirectionalAnimationSet attackAnimation;
    [HideIf("attackFreezeAnimation")]
    public DirectionalAnimationSet attackChargeAnimation;
    public bool attackFreezeAnimation = false;
    public AnimationClip damagedAnimation;

    // public AnimationClip dieAnimation;
    public float moveSpeed = 1f;
    public float visionRange = 5f;
    public EnemyAttackDefinition attackDefinition = new EnemyAttackDefinition();

    public bool comeBack = true;

    public float health;
    
}