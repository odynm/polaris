// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PolarisCore
{
    public class InputHandler : MonoBehaviour
    {
        private float _mouseSensibility;

        private bool[] _inputBuffer = new bool[(int)EInputNames.TOTAL_INPUTS];

        // Pode ser alterado para receber teclas dinâmicas, apenas é necessário
        // que outra classe altere os arrays de input desta classe, alterando os keycodes
        // de cada ação
        // O único problema que permanece é o dos Axis, que provavelmente deve ser feito manualmente
        // para todas possibilidades

        private KeyCode[] _forwardInput = new KeyCode[2];
        private KeyCode[] _backwardInput = new KeyCode[2];
        private KeyCode[] _leftInput = new KeyCode[2];
        private KeyCode[] _rightInput = new KeyCode[2];
        private KeyCode[] _jumpInput = new KeyCode[2];
        private KeyCode[] _fireInput = new KeyCode[2];
        private KeyCode[] _aimInput = new KeyCode[2];
        private KeyCode[] _changeGunUp = new KeyCode[2];
        private KeyCode[] _changeGunDown = new KeyCode[2];
        private KeyCode[] _firstGun = new KeyCode[2];
        private KeyCode[] _secondGun = new KeyCode[2];
        private KeyCode[] _thirdGun = new KeyCode[2];

        private bool _controlLocked;

        void Awake()
        {
            SetUpForKeyboard();
            
            SetMouseSensibility(Saver.Instance.GetPrefs().mouseSensitivity);
        }

        void Update()
        {
            HandleInput(EInputNames.FORWARD, _forwardInput);
            HandleInput(EInputNames.BACKWARD, _backwardInput);
            HandleInput(EInputNames.LEFT, _leftInput);
            HandleInput(EInputNames.RIGHT, _rightInput);
            HandleInput(EInputNames.JUMP, _jumpInput);
            HandleInput(EInputNames.FIRE, _fireInput);
            HandleInput(EInputNames.AIM, _aimInput);
            HandleInput(EInputNames.FIRST_GUN, _firstGun);
            HandleInput(EInputNames.SECOND_GUN, _secondGun);
            HandleInput(EInputNames.THIRD_GUN, _thirdGun);
            GetAxisInput(EInputNames.CHANGE_GUN_UP, EInputNames.CHANGE_GUN_DOWN, "Mouse ScrollWheel");
        }

        public void SetMouseSensibility(float sensiblity)
        {
            _mouseSensibility = 1f + (sensiblity * 9f);
        }

        public void SetInvertMouse (bool invert)
        {
            if (invert)
                _fireInput [0] = KeyCode.Mouse1;
            else
                _fireInput [0] = KeyCode.Mouse0;
        }

        public bool[] GetInputStream()
        {
            if (_controlLocked) 
                return new bool[(int)EInputNames.TOTAL_INPUTS];
            return _inputBuffer;
        }

        public Vector2 GetMouseAxisInput()
        {
            var axisInput = new Vector2();

            if (_controlLocked)
                return axisInput;
                
            axisInput.x = Input.GetAxis("Mouse X") * _mouseSensibility;
            axisInput.y = Input.GetAxis("Mouse Y") * _mouseSensibility;
            return axisInput;
        }

        public void LockControl(bool flag)
        {
            _controlLocked = flag;
        }

        private void GetAxisInput(EInputNames positive, EInputNames negative, string axisName)
        {
            _inputBuffer[(int)positive] = _inputBuffer[(int)negative] = false;

            if (Input.GetAxisRaw(axisName) > 0f)
                _inputBuffer[(int)positive] = true;
            if (Input.GetAxisRaw(axisName) < 0f)
                _inputBuffer[(int)negative] = true;
        }

        private void SetUpForKeyboard()
        {
            _forwardInput[0] = KeyCode.W; _forwardInput[1] = KeyCode.UpArrow;
            _backwardInput[0] = KeyCode.S; _backwardInput[1] = KeyCode.DownArrow;
            _leftInput[0] = KeyCode.A; _leftInput[1] = KeyCode.LeftArrow;
            _rightInput[0] = KeyCode.D; _rightInput[1] = KeyCode.RightArrow;
            _jumpInput[0] = KeyCode.Space; _jumpInput[1] = KeyCode.None;
            _aimInput[0] = KeyCode.None; _aimInput[1] = KeyCode.None;
            _changeGunUp[0] = KeyCode.Mouse1; _changeGunUp[1] = KeyCode.None;
            _changeGunDown[0] = KeyCode.Mouse1; _changeGunDown[1] = KeyCode.None;
            _firstGun[0] = KeyCode.Alpha1; _firstGun[1] = KeyCode.Alpha0;
            _secondGun[0] = KeyCode.Alpha2; _secondGun[1] = KeyCode.Alpha9;
            _thirdGun[0] = KeyCode.Alpha3; _thirdGun[1] = KeyCode.Alpha8;

            if (Saver.Instance.GetPrefs().invertMouseClick)
                _fireInput [0] = KeyCode.Mouse1;
            else
                _fireInput [0] = KeyCode.Mouse0;
            _fireInput[1] = KeyCode.LeftControl;
        }

        private void HandleInput(EInputNames action, KeyCode[] keyCodes)
        {
            if (_inputBuffer[(int)action] == true)
            {
                for (int i = 0; i < keyCodes.Length; i++)
                {
                    if (Input.GetKeyUp(keyCodes[i]))
                    {
                        _inputBuffer[(int)action] = false;
                        return;
                    }
                }
            }

            else
            {
                for (int i = 0; i < keyCodes.Length; i++)
                {
                    if (Input.GetKeyDown(keyCodes[i]))
                    {
                        _inputBuffer[(int)action] = true;
                        return;
                    }
                }
            }
        }
    }
}
