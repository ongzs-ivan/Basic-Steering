using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoidData", menuName = "ScriptableObjects/Boids", order = 1)]
public class BoidsSetting : ScriptableObject
{
    public float mass;              // 15
    public float mVelocity;         // 3    
    public float mForce;            // 15
    public float mAhead;            // 10
    public float mAvoidForce;       // 5

    [Header("Weights")]
    public float targetWeight;

    [Header("Rules Settings")]
    public float SeparationForce;   // 50
    public float SeparationRadius;  // 25
    public float DetectionRadius;   // 50
}
