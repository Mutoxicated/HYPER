using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEvent
{
    void Event(Vector3 position, Quaternion rotation);
}
