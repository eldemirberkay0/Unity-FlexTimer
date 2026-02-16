using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace FlexTimer
{
    internal static class PlayerLoopHandler
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InsertTimerLoop()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            var timerSystem = new PlayerLoopSystem
            {
                type = typeof(TimerManager),
                updateDelegate = TimerManager.UpdateTimers,
                subSystemList = null
            };

            var loopWithTimerUpdate = InsertSystem<Update>(currentPlayerLoop, timerSystem);
            PlayerLoop.SetPlayerLoop(loopWithTimerUpdate);
        }

        internal static void RemoveTimerLoop()
        {
            var currentPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            var loopWithoutTimerUpdate = RemoveTimerRecursive(currentPlayerLoop);
            PlayerLoop.SetPlayerLoop(loopWithoutTimerUpdate);
        }

        internal static PlayerLoopSystem InsertSystem<T>(PlayerLoopSystem targetSystem, PlayerLoopSystem systemToInsert) where T : struct
        {
            PlayerLoopSystem newLoop = targetSystem;
            List<PlayerLoopSystem> newSubSystems = new List<PlayerLoopSystem>();

            if (targetSystem.subSystemList != null)
            {
                foreach (var system in targetSystem.subSystemList) { newSubSystems.Add(InsertSystem<T>(system, systemToInsert)); }
            }
            if (targetSystem.type == typeof(T)) { newSubSystems.Add(systemToInsert); }

            newLoop.subSystemList = newSubSystems.ToArray();
            return newLoop;
        }

        internal static PlayerLoopSystem RemoveTimerRecursive(PlayerLoopSystem targetSystem)
        {
            PlayerLoopSystem newLoop = targetSystem;
            List<PlayerLoopSystem> newSubSystems = new List<PlayerLoopSystem>();
            if (targetSystem.subSystemList != null)
            {
                foreach (var system in targetSystem.subSystemList)
                {
                    if (system.updateDelegate == TimerManager.UpdateTimers) { continue; }
                    newSubSystems.Add(RemoveTimerRecursive(system));
                }
            }
            newLoop.subSystemList = newSubSystems.ToArray();
            return newLoop;
        }
    }
}