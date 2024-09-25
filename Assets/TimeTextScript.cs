using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeTextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    string ToFormat(float value)
    {
        if(value < 10f)
        {
            return "0" + Convert.ToString(value);
        }
        return Convert.ToString(value);
    }

    public static int SafeConvertToInt(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return 0;
        }
        return Mathf.Abs(Convert.ToInt32(value));
    }
    void Update()
    {
        Vector3 time = ClockScript.GetTimeFormClock();
        _textMesh.text = $"{ToFormat(time.x)}:{ToFormat(time.y)}:{ToFormat(time.z)}";
    }
}
