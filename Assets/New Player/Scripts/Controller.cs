using UnityEngine;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        private InputHandler _inputHandler;
        private AnimatorManager _animatorManager;
        private AttackHandler _attackHandler;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private LockMarker _lockMarker;
        private Locomotion _locomotion;
        private Lock _lock;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            Tick(Time.deltaTime);
        }

        private void OnDisable()
        {
            Cleanup();
        }

        public void Init()
        {
            _lockMarker.SingletonSetup();

            _inputHandler = GetComponent<InputHandler>();
            _animatorManager = GetComponent<AnimatorManager>();
            _attackHandler = GetComponent<AttackHandler>();
            _locomotion = GetComponent<Locomotion>();
            _lock = GetComponent<Lock>();

            _inputHandler.Init();
            _animatorManager.Init(1, 1.2f);
            _attackHandler.Init();
            _cameraController.Init(transform);
            _locomotion.Init();
            _lock.Init();
        }

        public void Cleanup()
        {
            _inputHandler.Cleanup();
            _animatorManager.Cleanup();
            _attackHandler.Cleanup();
            _lock.Cleanup();
        }

        public void Tick(float delta)
        {
            Transform lockTarget = _lock.Target;
            Vector3 xDirection = _cameraController._CameraTransform.right;
            Vector3 yDirection = _cameraController._CameraTransform.forward;

            _locomotion.Tick(delta, lockTarget, xDirection, yDirection, _inputHandler.LeftStickInput);
            _cameraController.Tick(delta, lockTarget, _inputHandler.RightStickInput);
        }
    }
}