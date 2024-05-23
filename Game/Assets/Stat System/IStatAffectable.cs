using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Modification {
    RELATIVE,
    ABSOLUTE
}

public interface IStatAffectable {
    void ModifyNumerical(Numerical name, float value, Modification modType);
    void ModifyConditional(Conditional name, bool value);
}
