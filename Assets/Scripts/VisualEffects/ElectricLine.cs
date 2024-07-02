using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ElectricLine: MonoBehaviour 
{
    [SerializeField] VisualEffect electircLine;
    private Transform _endPosition;
    [SerializeField] private Transform startingPos; // Name of the first exposed Vector2 property
    [SerializeField] private Transform endingPos; // Name of the first exposed Vector2 property


    // Start is called before the first frame update
    public void Init(Transform endPosition)
    {
        _endPosition = endPosition;
        StartCoroutine(DisplayLine());
    }

    private IEnumerator DisplayLine()
    {
        startingPos.position = new Vector3(-30,0,0);
        endingPos.position = _endPosition.position + Vector3.right*5;
        electircLine.enabled = true;
        yield return new WaitForSeconds(1.3f);
        electircLine.enabled = false;
    }

    // Update is called once per frame
    
}
