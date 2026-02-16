using UnityEngine;
using FlexTimer;
using System;

namespace FlexTimer.Samples
{
    public class TickManager : MonoBehaviour
    {
        public static TickManager Instance = null;
        public Action OnCustomFixedUpdate;

        [Tooltip("Tick rate in seconds (e.g. 0.1 = 10Hz)")]
        public float tickRate = 0.1f;

        Timer timer;

        void Awake()
        {
            if (Instance != null) { Destroy(gameObject); }
            else { Instance = this; }
            timer = new Timer(tickRate, () => OnCustomFixedUpdate?.Invoke(), isLooped: true);
        }

        void Start()
        {
            timer.Start();
            // Invokes OnCustomFixedUpdate every {tickRate} seconds
        }
    }
}