//namespace EmeWillem
//{
//    using UnityEngine;
//    using System;

//    public class PlayerManager : MonoBehaviour
//    {
//        private float _delta;

//        [Header("PLAYER UI REFERENCES")]
//        [SerializeField] private StanceUI _stanceUI;

//        private PlayerInputHandler _inputHandler;
//        private PlayerAnimatorManager _AnimatorManager;
//        private PlayerStanceManager _stanceManager;
//        private PlayerStatManager _statManager;
//        private PlayerLock _lock;
//        private PlayerLocomotion _locomotion;
//        private PlayerAttackHandler _attackHandler;
//        private Health _health;
//        private Souls _souls;
//        private MovementTracking _movementTracking;
//        private FootstepHandler _footstepHandler;
//        private CameraController _cameraController;

//        private Transform _followTarget = null;

//        public static event Action<BaseAnimatorManager> PlayerDeath;

//        public void Init()
//        {
//            DontDestroyOnLoad(gameObject);

//            _inputHandler = GetComponent<PlayerInputHandler>();
//            _AnimatorManager = GetComponent<PlayerAnimatorManager>();
//            _stanceManager = GetComponent<PlayerStanceManager>();
//            _statManager = GetComponent<PlayerStatManager>();
//            _lock = GetComponent<PlayerLock>();
//            _locomotion = GetComponent<PlayerLocomotion>();
//            _attackHandler = GetComponent<PlayerAttackHandler>();
//            _health = GetComponent<Health>();
//            _souls = GetComponent<Souls>();
//            _movementTracking = GetComponent<MovementTracking>();
//            _footstepHandler = GetComponent<FootstepHandler>();
//            _cameraController = CameraController.Instance;

//            _stanceUI.Init();

//            _AnimatorManager.Init();
//            _stanceManager.Init();
//            _statManager.Init();
//            _lock.Init();
//            _locomotion.Init();
//            _attackHandler.Init(LayerMask.GetMask("DamageCollider"));
//            _souls.Init();
//            _movementTracking.Init();
//            _footstepHandler.Init();
//            _cameraController.Init(transform);

//            _lock.Locked += PlayerManager_LockedOn;
//            _health.HealthExhausted += PlayerManager_ValueExhausted;
//        }

//        public void FixedTick(float delta)
//        {
//            _delta = delta;

//            _stanceManager.FixedTick(_delta);
//            _attackHandler.FixedTick(_delta);
//            _movementTracking.FixedTick(_delta);
//        }

//        public void LateTick(float delta)
//        {
//            _delta = delta;

//            _statManager.LateTick(_delta);
//        }

//        public void FixedTick(float delta)
//        {
//            _delta = delta;

//            Vector3 xDirection = _cameraController._CameraTransform.right;
//            Vector3 yDirection = _cameraController._CameraTransform.forward;
//            float leftStickX = _inputHandler.LeftStickX;
//            float leftStickY = _inputHandler.LeftStickY;
//            float rightStickX = _inputHandler.RightStickX;
//            float rightStickY = _inputHandler.RightStickY;

//            _locomotion.FixedTick(_delta, xDirection, yDirection, leftStickX, leftStickY, _followTarget);
//            _cameraController.FixedTick(_delta, _followTarget, new Vector2(rightStickX, rightStickY));
//        }

//        public void Cleanup()
//        {
//            _lock.Locked -= PlayerManager_LockedOn;
//            _health.HealthExhausted -= PlayerManager_ValueExhausted;

//            _stanceUI.Cleanup();

//            _stanceManager.Cleanup();
//            _statManager.Cleanup();
//            _lock.Cleanup();
//            _locomotion.Cleanup();
//            _attackHandler.Cleanup();
//            _souls.Cleanup();
//        }

//        private void PlayerManager_LockedOn(Transform target)
//        {
//            _followTarget = target;
//        }

//        private void PlayerManager_ValueExhausted(GameObject playerObject)
//        {
//            _AnimatorManager.ForceCrossFade("Death");

//            Cleanup();

//            Destroy(_stanceManager);
//            Destroy(_statManager);
//            Destroy(_lock);
//            Destroy(_locomotion);
//            Destroy(_attackHandler);
//            Destroy(_health);
//            Destroy(_cameraController);

//            OnPlayerDeath();

//            Destroy(this);
//        }

//        private void OnPlayerDeath()
//        {
//            PlayerDeath?.Invoke(_AnimatorManager);
//        }
//    }
//}