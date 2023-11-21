using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    { get { return instance; } }

    [SerializeField]
    private GameObject startGameText;
    private bool startGame;

    [SerializeField]
    private GameObject gameOverScreen;

    [SerializeField]
    private float maxTime;
    private float timer;

    private int rand;
    private int prevRand = 0;

    private ObjectPoolManager poolManager;

    [SerializeField]
    private Text timerText;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip levelMusic;

    private enum ObjectiveType {StartGame = -2, GameEnd = -1, MoveToZone = 0, EvadeMeteors, MovingTriangles }
    private ObjectiveType currentObjective;

    public bool inZone;

    private int levelToSpawnAlien;
    private int currentLevelSpawnDelta;

    [SerializeField]
    private GameObject healthUi;
    [SerializeField]
    private GameObject scoreUi;
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text endScoreText;
    [SerializeField]
    private Text endHighScoreText;
    [SerializeField]
    private GameObject newHighScore;
    private int score;
    private int highScore;



    [Header("Move To Zone")]
    [SerializeField]
    private GameObject successZone;



    private void Awake()
    {
        instance = this;
        gameOverScreen.SetActive(false);
        startGame = false;
        currentObjective = ObjectiveType.StartGame;
    }
    void Start()
    {
        poolManager = ObjectPoolManager.Instance;
        highScore = 0;
        StartGame();
    }

    
    void Update()
    {
        ManagerTimeText();
        if(currentObjective == ObjectiveType.StartGame)
        {
            StartingGame();
        }
        else if (currentObjective == ObjectiveType.GameEnd)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartGame();    
            }
        }
        else
        {
            healthUi.SetActive(true);
            scoreUi.SetActive(true);
            scoreText.text = score.ToString();

            if(currentLevelSpawnDelta == levelToSpawnAlien) 
            {
                SpawnMarsian();
            }
        }
    }

    void SpawnMarsian()
    {
        levelToSpawnAlien = UnityEngine.Random.Range(5, 7); 
        currentLevelSpawnDelta = 0;
        GameObject marsian = poolManager.GetPooledObject(ObjectPoolManager.PoolTypes.Marsian);

        int randomX;
        int randomY;
        do
        {
            randomX = UnityEngine.Random.Range(-24, 24);
        } while (Mathf.Abs(randomX) < 24 - 2);

        do
        {
            randomY = UnityEngine.Random.Range(-12, 12);
        } while (Mathf.Abs(randomY) < 12 - 2);

        marsian.transform.position = new Vector2(randomX, randomY);
        marsian.SetActive(true);
    }

    void ManagerTimeText()
    {
        if(startGame)
        {
            timerText.text = ((int)timer).ToString();

            timer -= Time.deltaTime;

            if (timer < 1)
            {
                timerText.text = "";
            }
            if (timer < 0)
            {
                NewCycle();
            }
        }
        else
        {
            timerText.text = "";
        }
    }

    public void EndGame()
    {
        startGame = false;
        audioSource.Stop();
        currentObjective = ObjectiveType.GameEnd;
        ClearCycle();
        endScoreText.text = score.ToString();
        if(score > highScore)
        {
            highScore = score;
            newHighScore.SetActive(true);
        }
        else
        {
            newHighScore.SetActive(false);
        }
        endHighScoreText.text = highScore.ToString();
        gameOverScreen.SetActive(true);
    }

    void StartGame()
    {
        startGameText.SetActive(true);
        gameOverScreen.SetActive(false);
        levelToSpawnAlien = 3; 
        currentLevelSpawnDelta = 0;
        score = -100;
        healthUi.SetActive(false);  
        scoreUi.SetActive(false);   
        audioSource.clip = levelMusic;
        audioSource.Play();
        currentObjective = ObjectiveType.StartGame;
        PlayerController.Instance.NewGame();
        ManageCycle();
    }

    void StartingGame()
    {
        if(inZone) 
        {
            startGame = true;
        }
        else
        {
            startGame = false;
            timer = maxTime + .99f;
        }
    }

    void SpawnZone(int xRight, int yTop)
    {
        int randomX;
        int randomY;
        do
        {
            randomX = UnityEngine.Random.Range(-xRight, xRight);
        } while (Mathf.Abs(randomX) < xRight - 2);

        do
        {
            randomY = UnityEngine.Random.Range(-yTop, yTop);
        } while (Mathf.Abs(randomY) < yTop - 2);

        successZone.transform.position = new Vector2(randomX, randomY);
        successZone.gameObject.SetActive(true);
    }

    void NewCycle()
    {
        if(currentObjective == ObjectiveType.MoveToZone && !inZone) 
        {
            PlayerController.Instance.DealDamage();
        }
        ClearCycle();
        do
        {
            rand = UnityEngine.Random.Range(0, 3);
        }
        while (rand == prevRand);
        prevRand = rand;
        currentLevelSpawnDelta++;
        currentObjective = (ObjectiveType)Enum.ToObject(typeof(ObjectiveType), rand);
        timer = maxTime + .99f;
        PlayerController.Instance.NewCycle();
        inZone = false;
        startGameText.SetActive(false);
        AddScore(100);
        ManageCycle();
    }

    void ManageCycle()
    {
        switch(currentObjective) 
        {
            case ObjectiveType.StartGame:
                SpawnZone(21, 6);
                break;
            case ObjectiveType.MoveToZone:
                MoveToZone();
                break;
            case ObjectiveType.EvadeMeteors:
                StartCoroutine(EvadeMeteors());
                break;
            case ObjectiveType.MovingTriangles:
                StartCoroutine(MovingTriangles());
                break;
        }

        
    }
    void ClearCycle()
    {
        poolManager.DeactivateAll();

        /////////////////////////////////////////////////////////////////////////////
        successZone.gameObject.SetActive(false);

    }

    public void AddScore(int _toAdd)
    {
        score += _toAdd;
        if(score > 9999)
        {
            score = 9999;
        }
    }

    void MoveToZone()
    {

        foreach (GameObject rock in poolManager.GetAllPooledObjects(ObjectPoolManager.PoolTypes.SMALL_ROCKS))
        {
            rock.transform.position = new Vector3(UnityEngine.Random.Range(-23, 23), UnityEngine.Random.Range(-13, 13));
            rock.SetActive(true);
        }
        foreach (GameObject rock in poolManager.GetAllPooledObjects(ObjectPoolManager.PoolTypes.MEDIUM_ROCKS))
        {
            rock.transform.position = new Vector3(UnityEngine.Random.Range(-23, 23), UnityEngine.Random.Range(-13, 13));
            rock.SetActive(true);
        }
        foreach (GameObject rock in poolManager.GetAllPooledObjects(ObjectPoolManager.PoolTypes.BIG_ROCKS))
        {
            rock.transform.position = new Vector3(UnityEngine.Random.Range(-23, 23), UnityEngine.Random.Range(-13, 13));
            rock.SetActive(true);
        }

        SpawnZone(21, 11);

    }

    IEnumerator EvadeMeteors()
    {
        do
        {
            GameObject meteor = poolManager.GetPooledObject(ObjectPoolManager.PoolTypes.Random_METEORS);
            meteor.transform.position = new Vector2(UnityEngine.Random.Range(-30, 30), 20);
            meteor.SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);
        } while (currentObjective == ObjectiveType.EvadeMeteors);
        
    }

    IEnumerator MovingTriangles()
    {
        int rand;
        do
        {
            GameObject triangle = poolManager.GetPooledObject(ObjectPoolManager.PoolTypes.Random_Triangles);
            rand = UnityEngine.Random.Range(0, 4);
            if(rand == 0)
            {
                triangle.transform.position = new Vector2(UnityEngine.Random.Range(-24, 24), 20);
                triangle.transform.rotation = Quaternion.Euler(0,0,180);
            }
            else if (rand == 1)
            {
                triangle.transform.position = new Vector2(UnityEngine.Random.Range(-24, 24), -20);
                triangle.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (rand == 2)
            {
                triangle.transform.position = new Vector2(30, UnityEngine.Random.Range(-14, 14));
                triangle.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (rand == 3)
            {
                triangle.transform.position = new Vector2(-30, UnityEngine.Random.Range(-14, 14));
                triangle.transform.rotation = Quaternion.Euler(0, 0, 270);
            }
            triangle.SetActive(true);
            yield return new WaitForSecondsRealtime(.3f);
        } while (currentObjective == ObjectiveType.MovingTriangles);

    }
}
