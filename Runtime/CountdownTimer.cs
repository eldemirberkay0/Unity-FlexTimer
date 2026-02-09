using UnityEngine;
using FlexTimer;
using System;

namespace FlexTimer
{
    public class CountdownTimer : Timer
    {
        public float RemainingSeconds => Math.Clamp(remainingSeconds, 0, duration);
        public float RemainingNormalized => Math.Clamp(remainingSeconds / duration, 0, 1);
        public float ProgressNormalized => Mathf.Clamp((duration - remainingSeconds) / duration, 0, 1);
        public float ProgressSeconds => Mathf.Clamp(duration - remainingSeconds, 0, duration);
        private float duration;
        private float remainingSeconds;

        public CountdownTimer(float duration, Action OnFinished = null, bool isScaled = true)
        {
            this.duration = duration;
            this.OnFinished += OnFinished;
            IsScaled = isScaled;
        }

        internal override void Update()
        {
            if (IsRunning)
            {
                remainingSeconds -= IsScaled ? Time.deltaTime : Time.unscaledDeltaTime;
                if (remainingSeconds <= 0) { Finish(); }
            }
        }

        public override void Start()
        {
            remainingSeconds = duration;
            IsRunning = true;
            TimerManager.RegisterTimer(this);
        }

        internal override void Finish()
        {
            OnFinished?.Invoke();
            TimerManager.RemoveTimer(this);
            IsRunning = false;
            OnFinished = null;
        }
    }
}
