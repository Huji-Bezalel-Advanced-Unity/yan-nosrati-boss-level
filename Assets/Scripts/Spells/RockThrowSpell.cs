// using System;
// using System.Collections;
// using System.Collections.Generic;
// using DefaultNamespace;
// using UnityEditor.VersionControl;
// using UnityEngine;
// using System.Threading.Tasks;
// using Debuffs;
// using Managers;
// using Warriors;
// using Random = UnityEngine.Random;
// using Task = System.Threading.Tasks.Task;
//
//
// namespace Spells
// {
//     public class RockThrowSpell : Spell
//     {
//         
//         private void Awake()
//         {
//             
//             DebuffsList = new List<Debuff>() { new DamageDebuff(20), new StunDebuff(3) };
//         }
//
//         private void OnEnable()
//         {  
//             double rand = Random.Range(-6f, 6f);
//             // this matrix represents the boss pos, player bos, and a random pos in the mid screen - solving it will yiled a parabola curve  
//             double[,] matrix = new double[,]
//             {
//                 { transform.position.x * transform.position.x, transform.position.x, 1, transform.position.y },
//                 { rand * rand, rand, 1, 0 },
//                 {
//                     Constants.BowPosition.x * Constants.BowPosition.x, Constants.BowPosition.x, 1,
//                     Constants.BowPosition.y + rand
//                 }
//             };
//             List<double> coefs = Util.Solve(matrix);
//             StartRockTravel(this,coefs);
//         }
//
//         public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation)
//         {
//             Spell rock = ObjectPoolManager.Instance.GetObjectFromPool<Spell>(gameObject.tag);
//             if (!rock)
//             {
//                 rock = Instantiate(this, startingPosition, Quaternion.identity);
//             }
//
//             rock.transform.position = startingPosition;
//
//         }
//
//         public void StartRockTravel( RockThrowSpell spell,List<double> coefs)
//         {
//             StartCoroutine(RockTravel(spell, coefs));
//         }
//
//         public override void ResetSpell()
//         {
//             return;
//         }
//
//         private IEnumerator RockTravel(Spell rock, List<double> c)
//         {
//             while (rock || rock.transform.position.x > Constants.BowPosition.x)
//             { 
//                 double newX = rock.transform.position.x - 0.2;
//                 double newY = c[0] * newX * newX + c[1] * newX + c[2];
//                 rock.transform.position = new Vector3((float)newX, (float)newY, 0);
//                 yield return null;
//             }
//
//             ObjectPoolManager.Instance.AddObjectToPool(this);
//         }
//
//         private void OnCollisionEnter2D(Collision2D other)
//         {
//             if (other.gameObject.GetComponent<BasicArrowSpell>() is not null)
//                 ObjectPoolManager.Instance.AddObjectToPool(this);
//         }
//     }
// }


using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Threading.Tasks;
using Debuffs;
using Managers;
using Warriors;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;


namespace Spells
{
    public class RockThrowSpell:Spell
    {
        private void Awake()
        {
            DebuffsList = new List<Debuff>() { new DamageDebuff(20), new StunDebuff(3) };
        }


        public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion playerRotation)
        {
            Spell rock = ObjectPoolManager.Instance.GetObjectFromPool<Spell>(gameObject.tag);
            if (!rock)
            {
                rock = Instantiate(this, startingPosition, Quaternion.identity);
            }
            double rand = Random.Range(-6f,6f);
            double[,] matrix = new double[,] {  // some complicated math to calculate a parabolic shape from the player to the boss
                { startingPosition.x * startingPosition.x, startingPosition.x, 1, startingPosition.y }, 
                { rand * rand, rand, 1, 0 }, 
                { Constants.BowPosition.x * Constants.BowPosition.x, Constants.BowPosition.x, 1, Constants.BowPosition.y + rand }
            };
            List<double> coefs = Util.Solve(matrix);
          
            RockTravel(rock, coefs);
        }
        
        private async void RockTravel(Spell rock, List<double> c)
        {
            while (true)
            {
                if (!rock) return;
                double newX = rock.transform.position.x - 0.2;
                double newY = c[0] * newX * newX + c[1] * newX + c[2];
                rock.transform.position = new Vector3((float)newX, (float)newY, 0);
                await Task.Yield();
            }
        }

     

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<BasicArrowSpell>() is not null) ObjectPoolManager.Instance.AddObjectToPool(this);
        }
    }
}