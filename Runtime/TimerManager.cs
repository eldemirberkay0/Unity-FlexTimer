using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexTimer
{
    public static class TimerManager
    {
        internal static List<Timer> timers = new List<Timer>();

        internal static void UpdateTimers()
        {
            foreach (Timer timer in timers.ToList()) { timer.Update(); }
        }

        internal static void RegisterTimer(Timer timer) => timers.Add(timer);
        internal static void RemoveTimer(Timer timer) => timers.Remove(timer);

        // Direct manager registration, reduces control on timer but it is more practical
        public static void RegisterEvent(float delay, Action action, int tickCount = 1, bool isLooped = false, bool isScaled = true)
        {
            Timer timer = new Timer(delay, action, tickCount, isLooped, isScaled);
            timer.Start();
        }
    }
}