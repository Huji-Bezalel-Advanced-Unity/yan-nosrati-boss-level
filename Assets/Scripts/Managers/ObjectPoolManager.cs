using System;
using System.Collections.Generic;
using Spells;
using UnityEngine;
using Warriors;

public class ObjectPoolManager
{
    public static ObjectPoolManager Instance;

    private Dictionary<string, Queue<Warrior>> _warriorsPool;
    private Dictionary<string, Queue<Spell>> _spellsPool;

    public ObjectPoolManager()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        _spellsPool = new Dictionary<string, Queue<Spell>>();
        _warriorsPool = new Dictionary<string, Queue<Warrior>>();
    }

    public void AddWarriorToPool(Warrior warrior)
    {
        string type = warrior.tag;
        if (!_warriorsPool.ContainsKey(type))
        {
            _warriorsPool[type] = new Queue<Warrior>();
        }
        _warriorsPool[type].Enqueue(warrior);
    }

    public Warrior GetWarriorFromPool(string tag)
    {
        // foreach (var VARIABLE in _warriorsPool)
        // {
        //     Debug.Log(VARIABLE.Key);
        //     foreach (var a in VARIABLE.Value)
        //     {
        //         Debug.Log(a);
        //     }
        // }
        if (_warriorsPool.ContainsKey(tag) && _warriorsPool[tag].Count > 0)
        {
            Warrior warrior =  _warriorsPool[tag].Dequeue();
            warrior.health = warrior.maxHealth;
            warrior.isDead = false;
            warrior.curDirection = warrior.baseDirection;
        }

        return null;

    }

    public void AddSpellToPool(Spell spell)
    {   
        string type = spell.tag;
        if (!_spellsPool.ContainsKey(type))
        {
            _spellsPool[type] = new Queue<Spell>();
        }
        _spellsPool[type].Enqueue(spell);
    }

    public Spell GetSpellFromPool(string tag)
    {
        
        if (_spellsPool.ContainsKey(tag) && _spellsPool[tag].Count > 0)
        {

            return _spellsPool[tag].Dequeue();
        }
        return null; // or throw an exception if you prefer
    }
}

