using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace FlexTimer
{
    public static class TimerManager
    {
        private static List<Timer> timers = new List<Timer>();

        internal static void UpdateTimers()
        {
            /*
            for (int i = 0; i < timers.Count; i++)
            {
                timers[i].Tick();
                // if (timers[i] == null) { continue; }
            }
            */
            foreach (Timer timer in timers.ToList()) { timer.Tick(); }
        }

        internal static void RegisterTimer(Timer timer) => timers.Add(timer);
        internal static void RemoveTimer(Timer timer) => timers.Remove(timer);
    }   
}