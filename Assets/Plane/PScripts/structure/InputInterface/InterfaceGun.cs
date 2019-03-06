using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void DelegateMethod();
public interface InterfaceGun {

    event DelegateMethod Fire;
    event DelegateMethod GunBegin;
    event DelegateMethod GunEnd;

}
