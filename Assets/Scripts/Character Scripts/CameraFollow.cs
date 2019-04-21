using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Use this for initialization
    public Transform target;
    public float smoothing;

    public Vector2 maxPos;
    public Vector2 minPos;

    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target == null)
        {
            if (GameObject.FindGameObjectWithTag("Player") != null)
                target = GameObject.FindGameObjectWithTag("Player").transform;
        }
        if (transform.position != target.position)
        {

            Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);
            targetPos.x = Mathf.Clamp(targetPos.x, minPos.x, maxPos.x);
            targetPos.y = Mathf.Clamp(targetPos.y, minPos.y, maxPos.y);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);


        }
    }
}
