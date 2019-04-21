using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AchievementManager : MonoBehaviour
{
    public Achievement[] achievements;

    public GameObject achievementPanel;

    Achievement ach1 = new Achievement(1, "Detecting Input!", false);
    Achievement ach2 = new Achievement(2, "You died!", false);

    // Start is called before the first frame update
    void Start()
    {
        achievementPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput(ach1);
        CheckDeath(ach2);
    }

    public void UpdateAchievements(Achievement a)
    {
        StartCoroutine(SetAchievementTrue(a));
    }

    IEnumerator SetAchievementTrue(Achievement a)
    {
        achievementPanel.transform.GetChild(0).GetComponent<Text>().text = a.achName;
        achievementPanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        achievementPanel.SetActive(false);
    }

    void CheckInput(Achievement a)
    {
        if (Input.anyKey && !a.isAchieved)
        {
            a.isAchieved = true;
            UpdateAchievements(a);
        }   
    }

    void CheckDeath(Achievement a)
    {
        if (GameObject.FindGameObjectWithTag("Player") != null) {
            GameObject g = GameObject.FindGameObjectWithTag("Player");
            if (g.GetComponent<Health>().GetHealthPercentage <= 0
                && !a.isAchieved)
            {
                a.isAchieved = true;
                UpdateAchievements(a);
            }
        }
    }
}
[System.Serializable]
public class Achievement
{
    public int id;
    public string achName;
    public bool isAchieved;
    public Action action;

    public Achievement(int id, string achName, bool achieved)
    {
        this.id = id;
        this.achName = achName;
        isAchieved = achieved;        
    }
}

public interface IAchieve
{
    void DoCheck(Action action);
}


