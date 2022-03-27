using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AttackSpecifications
{
    [SerializeField]
    public Vector2 attackDirection;
    [SerializeField]
    public float knockback;
    [SerializeField]
    public int damage;
}
