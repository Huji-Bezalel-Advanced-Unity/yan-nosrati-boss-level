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
            
            if (typeof(T) == typeof(Spell))
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
            _warriorsPool.Clear();
            _spellsPool.Clear();
        }
    }
}