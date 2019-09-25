// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAI
{
    public class PAIState : MonoBehaviour
    {
        public virtual void StateActivated() { }
        public virtual void StateDeactivated() { }
        public virtual void Execute() { }
    }
}