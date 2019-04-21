using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject[] enemies;
    
    LevelGenerator levelGen;
    GridMaker gridGen;
    [SerializeField] private int enemiesToSpawn;
    private float delayBeforeInitialize = 0.5f;

    [SerializeField]private Image healthSlider;

    [SerializeField] private GameObject healthUI;

    [SerializeField] private GameObject selectionPanel;



    private Health playerHealth;
    private void Awake()
    {
        levelGen = GetComponent<LevelGenerator>();
        gridGen = GetComponent<GridMaker>();
    }

    // Start is called before the first frame update
    void Start()
    {
        selectionPanel.SetActive(true);
        healthUI.SetActive(false);
        //StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth == null)
        {
            healthUI.SetActive(false);
            return;
        }
        else if(!healthUI.activeInHierarchy && playerHealth != null)
        {
            healthUI.SetActive(true);            
        }
        healthSlider.fillAmount = playerHealth.GetHealthPercentage;
    }

    public void BeginGame(int hero)
    {
        classChosen = (heroClass)hero;
        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {        
        levelGen.Init();
        yield return new WaitForSeconds(delayBeforeInitialize);
        gridGen.CreateGrid();
        yield return new WaitForSeconds(delayBeforeInitialize);
        selectionPanel.SetActive(false);
        SpawnHeroes(classChosen);
        yield return new WaitForSeconds(delayBeforeInitialize);
        SpawnEnemies(enemiesToSpawn);                
    }

    void SpawnHeroes(heroClass classChosen)
    {
        if ((int)classChosen >= heroes.Length)
        {
            classChosen = (heroClass)heroes.Length - 1;
        }
        GameObject hero = Instantiate(heroes[(int)classChosen], Vector2.zero, Quaternion.identity);
        playerHealth = hero.GetComponent<Health>();
    }

    void SpawnEnemies(int enemiesToSpawn)
    {
        int numberofEnemies = 0;
        while (numberofEnemies < enemiesToSpawn && levelGen.availableLocationsToSpawnEnemies.Count > 0)
        {
            int randLocation = Random.Range(0, levelGen.availableLocationsToSpawnEnemies.Count);
            int randomEnemy = Random.Range(0, enemies.Length);
            int loopCounter = 0;
            while (Vector2.Distance(Vector2.zero, levelGen.availableLocationsToSpawnEnemies[randLocation]) < 10f && loopCounter < 200)
            {
                randLocation = Random.Range(0, levelGen.availableLocationsToSpawnEnemies.Count);
                loopCounter++;                
            }
            Instantiate(enemies[randomEnemy], levelGen.availableLocationsToSpawnEnemies[randLocation], Quaternion.identity);
            levelGen.availableLocationsToSpawnEnemies.RemoveAt(randLocation);
            numberofEnemies++;
        }
    }
    
}
