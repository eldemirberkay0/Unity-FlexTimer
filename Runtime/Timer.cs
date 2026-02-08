using System;
using UnityEngine;

namespace FlexTimer
{
    public class Timer
    {
        public Action OnFinished;
        public bool IsRunning { get; private set; } = false;
        protected float duration;
        protected float timePassed;

        public Timer(float duration)
        {
            this.duration = duration;
        }

        public Timer(float duration, Action OnFinished)
        {
            this.duration = duration;
            this.OnFinished += OnFinished;
        }

        internal protected virtual void Tick()
        {
            if (IsRunning) { timePassed += Time.deltaTime; }
            if (timePassed >= duration) { Stop(); }
        }

        public void Start()
        {
            TimerManager.RegisterTimer(this);
            IsRunning = true;
        }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;

        public void Stop()
        {
            OnFinished?.Invoke();
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnFinished = null;
        }
    }
}