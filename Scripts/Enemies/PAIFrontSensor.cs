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
    public class PAIFrontSensor : MonoBehaviour
    {

        private List<string> _importantTags = new List<string> { "EnemyFrontSensor", "Player" };

        private string _lastTag = "";
        private bool _hasHitted = false;
        private bool _hasExited = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_importantTags.Contains(other.tag))
            {
                _lastTag = other.tag;
                _hasHitted = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_importantTags.Contains(other.tag))
            {
                _lastTag = other.tag;
                _hasExited = true;
            }
        }

        public bool Hitted(out string otherTag)
        {
            if (!_hasHitted)
            {
                otherTag = "";
                return false;
            }

            else
            {
                _hasHitted = false;
                otherTag = _lastTag;
                return true;
            }
        }

        public bool Exited(out string otherTag)
        {
            if (!_hasExited)
            {
                otherTag = "";
                return false;
            }

            else
            {
                _hasExited = false;
                otherTag = _lastTag;
                return true;
            }
        }
    }
}