using System;
using System.Collections.Generic;
using UnityEngine;

namespace FlexTimer
{
    public static class TimerManager
    {
        internal static List<Timer> timers = new List<Timer>();

        internal static void UpdateTimers()
        {
            for (int i = timers.Count - 1; i >= 0; i--) { timers[i].Update(); }
        }

        internal static void RegisterTimer(Timer timer) => timers.Add(timer);
        internal static void RemoveTimer(Timer timer) => timers.Remove(timer);

        /// <summary> Creates a timer with an event attached to it and starts timer directly. </summary>
        /// <param name="duration"> Duration (second) of each tick. </param>
        /// <param name="action"> Invokes on timer tick. </param>
        /// <param name="isScaled"> Uses Time.unscaledDeltaTime if false. True by default. </param>
        public static void RegisterEvent(float duration, Action action, bool isScaled = true)
        {
            Timer timer = new Timer(duration, action, null, null, 1, false, isScaled);
            timer.Start();
        }

        /// <summary> Removes all timers and clears their delegates. Suggested to use while changing scene. </summary>
        public static void RemoveAllTimers()
        {
            if (timers == null) { return; }
            for (int i = timers.Count - 1; i >= 0; i--)
            {
                timers[i].Pause();
                timers[i].OnFinished = null;
                timers[i].OnTick = null;
                timers[i].OnUpdate = null;
            }
            timers.Clear();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void EnsureClearOnQuit()
        {
            Application.quitting -= RemoveAllTimers;
            Application.quitting += RemoveAllTimers;
            Application.quitting -= PlayerLoopUtils.RemoveTimerLoop;
            Application.quitting += PlayerLoopUtils.RemoveTimerLoop;
        }
    }
}