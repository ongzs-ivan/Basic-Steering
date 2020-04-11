using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [HideInInspector] public Vector3 center;
    public float radius;

    private void Start()
    {
        center = this.transform.position;
    }
}
