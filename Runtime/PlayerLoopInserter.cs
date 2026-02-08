using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace FlexTimer
{
    internal static class PlayerLoopInserter 
    {
        [RuntimeInitializeOnLoadMethod]
        private static void InsertTimerLoop()
        {
            var defaultLoop = PlayerLoop.GetDefaultPlayerLoop();
            var timerUpdate = new PlayerLoopSystem
            {
                subSystemList = null,
                updateDelegate = TimerManager.UpdateTimers,
                type = typeof(TimerManager)
            };
            var loopWithTimerUpdate = CreateInsertedLoop<Update>(in defaultLoop, timerUpdate);
            PlayerLoop.SetPlayerLoop(loopWithTimerUpdate);
        }

        private static PlayerLoopSystem CreateInsertedLoop<T>(in PlayerLoopSystem loopSystem, PlayerLoopSystem newSystem) where T : struct
        {
            PlayerLoopSystem newPlayerLoop = new()
            {
                loopConditionFunction = loopSystem.loopConditionFunction,
                type = loopSystem.type,
                updateDelegate = loopSystem.updateDelegate,
                updateFunction = loopSystem.updateFunction
            };

            List<PlayerLoopSystem> newSubSystemList = new();
            if (loopSystem.subSystemList != null)
            {
                for (var i = 0; i < loopSystem.subSystemList.Length; i++)
                {
                    newSubSystemList.Add(loopSystem.subSystemList[i]);
                    // If the previously added subsystem is of the type to add after, add the custom system
                    if (loopSystem.subSystemList[i].type == typeof(T)) { newSubSystemList.Add(newSystem); }
                }
            }

            newPlayerLoop.subSystemList = newSubSystemList.ToArray();
            return newPlayerLoop;
        }
    }
}