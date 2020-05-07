using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Romani;

public class Teste : MonoBehaviour
{
    public Transform tgt1;
    public Transform tgt2;
    private void Start() {
        print(Vector3.Distance(tgt1.position, tgt2.position));
        print(MathLib.Distance(tgt1.position, tgt2.position));
    }
}
