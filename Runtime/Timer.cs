using System;
using UnityEngine;

namespace FlexTimer
{
    public class Timer
    {
        public float SecondsToTick => Mathf.Clamp(secondsToTick, 0, tickDuration);
        public float SecondsToFinish => Mathf.Clamp(secondsToTick + (((tickCount - TicksPassed) - 1) * tickDuration), 0, tickDuration * tickCount);
        public float TimeToTickNormalized => Mathf.Clamp(SecondsToTick / tickDuration, 0, 1);
        public float TimeToFinishNormalized => Mathf.Clamp(SecondsToFinish / (tickDuration * tickCount), 0, 1);

        public Action OnUpdate;
        public Action OnTick;
        public Action OnFinished;

        public int TicksPassed { get; private set; } = 0;
        public bool IsScaled { get; private set; } = true;
        public bool IsRunning { get; private set; } = false;
        public bool IsLooped { get; private set; } = false;

        private int tickCount;
        private float tickDuration;
        private float secondsToTick;
        private MonoBehaviour attachedTo;

        /// <summary> Creates a timer with various parameters. </summary>
        /// <param name="tickDuration"> Duration (second) of each tick. </param>
        /// <param name="OnTick"> The action invokes on timer tick. Null by default. </param>
        /// <param name="OnFinished"> The action invokes when no ticks left. Null by default. </param>
        /// <param name="OnUpdate"> The action invokes every timer update. Null by default. </param>
        /// <param name="tickCount"> How many times the timer will tick. 1 by default. </param>
        /// <param name="isLooped"> Ticks forever if true. Overrides tickCount if true. False by default. </param>
        /// <param name="isScaled"> Uses Time.unscaledDeltaTime if false. True by default. </param>
        /// <param name="attachedTo"> MonoBehavior that timer attaches to. If this MonoBehavior is destroyed, timer will cancel itself. </param>
        public Timer(float tickDuration, Action OnTick = null, Action OnFinished = null, Action OnUpdate = null, int tickCount = 1, bool isLooped = false, bool isScaled = true, MonoBehaviour attachedTo = null)
        {
            this.tickDuration = tickDuration;
            this.OnTick += OnTick;
            this.OnFinished += OnFinished;
            this.OnUpdate += OnUpdate;
            this.tickCount = tickCount;
            IsLooped = isLooped;
            IsScaled = isScaled;
            if (attachedTo != null)
            {
                this.attachedTo = attachedTo;
                this.OnUpdate += CheckAttachedObject;
            }
        }

        /// <summary> Registers timer to TimerManager and sets IsRunning true. </summary>
        public void Start()
        {
            if (!TimerManager.timers.Contains(this))
            {
                TicksPassed = 0;
                secondsToTick = tickDuration;
                TimerManager.RegisterTimer(this);
            }
            IsRunning = true;
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
            TicksPassed++;
            if (TicksPassed >= tickCount && !IsLooped)
            {
                Finish();
                return;
            }
            secondsToTick = tickDuration;
        }

        /// <summary> Pauses timer and resets tick count and duration. No auto-start. </summary>
        public void Reset()
        {
            Pause();
            TicksPassed = 0;
            secondsToTick = tickDuration;
        }

        /// <summary> Pauses timer and resets tick count and duration. No auto-start. Sets a new duration. </summary>
        public void Reset(float newTickDuration)
        {
            Pause();
            tickDuration = newTickDuration;
            TicksPassed = 0;
            secondsToTick = tickDuration;
        }

        /// <summary> Resets timer and starts it afterwards. </summary>
        public void Restart()
        {
            Reset();
            Start();
        }

        /// <summary> Resets timer and starts it afterwards. Sets a new duration. </summary>
        public void Restart(float newTickDuration)
        {
            Reset(newTickDuration);
            Start();
        }

        private void Finish()
        {
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnFinished?.Invoke();
        }

        /// <summary> Stops timer and clears it's references. </summary>
        public void Cancel()
        {
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnTick = null;
            OnUpdate = null;
            OnFinished = null;
        }

        private void CheckAttachedObject()
        {
            if (attachedTo == null) { Cancel(); }
        }

        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;
    }
}