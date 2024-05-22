using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private ClassItem ci;

    public InterfaceReference<IEvent, MonoBehaviour> spawn;

    [SerializeField] private bool EnemyBulletKill;

    private void Start() {
        ci.state += State;
    }

    private void OnDestroy() {
        ci.state -= State;
    }

    private void State(bool state) {
        if (state) {
            if (EnemyBulletKill) {
                PlayerEvents.EnemyBulletKill += spawn.Value.Event;
            }
        }else {
            if (EnemyBulletKill) {
                PlayerEvents.EnemyBulletKill -= spawn.Value.Event;
            }
        }
    }
}


