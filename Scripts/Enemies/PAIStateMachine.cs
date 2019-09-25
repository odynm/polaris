// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PAI
{
    public class PAIStateMachine : MonoBehaviour
    {
        private PAIView _view;
        private PAIRoaming _roaming;
        private PAIEnemy _enemy;
        private IPAIAttack _attack;

        private bool _hasFoundPlayer;

        EnemyStates _currentState;
        enum EnemyStates
        {
            Roaming,
            UnderDirectControl,
            Attacking
        }

        private void Awake()
        {
            _view = GetComponent<PAIView>();
            _roaming = GetComponent<PAIRoaming>();
            _enemy = GetComponent<PAIEnemy>();
            _attack = GetComponent<IPAIAttack>();
        }

        public void InitializeObjects()
        {
            if (_enemy.canRoam)
            {
                _roaming.CanRoam(true);
                _currentState = EnemyStates.Roaming;
            }
        }

        public void Execute()
        {
            if (_currentState == EnemyStates.UnderDirectControl)
            {
                throw new NotImplementedException();
            }

            else if (_currentState == EnemyStates.Roaming)
            {
                _roaming.Execute();
            }

            else if (_currentState == EnemyStates.Attacking)
            {
                _attack.Execute();
            }

            _hasFoundPlayer = _view.Execute();

            if (_hasFoundPlayer && _currentState != EnemyStates.UnderDirectControl)
            {
                _currentState = EnemyStates.Attacking;
            }
        }

        public void ActivateEnemyAttack()
        {
            _currentState = EnemyStates.Attacking;
        }

        public void ActivateManualControl()
        {
            _currentState = EnemyStates.UnderDirectControl;
        }

        public void Reset()
        {

        }
    }
}