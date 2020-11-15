using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ArenaManager : MonoBehaviour
{
    [Header("Game Mode Dependencies")]
    public List<EnemyData> enemyTypes = new List<EnemyData>();
    public GameObject gameOverScreen = null;
    public GameObject enemyObject = null;
    public GameObject entityParent = null;
    public PlayerController _player = null;
    public TextMeshProUGUI timerText = null;
    public TextMeshProUGUI scoreText = null;
    [Tooltip("In minutes.")] public float maxTime = 0f;
    
    [Header("Game Mode Data")]
    public int maxEntities = 0;
    public List<GameObject> spawnPoints = new List<GameObject>();
    public float minSpawnRate = 0f;
    public float maxSpawnRate = 0f;

    [Header("Debug Data")]
    [ReadOnly] public float timeSinceLastSpawn = 0f;
    [ReadOnly] public float spawnCheck = 0f;
    [ReadOnly, Tooltip("In seconds.")] public float currTime = 0f;
    [ReadOnly] public int currEntities = 0;
    [SerializeField, ReadOnly] private bool _start = false;

    public static ArenaManager _inst;


    void Awake()
    {
        // Confirm singleton instance is active
        if (_inst == null)
            _inst = this;
        else if (_inst != this)
            GameObject.Destroy(this);

        if (_player == null && GameObject.Find("Player") != null)
            _player = GameObject.Find("Player").GetComponent<PlayerController>();
        
        if (timerText == null && GameObject.Find("Game Timer") != null)
            timerText = GameObject.Find("Game Timer").GetComponent<TextMeshProUGUI>();
        
        if (scoreText == null && GameObject.Find("Score Text") != null)
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // currTime = 60 * maxTime;
        _start = true;
        _player.hasControl = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_start)
        {
            // Game Over conditions
            if (_player._currHealth <= 0)
                GameOver();
            else
            {
                // TODO - Enemy spawning...
                EnemySpawnHandler();

                // Update the timer...
                currTime += Time.deltaTime;
                UpdateTimer();
                UpdateScore();
            }
        }
    }


    private void UpdateScore()
    {
        scoreText.text = "Score: " + _player.currPlayerScore;
    }

    private void GameOver()
    {
        Debug.Log("Game Over...");
        _player.hasControl = false;
        gameOverScreen.SetActive(true);

        TextMeshProUGUI temp = gameOverScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        temp.text = "Time lasted: " + timerText.text + "\n\n" + "Score achieved: " + _player.currPlayerScore + "\n\n" + "Final Score: " + ((int)Mathf.Ceil(currTime) + _player.currPlayerScore);
    }

    private void EnemySpawnHandler()
    {
        if ((Time.time - timeSinceLastSpawn) >= spawnCheck && !(currEntities >= maxEntities))
        {

            Vector3 spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position;

            EnemyData chosenData = enemyTypes[Random.Range(0, enemyTypes.Count)];

            GameObject refr = Instantiate(enemyObject, spawnPoint, Quaternion.identity, entityParent.transform);
            AIHandler refrData = refr.GetComponent<AIHandler>();

            refrData.sr.sprite = chosenData.enemySprite;
            refrData._type = chosenData.enemyType;
            refrData.target = _player.transform;
            refrData.maxHealth = chosenData.MaxHealth;
            refrData.moveSpeed = chosenData.MovementSpeed;
            refrData.minDistance = chosenData.MinFollowDistance;
            refrData.damageAmount = chosenData.Damage;
            refrData.damageTickRate = chosenData.DamageRate;
            refrData._dataContainer = chosenData;

            // refrData._wake = true;

            spawnCheck = Random.Range(minSpawnRate, maxSpawnRate);
            currEntities = Mathf.Clamp((currEntities + 1), 0, maxEntities);
            timeSinceLastSpawn = Time.time;
        }
    }


    /* --------------------------------------------------------------------------------------------
        The bellow function "UpdateTimer()" was not code I implemented durring the Jam period.
        This was something I wrote for a previous project that I copy/pasted over.
    */
    private void UpdateTimer()
    {
        float minutes = Mathf.Floor((currTime / 60));
        float seconds = Mathf.Ceil(currTime % 60);
        string strMin = minutes.ToString();
        string strSec = seconds.ToString();

        if (minutes < 10)
            strMin = "0" + strMin;
        if (seconds >= 60 || seconds < 1)
        {
            strSec = "00";   
        }

        if (minutes == 0 && currTime < 61 && currTime > 59)
            strSec = "60";

        else if (seconds < 10)
            strSec = "0" + strSec;


        timerText.text = strMin + " : " + strSec;
    }
    // --------------------------------------------------------------------------------------------
}
