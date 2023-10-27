using System;
using TMPro;
using UnityEngine;

public enum ClockMode
{
    TimeAdjust,
    AlarmAdjust
}

public class AlarmBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text clockDisplay;
    [SerializeField] TMP_Text alarmDisplay;
    [SerializeField] private AudioSource clockSound;
    [SerializeField] private GameObject pmPoint;
    [SerializeField] private GameObject alarmPoint;

    private DateTime _currentDateTime;
    private DateTime _alarmTime;
    private bool _isAlarmSet = false;
    private bool _isPm = false; // for 12-hour format, consider starting with AM

    private ClockMode _currentMode = ClockMode.TimeAdjust;
    
    private void Start()
    {
        UpdateCurrentTime();
        DisplayTime();
    }

    private void UpdateCurrentTime()
    {
        _currentDateTime = DateTime.Now;
    }

    private void DisplayTime()
    {
        string timeFormat = _isPm ? "hh:mm" : "HH:mm";

        if (_currentMode == ClockMode.AlarmAdjust)
        {
            clockDisplay.gameObject.SetActive(false);
            alarmDisplay.gameObject.SetActive(true);
            DisplayAlarmTime();
        }
        else
        {
            alarmDisplay.gameObject.SetActive(false);
            clockDisplay.gameObject.SetActive(true);
            clockDisplay.text = _currentDateTime.ToString(timeFormat);
        }
    }

    private void DisplayAlarmTime()
    {
        string timeFormat = _isPm ? "hh:mm" : "HH:mm";
        alarmDisplay.text = _alarmTime.ToString(timeFormat);
    }

    public void ToggleSnoozeAlarm()
    {
        if (_isAlarmSet && DateTime.Now.TimeOfDay >= _alarmTime.TimeOfDay)
        {
            // Snooze logic: Delay the alarm for 10 minutes
            clockSound.Stop();
            _alarmTime = DateTime.Now.AddMinutes(10);
        }
        else
        {
            // Enter the AlarmAdjust mode to set the alarm time
            EnterAlarmAdjustMode();
        }
    }

    public void EnterAlarmAdjustMode()
    {
        _currentMode = _currentMode == ClockMode.AlarmAdjust ? ClockMode.TimeAdjust : ClockMode.AlarmAdjust;
    }

    public void AdjustHour()
    {
        if (_currentMode == ClockMode.TimeAdjust)
        {
            _currentDateTime = _currentDateTime.AddHours(1);
            if (_currentDateTime.Hour == 12) _isPm = !_isPm;
            DisplayTime();
        }
        else if (_currentMode == ClockMode.AlarmAdjust)
        {
            _alarmTime = _alarmTime.AddHours(1);
            DisplayAlarmTime();
        }
    }

    public void AdjustMinutes()
    {
        if (_currentMode == ClockMode.TimeAdjust)
        {
            _currentDateTime = _currentDateTime.AddMinutes(1);
            DisplayTime();
        }
        else if (_currentMode == ClockMode.AlarmAdjust)
        {
            _alarmTime = _alarmTime.AddMinutes(1);
            DisplayAlarmTime();
        }
    }

    public void ToggleTimeFormat()
    {
        _isPm = !_isPm;
        pmPoint.SetActive(_isPm);
        DisplayTime();
    }

    public void ToggleAlarm()
    {
        _isAlarmSet = !_isAlarmSet;
        alarmPoint.SetActive(_isAlarmSet);
        clockSound.Stop();
    }

    private void Update()
    {
        // To keep updating the display every second
        if (DateTime.Now.Second != _currentDateTime.Second)
        {
            UpdateCurrentTime();
            DisplayTime();

            // Check if the alarm should go off
            var now = DateTime.Now;
            if (_isAlarmSet && (now.TimeOfDay >= _alarmTime.TimeOfDay))
            {
                // Play alarm sound or any other logic you want
                clockSound.Play();
            }
        }
    }
}