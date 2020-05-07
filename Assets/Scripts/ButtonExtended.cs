using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExtended : MonoBehaviour
{
    private Vector3 initialScale;
    public Vector3 biggerScale;
    private void Start() 
    {
        initialScale = transform.localScale;
    }
    private void OnMouseEnter() 
    {
        transform.localScale = biggerScale;
    }
    private void OnMouseExit() 
    {
        transform.localScale = initialScale;
    }
}
