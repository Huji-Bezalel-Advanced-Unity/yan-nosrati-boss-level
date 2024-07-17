using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricLine: MonoBehaviour 
{
    [SerializeField] VisualEffect electircLine;
    private Transform _endPosition;
    [SerializeField] private Transform startingPos;
    [SerializeField] private Transform endingPos; 
    private float effectDuration = 1.3f;


    // Start is called before the first frame update
    public void Init(Transform endPosition)
    {
        _endPosition = endPosition;
        StartCoroutine(DisplayLine());
    }

    private IEnumerator DisplayLine()
    {
        electircLine.enabled = true;
        AudioManager.Instance.PlaySound(SoundName.BowUpgrade);
        yield return new WaitForSeconds(effectDuration);
        electircLine.enabled = false;
    }

    // Update is called once per frame
    
}
