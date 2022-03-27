using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUnit : MonoBehaviour
{
    public float HP;
    [SerializeField] private float maxHP;
    public float Max_HP
    {
        get => maxHP;
        set
        {
            // update HP to have the same proportion of the new max HP
            HP = (HP * (float)value / maxHP);
            maxHP = value;
        }
    }

    void Start()
    {
        HP = Max_HP;
    }
}
