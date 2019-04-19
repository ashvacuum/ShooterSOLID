using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharRotator
{
    private readonly float rotationSpeed;    

    public CharRotator(float rotationSpeed)
    {
        this.rotationSpeed = rotationSpeed;
    }
    

    public void Rotate(Vector3 target, Transform currentPos)
    {
        Vector2 direction = target - currentPos.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        currentPos.rotation = Quaternion.Slerp(currentPos.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
