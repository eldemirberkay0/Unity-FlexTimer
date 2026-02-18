using UnityEngine;

namespace FlexTimer.Samples
{
    public class AttachedTimer : MonoBehaviour
    {
        private float enemyHealth = 10;
        Timer timer;

        void Start()
        {
            // Create a timer attached to this MonoBehaviour so that if MonoBehaviour is destroyed, timer cancels itself.
            timer = new Timer(2, TakeDamage, tickCount: 3, attachedTo: this);
            timer.Start();
        }

        void TakeDamage()
        {
            enemyHealth -= 5;
            Debug.Log("Enemy took damage! Current health: " + enemyHealth.ToString());
            // Enemy dies after 2 tick, and if timer is not attached it will try to access enemy that results with an error
            if (enemyHealth <= 0)
            {
                Debug.Log("Enemy died.");
                Destroy(gameObject);
            }
        }
    }
}