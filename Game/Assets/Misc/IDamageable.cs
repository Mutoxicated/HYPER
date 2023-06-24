using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagebale
{
    void TakeDamage(int intake);
    void TakeDamage(int intake, GameObject sender);
}
