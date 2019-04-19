using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected float rotationSpeed;

    protected CharRotator rotator;
    protected Vector3 target;
}
