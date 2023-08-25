using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float intake, GameObject sender, float arbParam, int arbParam2);
    void TakeInjector(Injector injector);
}