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
    public class Door : MonoBehaviour
    {
        public ECards requiredCard;

        private Animator[] _anim;
        private PlayerController _player;

        private int numCollidersInside;

        private Transform openSymbol;
        private Transform sealedSymbol;
        private Transform blueSymbol;
        private Transform orangeSymbol;
        private Transform purpleSymbol;

        private int _openDoorKey;
        private int _closeDoorKey;

        private AudioSource _unlockedSound;
        private bool unlocked;
        private bool playedSound;

        void Awake()
        {
            _anim = GetComponentsInChildren<Animator>();
            _unlockedSound = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (requiredCard != ECards.UNLOCKED && requiredCard != ECards.LOCKED)
                _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            _openDoorKey = Pool.Instance.GetSharedPoolKey("DoorOpen");
            _closeDoorKey = Pool.Instance.GetSharedPoolKey("DoorClose");


            //Controle do status das portas
            openSymbol = transform.Find ("Open");
            sealedSymbol = transform.Find ("Sealed");
            blueSymbol = transform.Find ("Blue");
            orangeSymbol = transform.Find ("Orange");
            purpleSymbol = transform.Find ("Purple");
            if (requiredCard == ECards.LOCKED) {
                DisableSymbols ();
                sealedSymbol.gameObject.SetActive(true);
            } else if (requiredCard == ECards.BLUE_CARD) {
                DisableSymbols ();
                blueSymbol.gameObject.SetActive(true);
            } else if (requiredCard == ECards.ORANGE_CARD) {
                DisableSymbols ();
                orangeSymbol.gameObject.SetActive(true);
            } else if (requiredCard == ECards.PURPLE_CARD) {
                DisableSymbols ();
                purpleSymbol.gameObject.SetActive(true);
            }
        }

        private void DisableSymbols() {
            openSymbol.gameObject.SetActive(false);
            sealedSymbol.gameObject.SetActive(false);
            blueSymbol.gameObject.SetActive(false);
            orangeSymbol.gameObject.SetActive(false);
            purpleSymbol.gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (requiredCard == ECards.LOCKED) return;

            if (requiredCard == ECards.UNLOCKED ||
                _player.HasCard(requiredCard))
            {
                if (((1 << other.gameObject.layer) & LayerMaskData.player) != 0)
                {
                    numCollidersInside++;
                    unlocked = true;
                }
                else if (((1 << other.gameObject.layer) & LayerMaskData.enemy) != 0 && other.gameObject.tag == "Enemy")
                {
                    other.gameObject.GetComponent<PAI.PAIEnemy>().SetDoor(this);
                    numCollidersInside++;
                }

                if (unlocked)
                {
                    foreach (Animator a in _anim)
                    {
                        if (!a.GetBool("Open"))
                        {
                            SoundManager.Play(_openDoorKey, transform);
                        }
                        a.SetBool("Open", true);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (requiredCard == ECards.LOCKED) return;

            if (((1 << other.gameObject.layer) & LayerMaskData.player) != 0)
                numCollidersInside = Mathf.Clamp(--numCollidersInside, 0, int.MaxValue);
            else if (((1 << other.gameObject.layer) & LayerMaskData.enemy) != 0 && other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponent<PAI.PAIEnemy>().SetDoor(null);
                numCollidersInside = Mathf.Clamp(--numCollidersInside, 0, int.MaxValue);
            }

            if (numCollidersInside == 0)
            {
                foreach (Animator a in _anim)
                {
                    if (a.GetBool("Open"))
                    {
                        SoundManager.Play(_closeDoorKey, transform);
                    }
                    a.SetBool("Open", false);   
                }
            }
        }

        public void PreSensorActivated()
        {
            if (requiredCard == ECards.UNLOCKED || requiredCard == ECards.LOCKED)
                return;
            if (requiredCard == ECards.LOCKED) return;

            if (_player.HasCard(requiredCard))
            {
                DisableSymbols();
                openSymbol.gameObject.SetActive(true);
                if (!playedSound) {
                    _unlockedSound.Play();
                    playedSound = true;
                }

            }
        }

        public void SubtractEnemy()
        {
            if (numCollidersInside > 0)
            {
                numCollidersInside--;
            }

            if (numCollidersInside == 0)
            {
                foreach (Animator a in _anim)
                {
                    if (a.GetBool("Open"))
                    {
                        SoundManager.Play(_closeDoorKey, transform);
                    }
                    a.SetBool("Open", false);
                }
            }
        }
    }
}