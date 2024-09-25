using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClockScript : MonoBehaviour
{
    public static bool IsManualSetModeEnabled { get; private set; }

    private static float _hours = 0f;
    private static float _minutes = 0f;
    private static float _seconds = 0f;

    private float _mArrowAngle = 0f;
    private float _hArrowAngle = 0f;
    private float _sArrowAngle = 0f;
    private float _manualHoursAngle;

    [SerializeField] private GameObject _hArrow, _mArrow, _sArrow;

    public static Vector3 GetTimeFormClock()  
    {
        return new Vector3(Mathf.FloorToInt(_hours), Mathf.FloorToInt(_minutes), Mathf.FloorToInt(_seconds));
    }

    public static void ToggleManualSetMode()
    {
        IsManualSetModeEnabled = !IsManualSetModeEnabled;
    }

    void Start()
    {
        RequestScript.TimeHasUpdated += UpdateClock;
    }

    public static void UpdateClock()
    {
        print("update clock!");
        _seconds = RequestScript.TimeFromNetOrInput[2];
        _minutes = RequestScript.TimeFromNetOrInput[1] + _seconds / 60f;
        _hours = RequestScript.TimeFromNetOrInput[0] + _minutes / 60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsManualSetModeEnabled)
        {
            _manualHoursAngle = ArrowScript.Angle;
            float twelveOrTwentyFour = 12 + 12 * Convert.ToInt32(ArrowScript.IsAfterMidnight);
            _hours = twelveOrTwentyFour - _manualHoursAngle / 30f;
            _minutes = (_hours * 60f) % 60f;
            _mArrowAngle = (-1f) * _minutes * 6f;
            _mArrow.transform.rotation = Quaternion.Euler(0f, 0f, _mArrowAngle);
        }
        else
        {
            _seconds += Time.deltaTime;
            _seconds %= 60;
            _minutes += Time.deltaTime / 60f;
            _minutes %= 60;
            _hours += Time.deltaTime / (60f * 60f);
            _hours %= 24;

            _hArrowAngle = (-1f) * (_hours % 12) * 30f;
            _mArrowAngle = (-1f) * _minutes * 6f;
            _sArrowAngle = (-1f) * _seconds * 6f;

            _hArrow.transform.rotation = Quaternion.Euler(0f, 0f, _hArrowAngle);
            _mArrow.transform.rotation = Quaternion.Euler(0f, 0f, _mArrowAngle);
            _sArrow.transform.rotation = Quaternion.Euler(0, 0, _sArrowAngle);
        }
    }
}
