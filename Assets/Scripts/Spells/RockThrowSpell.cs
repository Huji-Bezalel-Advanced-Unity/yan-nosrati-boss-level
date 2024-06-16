using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor.VersionControl;
using UnityEngine;
using System.Threading.Tasks;
using Random = UnityEngine.Random;
using Task = System.Threading.Tasks.Task;


namespace Spells
{
    public class RockThrowSpell:Spell
    {
        public override void Cast(Vector2 direction, Vector3 startingPosition, Quaternion PlayerRotation)
        {
            RockThrowSpell rock = Instantiate(this, startingPosition, Quaternion.identity);
            print(rock.enabled);
            double rand = Random.Range(-6f,6f);
            double[,] matrix = new double[,] {
                { startingPosition.x * startingPosition.x, startingPosition.x, 1, startingPosition.y }, 
                { rand * rand, rand, 1, 0 }, 
                { Constants.BowPosition.x * Constants.BowPosition.x, Constants.BowPosition.x, 1, Constants.BowPosition.y + rand }
            };
            List<double> coefs = Util.Solve(matrix);
            foreach (var VARIABLE in coefs)
            {
                print(VARIABLE);
            }
            RockTravel(rock, coefs);
        }

        private async Task RockTravel(RockThrowSpell rock, List<double> c)
        {
            while (rock.transform.position.x > Constants.BowPosition.x)
            {
                double newX = rock.transform.position.x - 0.2;
                double newY = c[0] * newX * newX + c[1] * newX + c[2];
                rock.transform.position = new Vector3((float)newX, (float)newY, 0);
                await Task.Yield();
            }
        }
    }
}