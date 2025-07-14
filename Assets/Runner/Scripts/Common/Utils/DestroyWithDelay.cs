using UnityEngine;

namespace Assets.Runner.Scripts.Common.Utils
{
    internal class DestroyWithDelay : MonoBehaviour
    {
        [SerializeField] private float _delay = 1f;
        private void Awake()
        {
            Destroy(gameObject, _delay);
        }
    }
}
