using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class ThreadQueuer : MonoBehaviour
{
    public static ThreadQueuer queuer;

    List<Action> functionsToRunInMainThread;

    private void Start()
    {
        
        functionsToRunInMainThread = new List<Action>();
        //StartThreadedFunction( () => { SlowFunctionThatDoesAUnityThing(Vector3.up); } );        
        if (queuer == null) {
            queuer = this;
        } else
        {
            Destroy(queuer);
            queuer = this;
        }
    }

    private void Update() 
    {
        while(functionsToRunInMainThread.Count > 0)
        {
            Action someFunc = functionsToRunInMainThread[0];
            functionsToRunInMainThread.RemoveAt(0);
            someFunc();            
        }
    }

   

    public void StartThreadedFunction(Action someFunction )
    {
        Thread t = new Thread(new ThreadStart(someFunction));
        t.Start();
    }

    public void QueueMainThreadFunction(Action someFunction)
    {
        functionsToRunInMainThread.Add(someFunction);
    }

    void SlowFunctionThatDoesAUnityThing(Vector3 foo)
    {
        Thread.Sleep(2000);

        Action aFunction = () =>
        {
            Debug.Log("I am applying to a unity GameObject");
            this.transform.position += foo;            
        };
        QueueMainThreadFunction(aFunction);
    }
}
