using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light sun;
    [SerializeField] private Gradient lightColor;
    [SerializeField] private const float cycleLength = 500; // Ändere die cycleLength hier
#if UNITY_EDITOR
    [Help("Verändere die Tageslänge im Script, der Variablenname ist 'cycleLength'\nDefault cycleLength = 500\nDefault nightTime = 230.", UnityEditor.MessageType.Info)]
#endif
    [SerializeField] private bool setNightTimeManually = true;
    [SerializeField, ConditionalHide("setNightTimeManually")] private float nightTime = 230;
    [SerializeField] private bool dynamicTime = true;

    public bool isNight;
    [Range(0, cycleLength)] public float currentTimeOfDay = 0;
    
    public UnityEvent onDay;
    public UnityEvent onNight;
    public UnityEvent onDayPassed;

    
    private Transform sunParent;
    private int days;
    private float evaluateNum = 0.04f;
    private float evaluateRotNum = 0.72f;
    public static bool GlobalIsNight;


    #region GETTER_SETTER
    public int Days
    {
        get { return days; }
    }

    public bool IsNight
    {
        set { isNight = value; }
        get { return isNight; }
    }
    #endregion

    public void OnDay()
    {
        isNight = false;
        onDay?.Invoke();
    }

    public void OnNight()
    {
        isNight = true;
        onNight?.Invoke();
    }

    public void OnDayPassed()
    {
        onDayPassed?.Invoke();
    }

    private void Start()
    {
        evaluateNum = 1 / cycleLength;
        evaluateRotNum = 360 / cycleLength;

        if (!setNightTimeManually)
            nightTime = (cycleLength / 2) - (cycleLength * 0.1f);

        sunParent = sun.transform.parent;
    }


    private void Update()
    {
        GlobalIsNight = isNight;
        if (currentTimeOfDay > cycleLength)
        {
            currentTimeOfDay = 0;
        }

        if ((int)currentTimeOfDay == nightTime)
        {
            OnNight();
        }

        if ((currentTimeOfDay + 0.5f) >= cycleLength)
        {
            OnDay();
            OnDayPassed();
            isNight = false;
            days += 1;
        }

        sun.color = lightColor.Evaluate(currentTimeOfDay * evaluateNum);
        sunParent.localRotation = Quaternion.Euler(0, sun.gameObject.transform.localRotation.z + currentTimeOfDay * evaluateRotNum, 0);

        if (!dynamicTime) return;
        currentTimeOfDay += Time.deltaTime;
    }
}
