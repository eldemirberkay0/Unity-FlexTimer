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
        public int TicksRemaining => tickCount;

        public Action OnUpdate;
        public Action OnTick;
        public Action OnFinished;

        public bool IsScaled { get; private set; } = true;
        public bool IsRunning { get; private set; } = false;
        public bool IsLooped { get; private set; } = false;
        
        private int tickCount;
        private float tickDuration;
        private float secondsToTick;

        /// <summary> Creates a timer with various parameters. </summary>
        /// <param name="tickDuration"> Duration (second) of each tick. </param>
        /// <param name="OnTick"> The action invokes on timer tick. Null by default. </param>
        /// <param name="OnFinished"> The action invokes when no ticks left. Null by default. </param>
        /// <param name="OnUpdate"> The action invokes every timer update. Null by default. </param>
        /// <param name="tickCount"> How many times the timer will tick. 1 by default. </param>
        /// <param name="isLooped"> Ticks forever if true. Overrides tickCount if true. False by default. </param>
        /// <param name="isScaled"> Uses Time.unscaledDeltaTime if false. True by default. </param>
        public Timer(float tickDuration, Action OnTick = null, Action OnFinished = null, Action OnUpdate = null, int tickCount = 1, bool isLooped = false, bool isScaled = true)
        {
            this.tickDuration = tickDuration;
            this.OnTick += OnTick;
            this.OnFinished += OnFinished;
            this.OnUpdate += OnUpdate;
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
                OnUpdate?.Invoke();
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
            OnUpdate = null;
            OnFinished?.Invoke();
            OnFinished = null;
        }
        
        public void Cancel()
        {
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnTick = null;
            OnUpdate = null;
            OnFinished = null;
        }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;
    }
}