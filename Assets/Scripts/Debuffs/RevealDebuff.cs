using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

public class RevealDebuff : Debuff
{
    public int Duration { get; private set; }

    public RevealDebuff(int duration)
    {
        Duration = duration;
    }

    public void Apply(Entity entity)
    {
        
       StartFade(entity.GetComponent<Renderer>());

    }

    
    // Example of how to call the async function
    public async void StartFade(Renderer renderer)
    {
        if (renderer != null && renderer.material.color.a < 1f)
        {
            await Util.DoFadeLerp(renderer, renderer.material.color.a, 1f, Duration); // Example values for startValue, endValue, and duration
            await Util.DoFadeLerp(renderer, 1f, 0f, Duration);
            
        }
    }

}