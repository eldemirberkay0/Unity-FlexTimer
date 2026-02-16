using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace FlexTimer
{
    internal static class PlayerLoopUtils
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InsertTimerLoop()
        {
            var defaultLoop = PlayerLoop.GetDefaultPlayerLoop();

            var timerUpdate = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = TimerManager.UpdateTimers,
                type = typeof(TimerManager)
            };

            PlayerLoopSystem newPlayerLoop = new()
            {
                loopConditionFunction = defaultLoop.loopConditionFunction,
                type = defaultLoop.type,
                updateDelegate = defaultLoop.updateDelegate,
                updateFunction = defaultLoop.updateFunction
            };

            List<PlayerLoopSystem> newSubSystemList = new();
            if (defaultLoop.subSystemList != null)
            {
                for (int i = 0; i < defaultLoop.subSystemList.Length; i++)
                {
                    newSubSystemList.Add(defaultLoop.subSystemList[i]);
                    // If the previously added subsystem is of the type to add after, add the custom system
                    if (defaultLoop.subSystemList[i].type == typeof(Update)) { newSubSystemList.Add(timerUpdate); }
                }
            }

            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            PlayerLoop.SetPlayerLoop(newPlayerLoop);
        }

        internal static void RemoveTimerLoop()
        {
            var currentLoop = PlayerLoop.GetCurrentPlayerLoop();

            PlayerLoopSystem newPlayerLoop = new()
            {
                loopConditionFunction = currentLoop.loopConditionFunction,
                type = currentLoop.type,
                updateDelegate = currentLoop.updateDelegate,
                updateFunction = currentLoop.updateFunction
            };

            List<PlayerLoopSystem> newSubSystemList = new();
            if (currentLoop.subSystemList != null)
            {
                for (int i = 0; i < currentLoop.subSystemList.Length; i++)
                {
                    if (currentLoop.subSystemList[i].type == typeof(TimerManager)) { continue; }
                    newSubSystemList.Add(currentLoop.subSystemList[i]);
                }
            }

            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            PlayerLoop.SetPlayerLoop(newPlayerLoop);
        }
    }
}