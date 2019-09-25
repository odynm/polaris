// Polaris - Project VII 2018
// 08/03/18 -> 02/07/18
// Compiled in Unity 2017.2.0f3
// Writed by Mathias Ody (M-Ody)
// mathiasluizody@hotmail.com
using EZCameraShake;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

namespace PolarisCore
{
    public class PlayerController : MonoBehaviour
    {
        private const float DISTANCE_TO_GROUND = 1f;

        public bool debugMode;

        public Transform dolly;

        public float velocity = 6f;
        public float sprintSpeed = 8f;
        public float jumpSpeed = 15f;
        public float _gravity = 1f;
        public Animator fadeAnim;

        private InputHandler _ih;
        private PlayerRotation _pr;
        private CharacterController _cc;
        private Animator _anim;
        private Inventory _inventory;
        private GunManager _gun;
        private Animator _camAnim;
        private Rigidbody _rb;
        private CapsuleCollider _col;

        private Vector3 _verticalVelocity = Vector3.zero;

        private bool _isFlying = true;
        private Vector3 _movement;
        private float _gravityAccelerationY = 0f;

        private bool alive = true;

        private bool _isPaused = false;

        private int _playerPainKey;
        private int _playerJumpKey;
        private int _playerDeathKey;

        void Awake()
        {
            var obj = Camera.main.transform.parent;
            var prefs = Saver.Instance.GetPrefs();
            obj.GetComponentInChildren<VolumetricLightRenderer>().enabled = prefs.postProcessing;
            obj.GetComponentInChildren<PostProcessingBehaviour>().enabled = prefs.postProcessing;
        }

        void Start()
        {
            _ih = GetComponent<InputHandler>();
            _pr = GetComponentInChildren<PlayerRotation>();
            _cc = GetComponent<CharacterController>();
            _anim = GameObject.Find("Arms").GetComponent<Animator>();
            _inventory = Inventory.Instance;
            _gun = GetComponentInChildren<GunManager>();
            _camAnim = Camera.main.gameObject.GetComponent<Animator>();
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<CapsuleCollider>();

            _inventory.Initialize();

            if (debugMode)
                _inventory.MaxAll();

            Cursor.lockState = CursorLockMode.Locked;

            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                if (SceneManager.GetActiveScene().name.Contains("1"))
                {
                    Saver.Instance.DeleteProgress();
                    var stats = Saver.Instance.GetStats();
                    Inventory.Instance._health = 100;
                    Inventory.Instance._pistolAmmo = 15;
                    Inventory.Instance._shotgunAmmo = 5;
                    Inventory.Instance._rocketLauncherAmmo = 1;
                    Inventory.Instance.RemoveItem(EItemType.SHOTGUN);
                    Inventory.Instance.RemoveItem(EItemType.ROCKET_LAUNCHER);
                    Inventory.Instance.RequestUpdateHUD();
                }
                else
                {
                    var stats = Saver.Instance.GetStats();
                    if (stats.hasShotgun)
                        Inventory.Instance.AddItem(EItemType.SHOTGUN);
                    if (stats.hasRocketLauncher)
                        Inventory.Instance.AddItem(EItemType.ROCKET_LAUNCHER);
                    Inventory.Instance._health = stats.health;
                    Inventory.Instance._pistolAmmo = stats.pistolAmmo;
                    Inventory.Instance._shotgunAmmo = stats.shotgunAmmo;
                    Inventory.Instance._rocketLauncherAmmo = stats.rocketLauncherAmmo;
                    Inventory.Instance.RequestUpdateHUD();
                }
            }

            _playerPainKey = Pool.Instance.GetSharedPoolKey("PlayerPain");
            _playerJumpKey = Pool.Instance.GetSharedPoolKey("PlayerJump");
            _playerDeathKey = Pool.Instance.GetSharedPoolKey("PlayerDeath");
        }

        void Update()
        {
            UpdateGravity();
            HandleMovimentation();
            HandleAnimations();
        }

        public bool HasCard(ECards card)
        {
            return _inventory.HasCard(card);
        }

        public void Hit(int damage)
        {
            if (debugMode) return;
            
            if (_inventory.Hit(damage) <= 0)
            {
                Die();
            }
            else
            {
                CameraShaker.Instance.ShakeOnce(2f, 2f, 0.1f, 0.1f);
                SoundManager.Play(_playerPainKey, transform);
            }
        }

        private void UpdateGravity()
        {
            if (!_cc.isGrounded && _verticalVelocity.y > -100f)
            {
                _verticalVelocity.y -= _gravity * Time.deltaTime;
            }
        }

        private void HandleMovimentation()
        {
            bool[] buffer = _ih.GetInputStream();

            int hMov, vMov;
            Vector2 axis = _ih.GetMouseAxisInput();

            vMov = buffer[(int)EInputNames.FORWARD] ? 1 : 0;
            vMov = buffer[(int)EInputNames.BACKWARD] ? -1 : vMov;

            hMov = buffer[(int)EInputNames.RIGHT] ? 1 : 0;
            hMov = buffer[(int)EInputNames.LEFT] ? -1 : hMov; 

            CheckIfIsPaused();

            if (!_isPaused)
            {
                float hMovf, vMovf;
                MovimentationHelper.Axify(hMov, vMov);
                MovimentationHelper.GetAxis(out hMovf, out vMovf);
                Move();
                Rotate(axis);


                if (buffer[(int)EInputNames.JUMP])
                {
                    Jump();
                    buffer[(int)EInputNames.JUMP] = false;
                }

                if (buffer[(int)EInputNames.FIRE])
                {
                    Fire();
                    buffer[(int)EInputNames.FIRE] = false;
                }

                if (buffer[(int)EInputNames.CHANGE_GUN_DOWN])
                    _gun.NextGun();

                else if (buffer[(int)EInputNames.CHANGE_GUN_UP])
                    _gun.PreviousGun();

                if (buffer[(int)EInputNames.FIRST_GUN])
                    _gun.ChooseGun(0);
                else if (buffer[(int)EInputNames.SECOND_GUN])
                    _gun.ChooseGun(1);
                else if (buffer[(int)EInputNames.THIRD_GUN])
                    _gun.ChooseGun(2);
            }
        }

        private void Rotate(Vector2 axis)
        {
            transform.Rotate(transform.up * axis.x);

            _pr.RotateHead(axis.y);
        }

        private void Fire()
        {
            _gun.Shoot();
        }

        private void Move()
        {
            //CONTROLE DE MOVIMENTO DO OBJETO PLAYER
            float velocidadeZ = Input.GetAxis("Vertical") * velocity;
            float velocidadeX = Input.GetAxis("Horizontal") * velocity;

            if (!_cc.isGrounded)
            {
                _movement += _verticalVelocity;
                _movement *= Time.deltaTime;
            }

            /*if (Input.GetKey(KeyCode.LeftShift))
            {
                velocidadeZ *= sprintSpeed;
            }*/

            //ATUALIZA A DIRECAO QUE O PLAYER CAMINHA
            //DEPENDENDO DA DIRECAO QUE A CAMERA ESTA DIRECIONADA
            Vector3 speed = new Vector3(velocidadeX, _verticalVelocity.y, velocidadeZ);
            speed = transform.rotation * speed;
            //Move ignora gravidade, SimpleMove aplica gravidade, mas seu uso e limitado.

            //Dolly move
            dolly.localEulerAngles = new Vector3(0, 0, -Input.GetAxis("Horizontal") * 0.5f);

            _anim.SetFloat("Velocity", new Vector3(velocidadeX, 0f, velocidadeZ).magnitude);
            _cc.Move(speed * Time.deltaTime);
        }

        private void Jump()
        {
            if (_cc.isGrounded)
            {
                _anim.SetTrigger("Jump");
                _verticalVelocity.y = jumpSpeed;
                SoundManager.Play(_playerJumpKey, transform);
            }
        }

        private void HandleAnimations()
        {
            if (_isFlying)
            {
                if (_cc.isGrounded)
                {
                    _isFlying = false;
                    _anim.SetTrigger("Land");
                }
            }
            else
            {
                _isFlying = !_cc.isGrounded;
            }
        }

        private void Die()
        {
            if (alive)
            {
                _anim.SetTrigger("Die");
                _cc.enabled = false;
                _rb.isKinematic = false;
                _col.enabled = true;
                _ih.LockControl(true);
                SoundManager.Play(_playerDeathKey, transform);

                Destroy(_inventory);
                fadeAnim.gameObject.SetActive(true);
                fadeAnim.SetTrigger("FadeOut");
                Invoke("Restart", 0.5f);
                alive = false;
            }
        }

        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Collectable")
            {
                bool shouldDelete = _inventory.AddItem(other.GetComponent<Collectable>().itemType);
                other.gameObject.SetActive(!shouldDelete);
            }
        }

        private void CheckIfIsPaused()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                GameObject.FindWithTag("GameManager").GetComponent<GameManager>().Pause();
                _isPaused = !_isPaused;
                if (_isPaused)
                {
                    Cursor.lockState = CursorLockMode.Confined;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }             
            }
        }
    }
}
