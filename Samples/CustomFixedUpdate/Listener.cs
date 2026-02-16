using System;
using UnityEngine;

namespace FlexTimer.Samples
{
    public class Listener : MonoBehaviour
    {
        void Start()
        {
            // Link CustomFixedUpdate() to TickManager's OnCustomFixedUpdate 
            TickManager.Instance.OnCustomFixedUpdate += CustomFixedUpdate;
        }

        void CustomFixedUpdate()
        {
            Debug.Log("This text will show up every " + TickManager.Instance.tickRate.ToString() + " seconds");
        }
    }
}