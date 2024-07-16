// using System;
// using System.Collections.Generic;
// using Spells;
// using UnityEngine;
// using Warriors;
//
// namespace Managers
// {
//     public class ObjectPoolManager
//     {
//         public static ObjectPoolManager Instance;
//
//         private Dictionary<string, Queue<Warrior>> _warriorsPool;
//         private Dictionary<string, Queue<Spell>> _spellsPool;
//
//         public ObjectPoolManager()
//         {
//             if (Instance == null)
//             {
//                 Instance = this;
//             }
//
//             _spellsPool = new Dictionary<string, Queue<Spell>>();
//             _warriorsPool = new Dictionary<string, Queue<Warrior>>();
//         }
//
//         public void AddWarriorToPool(Warrior warrior)
//         {
//             warrior.ResetWarrior();
//             string type = warrior.tag;
//             if (!_warriorsPool.ContainsKey(type))
//             {
//                 _warriorsPool[type] = new Queue<Warrior>();
//             }
//
//             _warriorsPool[type].Enqueue(warrior);
//         }
//
//         public Warrior GetWarriorFromPool(string tag)
//         {
//             // foreach (var VARIABLE in _warriorsPool)
//             // {
//             //     Debug.Log(VARIABLE.Key);
//             //     foreach (var a in VARIABLE.Value)
//             //     {
//             //         Debug.Log(a);
//             //     }
//             // }
//             if (_warriorsPool.ContainsKey(tag) && _warriorsPool[tag].Count > 0)
//             {
//                 Warrior warrior = _warriorsPool[tag].Dequeue();
//                 warrior.gameObject.SetActive(true);
//             
//             }
//
//             return null;
//         }
//
//         public void AddSpellToPool(Spell spell)
//         {
//             spell.ResetSpell();
//             string type = spell.tag;
//             if (!_spellsPool.ContainsKey(type))
//             {
//                 _spellsPool[type] = new Queue<Spell>();
//             }
//
//             _spellsPool[type].Enqueue(spell);
//         }
//
//         public Spell GetSpellFromPool(string tag)
//         {
//             if (_spellsPool.ContainsKey(tag) && _spellsPool[tag].Count > 0)
//             {
//                 Spell spell = _spellsPool[tag].Dequeue();
//                 spell.gameObject.SetActive(true);
//                 return spell;
//             }
//
//             return null;
//         }
//     }
// }


using System.Collections.Generic;
using UnityEngine;


namespace Managers
{
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
            GameManager.Instance.EndGame += DisableAllActiveObjects;
        }

        ~ObjectPoolManager()
        {
            GameManager.Instance.EndGame -= DisableAllActiveObjects;
        }

        public void AddObjectToPool<T>(T obj) where T : MonoBehaviour
        {
            if (obj is Warrior warrior)
            {
                warrior.ResetWarrior();
                AddToPool(warrior, _warriorsPool);
            }
            else if (obj is Spell spell)
            {
                spell.ResetSpell();
                AddToPool(spell, _spellsPool);
            }
            else
            {
                Debug.LogWarning($"Object of type {typeof(T)} is not supported by the pool.");
            }
        }

        public T GetObjectFromPool<T>(string tag) where T : MonoBehaviour
        {
            if (typeof(T) == typeof(Warrior))
            {
                
                return GetFromPool(tag, _warriorsPool) as T;
            }
            else if (typeof(T) == typeof(Spell))
            {
                return GetFromPool(tag, _spellsPool) as T;
            }

            Debug.LogWarning($"Object of type {typeof(T)} is not supported by the pool.");
            return null;
        }

        private void AddToPool<T>(T obj, Dictionary<string, Queue<T>> pool) where T : MonoBehaviour
        {
            string type = obj.tag;
            if (!pool.ContainsKey(type))
            {
                obj.gameObject.SetActive(false);
                pool[type] = new Queue<T>();
            }

            pool[type].Enqueue(obj);
        }

        private T GetFromPool<T>(string tag, Dictionary<string, Queue<T>> pool) where T : MonoBehaviour
        {
            if (pool.ContainsKey(tag) && pool[tag].Count > 0)
            {
                T obj = pool[tag].Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }

            return null;
        }

        public void DisableAllActiveObjects()
        {
            DisableAllObjectsInPool(_warriorsPool);
            DisableAllObjectsInPool(_spellsPool);
        }

        private void DisableAllObjectsInPool<T>(Dictionary<string, Queue<T>> pool) where T : MonoBehaviour
        {
            foreach (var kvp in pool)
            {
                foreach (var obj in kvp.Value)
                {
                    obj.gameObject.SetActive(false);
                }
            }
        }
    }
}
    

