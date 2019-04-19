using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEvents : MonoBehaviour
{
    public delegate void Strike();

    public static event Strike OnStrike;

    
}
