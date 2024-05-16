using UnityEngine;

public class EventListener : MonoBehaviour
{
    [SerializeField] private ClassItem item;

    public InterfaceReference<IEvent, MonoBehaviour> spawn;

    [SerializeField] private bool EnemyBulletKill;

    private void Start() {
        if (EnemyBulletKill) {
            PlayerEvents.EnemyBulletKill += spawn.Value.Event;
        }
    }
}


