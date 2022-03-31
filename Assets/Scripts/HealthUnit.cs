using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUnit : MonoBehaviour
{
    public float HP = 0;
    [SerializeField]
    private float maxHP = 0;
    [SerializeField]
    private float _addictiveHP = 0;
    [SerializeField]
    private float _multiplierHP = 0;
    public float BaseHP = 0;
    [HideInInspector]
    public float AddictiveHP
    {
        get => _addictiveHP;
        set
        {
            _addictiveHP = value;
            this.updateMaxHP();
        }
    }
    [HideInInspector]
    public float MultiplierHP
    {
        get => _multiplierHP;
        set
        {
            _multiplierHP = value;
            this.updateMaxHP();
        }
    }
    public float Max_HP
    {
        get => maxHP;
    }
    private void updateMaxHP()
    {
        var relativeHp = maxHP != 0 ? HP / maxHP : 1;
        maxHP = (BaseHP + AddictiveHP) * (MultiplierHP + 1);
        HP = Mathf.Clamp(maxHP * relativeHp, 0, maxHP);

        Debug.Log("Max HP: " + this.maxHP + " HP: " + this.HP);

    }
    public void Awake()
    {
        this.HP = this.maxHP;
        this.updateMaxHP();

    }
}
