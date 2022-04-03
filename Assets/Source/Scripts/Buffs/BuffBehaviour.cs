using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class BuffBehaviour : MonoBehaviour
{
    private readonly Dictionary<ScriptableBuff, TimedBuff> _buffs = new Dictionary<ScriptableBuff, TimedBuff>();
    void Update()
    {
        //OPTIONAL, return before updating each buff if game is paused
        //if (Game.isPaused)
        //    return;

        foreach (var buff in _buffs.Values.ToList())
        {
            buff.Tick(Time.deltaTime);
            if (buff.IsFinished)
            {
                _buffs.Remove(buff.Buff);
            }
        }
    }

    public void AddBuff(TimedBuff buff)
    {
        Debug.Log("Adding buff: " + buff.Buff.name + " of type: " + buff.Buff.GetType().Name);
        if (_buffs.ContainsKey(buff.Buff))
        {
            _buffs[buff.Buff].Activate();
            Debug.Log("Buff already exists, activating it");
        }
        else
        {
            _buffs.Add(buff.Buff, buff);
            buff.Activate();
            Debug.Log("Buff added");
        }
    }
}