using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTypes : MonoBehaviour
{
    public int type;

    public void RoomDestroy()
    {
        Destroy(gameObject);
    }
}
