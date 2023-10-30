using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeHealth(float intake, float shieldIntake);
    float TakeDamage(float intake, Stats sender, ref float shieldOut, float arbParam, int arbParam2);
    float TakeDamage(float intake, Stats sender, float arbParam, int arbParam2);
    void TakeInjector(Injector injector, bool cacheInstances);
    void RevertInjector(Injector refInjector);
}