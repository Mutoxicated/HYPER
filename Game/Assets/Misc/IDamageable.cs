using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagebale
{
    void TakeDamage(float intake);
    void TakeDamage(float intake, GameObject sender);
}
