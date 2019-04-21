using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum heroClass
{
    enforcer,
    fireBat,
    marine
}

public class GameManager : MonoBehaviour
{
    public heroClass classChosen;
    public GameObject[] heroes;

    LevelGenerator levelGen;
    GridMaker gridGen;
    
    private float delayBeforeInitialize = 0.5f;

    private void Awake()
    {
        levelGen = GetComponent<LevelGenerator>();
        gridGen = GetComponent<GridMaker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartGame()
    {
        levelGen.Init();
        yield return new WaitForSeconds(delayBeforeInitialize);
        gridGen.CreateGrid();
        yield return new WaitForSeconds(delayBeforeInitialize);
        SpawnHeroes(classChosen);
    }

    void SpawnHeroes(heroClass classChosen)
    {
        if ((int)classChosen >= heroes.Length)
        {
            classChosen = (heroClass)heroes.Length - 1;
        }
        Instantiate(heroes[(int)classChosen], Vector2.zero, Quaternion.identity);
    }
}
