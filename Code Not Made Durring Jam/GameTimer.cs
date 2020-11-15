using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Dependencies")]
    public Text _timerText = null;

    [Header("Timer Data")]
    [Tooltip("In minutes.")] public float _startTime = 0f;
    [Tooltip("Signifies start of game."), SerializeField, ReadOnly] public bool _begin = false;
    [SerializeField, ReadOnly, Tooltip("In seconds.")] public float _timeLeft = 0f;

    [Header("Instance Data")]
    public static GameTimer _instance;
    [SerializeField, ReadOnly] private bool _gameStart = false;

    void Awake()
    {
        // Set static instance data
        if (!_gameStart)
        {
            _instance = this;
            _gameStart = true;
        }

        // Find timer display if there is one
        if (GameObject.Find("Timer Text") != null)
            _timerText = GameObject.Find("Timer Text").GetComponent<Text>();
    }

    void Start()
    {
        _timeLeft = _startTime * 60;
    }

    void Update()
    {
        if (!_begin)
            return;

        // Always decrement the timer
        DecrementTimer();

        // if the UI display for timer is present then update it
        if (_timerText != null)
            UpdateTimer();
    }

    public void StartTimer() { _begin = true; }
    public void StopTimer() { _begin = false; }

    private void UpdateTimer()
    {
        float minutes = Mathf.Floor((_timeLeft / 60));
        float seconds = Mathf.Ceil(_timeLeft % 60);
        string strMin = minutes.ToString();
        string strSec = seconds.ToString();

        if (minutes < 10)
            strMin = "0" + strMin;
        if (seconds >= 60 || seconds < 1)
        {
            strSec = "00";   
        }

        if (minutes == 0 && _timeLeft < 61 && _timeLeft > 59)
            strSec = "60";

        else if (seconds < 10)
            strSec = "0" + strSec;


        _timerText.text = strMin + " : " + strSec;
    }

    private void DecrementTimer()
    {
        _timeLeft -= Time.deltaTime;
        _timeLeft = Mathf.Clamp(_timeLeft, 0, _startTime * 60);
    }
}
