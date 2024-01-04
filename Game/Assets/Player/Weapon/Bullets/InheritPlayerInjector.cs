using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InheritPlayerInjector : MonoBehaviour
{
    [SerializeField] private Injector injector;

    void Start()
    {
        injector.InheritInjector(PlayerInfo.GetPH().immuneSystem.injector);
    }
}
