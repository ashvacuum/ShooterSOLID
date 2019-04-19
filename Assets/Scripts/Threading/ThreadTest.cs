using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ThreadTest : MonoBehaviour
{

    
    Thread myThread;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start() :: Started");
        myThread = new Thread( SlowJob );
        //runs in a new thread
        myThread.Start();
        Debug.Log("Start() :: Done");
    }

    // Update is called once per frame
    void Update()
    {
        if(myThread.IsAlive)
            Debug.Log("SlowJob is Running");
        
    }

    void SlowJob()
    {

        Debug.Log("Starting SlowJob doing 1000 things.");
        
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        sw.Start();

        for(int i = 0; i < 1000; i++)
        {
            Thread.Sleep(2);            
        }

        sw.Stop();

        Debug.Log("Done Executing after " + sw.ElapsedMilliseconds / 1000 + " seconds");

        
    }
}
