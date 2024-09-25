using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;
using TMPro;

public class RequestScript : MonoBehaviour
{
    public static int[] TimeFromNetOrInput { get; private set; } = { 0, 0, 0 };
    public static event Action TimeHasUpdated;
    [SerializeField] private TMP_InputField _HoursInputField, _MinutesInputField, _SecondsInputField;
    [SerializeField] private string _uri = "https://timeapi.io/api/time/current/zone?timeZone=Europe%2FMoscow";
    private const float SYNCDELAY = 3600f;

    public class TimeInfo
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int seconds;
        public int milliSeconds;
        public string dateTime;
        public string date;
        public string time;
        public string timeZone;
        public string dayOfWeek;
        public bool dstActive;
    }

    public void GetTimeFromInputFields()
    {
        TimeFromNetOrInput[0] = TimeTextScript.SafeConvertToInt(_HoursInputField.text);
        TimeFromNetOrInput[1] = TimeTextScript.SafeConvertToInt(_MinutesInputField.text);
        TimeFromNetOrInput[2] = TimeTextScript.SafeConvertToInt(_SecondsInputField.text);
        TimeHasUpdated?.Invoke();
    }

    public void StartRequestCoroutine()
    {
        StartCoroutine(GetRequest(_uri));
    }
    void Start()
    {
        InvokeRepeating(nameof(StartRequestCoroutine), 0f, SYNCDELAY);
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("request error :(");
                    break;
                case UnityWebRequest.Result.Success:
                    TimeInfo timeinfo = JsonUtility.FromJson<TimeInfo>(webRequest.downloadHandler.text);
                    TimeFromNetOrInput[0] = Convert.ToInt16(timeinfo.hour);
                    TimeFromNetOrInput[1] = Convert.ToInt16(timeinfo.minute);
                    TimeFromNetOrInput[2] = Convert.ToInt16(timeinfo.seconds);
                    TimeHasUpdated?.Invoke();
                    break;
            }
        }
    }
}
