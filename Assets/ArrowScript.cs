using System;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public static bool IsAfterMidnight { get; private set; }
    public static float Angle { get; private set; }


    private bool _isDragging;
    private Vector2 _previousUp;
    private float _sumAngle;
    void Start()
    {
        _previousUp = Vector2.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging && ClockScript.IsManualSetModeEnabled)
        {
            Angle = transform.eulerAngles.z;

            Vector3 mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            transform.up = direction;

            Vector3 currentUp = transform.up;
            _sumAngle += Vector2.SignedAngle(_previousUp, currentUp);
            _previousUp = currentUp;

            // если накопленый угол деленый на 360 (например 800 = 2 + 80 град) - это четное число, то день, если нечет значит ночь....
            IsAfterMidnight = (Convert.ToInt32(_sumAngle) / 360) % 2 == 0;
            // попытка починить обход нуля
            if (_sumAngle < 0f) IsAfterMidnight = !IsAfterMidnight;
        }
    }

    private void OnMouseDown()
    {
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
