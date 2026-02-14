using System;
using UnityEngine;

namespace FlexTimer
{
    public class Timer
    {
        public float SecondsToTick => Mathf.Clamp(secondsToTick, 0, tickDuration);
        public float SecondsToFinish => Mathf.Clamp(secondsToTick + (tickCount - 1) * tickDuration, 0, tickDuration * tickCount);
        public float SecondsToTickNormalized => Mathf.Clamp(secondsToTick / tickDuration, 0, 1);
        public float SecondsToFinishNormalized => Mathf.Clamp((secondsToTick + (tickCount - 1) * tickDuration) / (tickCount * tickDuration), 0, 1);

        public Action OnFinished;
        public Action OnTick;
        public bool IsScaled { get; private set; } = true;
        public bool IsRunning { get; private set; } = false;
        public bool IsLooped { get; private set; } = false;
        
        private int tickCount;
        private float tickDuration;
        private float secondsToTick;

        public Timer(float tickDuration, Action OnTick = null, int tickCount = 1, bool isLooped = false, bool isScaled = true)
        {
            this.tickDuration = tickDuration;
            this.OnTick += OnTick;
            this.tickCount = tickCount;
            IsLooped = isLooped;
            IsScaled = isScaled;
        }

        public void Start()
        {
            if (!TimerManager.timers.Contains(this))
            {
                secondsToTick = tickDuration;
                TimerManager.RegisterTimer(this);
                IsRunning = true;
            }
        }

        internal void Update()
        {
            if (IsRunning)
            {
                secondsToTick -= IsScaled ? Time.deltaTime : Time.unscaledDeltaTime;
                if (secondsToTick <= 0) { Tick(); }
            }
        }
        
        private void Tick()
        {
            OnTick?.Invoke();
            tickCount--;
            if (tickCount <= 0 && !IsLooped)  
            { 
                Finish(); 
                return;
            }
            secondsToTick = tickDuration;
        }

        private  void Finish()
        {
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnTick = null;
            OnFinished?.Invoke();
            OnFinished = null;
            TimerManager.RemoveTimer(this);
        }

        public void Pause() => IsRunning = false; 
        public void Resume() => IsRunning = true;
    }
}