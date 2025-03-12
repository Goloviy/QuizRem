using System;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance { get; private set; }
   
    private readonly List<Timer> timers = new List<Timer>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Update()
    {
        foreach (var timer in timers)
        {
            timer.Tick();
        }
    }
    
    public Timer CreateTimer(long durationMs, Action onFinished = null)
    {
        var timer = new Timer(durationMs);
        timers.Add(timer);
       
        timer.OnTimerDestroyed += () => RemoveTimer(timer);

        if (onFinished != null)
        {
            timer.OnTimerFinished += onFinished;
        }

        return timer;
    }
   
    private void RemoveTimer(Timer timer)
    {
        if (timers.Contains(timer))
        {
            timers.Remove(timer);
        }
    }
}