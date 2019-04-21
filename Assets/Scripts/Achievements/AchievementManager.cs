using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchievementManager : MonoBehaviour
{
    public Achievement[] achievements;

    public GameObject achievementPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
[System.Serializable]
public class Achievement : IAchieve
{
    public int id;
    public string achName;
    public static bool isAchieved;

    public Achievement(int id, string achName, bool achieved)
    {
        this.id = id;
        this.achName = achName;
        isAchieved = achieved;
    }

    public void CheckConditions(Action action)
    {
        
    }
}

public interface IAchieve
{
    void CheckConditions(Action action);
}


