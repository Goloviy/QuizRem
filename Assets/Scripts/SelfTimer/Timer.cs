using System;

namespace SelfTimer
{
    public class Timer : IDisposable
    {
        private long durationMs;
        private long remainingTimeMs;
        private long startTimeMs;
        private long pausedTimeMs;
        private bool onPause;

        public event Action OnTimerStarted;
        public event Action OnTimerPaused;
        public event Action OnTimerResumed;
        public event Action OnTimerFinished;
        public event Action OnTimerDestroyed;
        public event Action<long> OnTimerTick;

        public long RemainingTimeMs => remainingTimeMs;

        public Timer(long _durationMs)
        {
            durationMs = _durationMs;
            remainingTimeMs = _durationMs;
            onPause = true;
        }

        public void Start()
        {
            if (!onPause) return;

            startTimeMs = GetCurrentTimeMs();
            onPause = false;
            OnTimerStarted?.Invoke();
        }

        public void Pause()
        {
            if (onPause) return;

            pausedTimeMs = GetCurrentTimeMs();
            onPause = true;
            OnTimerPaused?.Invoke();
        }

        public void Resume()
        {
            if (!onPause) return;

            startTimeMs += GetCurrentTimeMs() - pausedTimeMs;
            onPause = false;
            OnTimerResumed?.Invoke();
        }

        public void Stop()
        {
            onPause = true;
        }

        public void Reset()
        {
            remainingTimeMs = durationMs;
            startTimeMs = GetCurrentTimeMs();
            onPause = true;
        }

        public void Tick()
        {
            if (onPause) return;

            long currentTimeMs = GetCurrentTimeMs();
            long elapsedTimeMs = currentTimeMs - startTimeMs;


            remainingTimeMs = Math.Max(durationMs - elapsedTimeMs, 0);
            OnTimerTick?.Invoke(remainingTimeMs);


            if (remainingTimeMs <= 0)
            {
                onPause = true;
                OnTimerFinished?.Invoke();
                Dispose();
            }
        }

        private long GetCurrentTimeMs()
        {
            return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public void Dispose()
        {
            OnTimerDestroyed?.Invoke();

            OnTimerStarted = null;
            OnTimerPaused = null;
            OnTimerResumed = null;
            OnTimerFinished = null;
            OnTimerDestroyed = null;
        }
    }
}