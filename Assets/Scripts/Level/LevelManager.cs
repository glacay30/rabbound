﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> planetList;

    [SerializeField]
    private Vector2 targetPosition;

    [SerializeField]
    private GameObject targetPlanet;

    //Define the range of RGN for each planet. The number should between 0.0 and 1.0, last element must be 1.0. Eg {0.5, 0.7, 1.0}
    [SerializeField]
    private List<List<float>> planetPossibility;

    [SerializeField]
    private List<float> level0;

    [SerializeField]
    private List<float> level1;

    [SerializeField]
    private List<float> level2;

    [SerializeField]
    private List<float> level3;


    //Level is defined by distance. After the player reach certain amount of distance, the level becomes harder and planet possibility need to red
    [SerializeField]
    private List<float> levelDistance;

    [SerializeField]
    private GameObject player;

    //Distance between player position and the old generate box center
    [SerializeField]
    private float distanceToRegenerate;

    //Varialble to determine how far each cell center is to its adjacent cell center
    [SerializeField]
    private float cellDistance;

    //How many cells on each line(line to center)
    [SerializeField]
    private float generateBoxCellPerLength;


    public float generatedPersentage;

    private Vector3 generateBoxCenter;
    private float generateBoxLength;

    private HashSet<Vector2> generatedCells;

    private int currentLevel;

    private Vector3 origin;

    [SerializeField]
    private AudioClip winSound;
    private AudioSource audioPlayer;

    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        generatedCells = new HashSet<Vector2>();
        generateBoxLength = generateBoxCellPerLength * cellDistance;

        targetPlanet.transform.Translate(new Vector3(targetPosition.x - targetPlanet.transform.position.x, targetPosition.y - targetPlanet.transform.position.y, 0));
        generatedCells.Add(new Vector2((int)targetPosition.x, (int)targetPosition.y));
        generatedCells.Add(new Vector2((int) targetPosition.x - 1, (int) targetPosition.y - 1));
        generatedCells.Add(new Vector2((int)targetPosition.x - 1, (int)targetPosition.y + 1));
        generatedCells.Add(new Vector2((int)targetPosition.x + 1, (int)targetPosition.y - 1));
        generatedCells.Add(new Vector2((int)targetPosition.x + 1, (int)targetPosition.y + 1));
        currentLevel = 0;
        origin = player.transform.position;
        planetPossibility = new List<List<float>>();
        planetPossibility.Add(level0);
        planetPossibility.Add(level1);
        planetPossibility.Add(level2);
        planetPossibility.Add(level3);
    }


    // Start is called before the first frame update
    void Start()
    {
        generateBoxCenter = player.transform.position;
        generatedCells.Add(new Vector2(0, 0));
        GeneratePlanets();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, generateBoxCenter) >= distanceToRegenerate)
            GeneratePlanets();

        if (currentLevel < 3 && Vector3.Distance(player.transform.position, origin) >= levelDistance[currentLevel])
            currentLevel++;

        if (!player.activeSelf)
            GameOver();

        if (player.GetComponent<Player>().Win)
            GameWin();
    }

    void GeneratePlanets()
    {
        generateBoxCenter = player.transform.position;
        for (int xcoord = Mathf.CeilToInt(generateBoxCenter.x - generateBoxLength); xcoord <= Mathf.FloorToInt(generateBoxCenter.x + generateBoxLength); xcoord++)
            for (int ycoord = Mathf.CeilToInt(generateBoxCenter.y - generateBoxLength); ycoord <= Mathf.FloorToInt(generateBoxCenter.y + generateBoxLength); ycoord++)
                if (xcoord % (int)cellDistance == 0 && ycoord % (int)cellDistance == 0 && !generatedCells.Contains(new Vector2(xcoord, ycoord)))
                {
                    //Calculate the planet based on possibility we set
                    float number = Random.Range(0.0f, 1.0f);
                    int index = -1;
                    for (int i = 0; i < planetList.Count; i++)
                        if (number <= planetPossibility[currentLevel][i])
                        {
                            index = i;
                            break;
                        }
                    GameObject planetPrefab = planetList[index];

                    GeneratePlanet(xcoord, ycoord, planetPrefab);
                }


    }

    void GeneratePlanet(float x, float y, GameObject planetPrefab)
    {
        generatedCells.Add(new Vector2(x, y));

        float planetX = Random.Range(x - (cellDistance / 2 - planetPrefab.transform.localScale.x), x + (cellDistance / 2 - planetPrefab.transform.localScale.x));
        float planetY = Random.Range(y - (cellDistance / 2 - planetPrefab.transform.localScale.y), y + (cellDistance / 2 - planetPrefab.transform.localScale.y));
        GameObject planet = Instantiate(planetPrefab, new Vector3(planetX, planetY, 0), Quaternion.identity);
    }

    public void GameOver()
    {
        IEnumerator GameOverRoutine;
        GameOverRoutine = WaitAndLoadScene("GameOver", 2.0f);
        StartCoroutine(GameOverRoutine);
    }

    public void GameWin()
    {
        IEnumerator GameWinRoutine;
        GameWinRoutine = WaitAndLoadScene("GameWin", 4.0f);
        StartCoroutine(GameWinRoutine);
    }

    private IEnumerator WaitAndLoadScene(string SceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneName);
    }
}
