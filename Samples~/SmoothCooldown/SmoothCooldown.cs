using UnityEngine;
using UnityEngine.UI;

namespace FlexTimer.Samples
{
    public class SmoothCooldown : MonoBehaviour
    {
        [SerializeField] private float cooldownDuration = 5f;
        [SerializeField] Image cooldownBar;

        Timer timer;
        bool isAbilityReady = true;

        void Start()
        {
            timer = new Timer(cooldownDuration, OnFinished: ResetCooldownBar, OnUpdate: UpdateCooldownBar);
        }

        public void StartCooldown()
        {
            if (isAbilityReady)
            {
                timer.Start();
                isAbilityReady = false;
            }
            else { Debug.Log("Cooldown is not finished"); }
        }

        void UpdateCooldownBar()
        {
            cooldownBar.fillAmount = timer.TimeToFinishNormalized;
        }

        void ResetCooldownBar()
        {
            cooldownBar.fillAmount = 1;
            isAbilityReady = true;
            timer.Reset();
        }
    }
}
