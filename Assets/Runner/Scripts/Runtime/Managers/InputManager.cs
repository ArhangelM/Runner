using Assets.Runner.Scripts.Common.Signals;
using Tools.SignalBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Runner.Scripts.Runtime.Managers
{
    internal class InputManager : MonoBehaviour
    {
        private MyInputSystem _inputSystem;
        private MyInputSystem.CharacterActions _characterActions;

        private void Awake()
        {
            InitInputSystem();
        }

        private void OnEnable() => _inputSystem.Enable();

        private void OnDisable() => _inputSystem.Disable();

        private void InitInputSystem()
        {
            _inputSystem = new MyInputSystem();
            _characterActions = _inputSystem.Character;

            _characterActions.Fire.performed += Fire;
        }

        private void Fire(InputAction.CallbackContext context) => SignalBus.Instance.Invoke(new FireSignal());
    }
}
