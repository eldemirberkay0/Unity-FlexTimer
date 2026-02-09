using System;
using UnityEngine;

namespace FlexTimer
{
    public abstract class Timer
    {
        public Action OnFinished;
        public bool IsScaled { get; protected set; } = true;
        public bool IsRunning { get; protected set; } = false;
        internal abstract void Update();
        internal abstract void Finish();
        public abstract void Start();
        public void Pause() => IsRunning = false; 
        public void Resume() => IsRunning = true;
    }
}