using System;
using UnityEngine;

namespace Assets.Runner.Scripts.Runtime.Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _shootForce = 10f;

        public event Action<Bullet> OnBulletCollide;

        private Transform _parent;

        private void OnCollisionEnter(Collision collision)
        {
            ResetTransform();
        }

        private void ResetTransform()
        {
            gameObject.SetActive(false);
            transform.position = _parent.position;
            transform.SetParent(_parent);
            _rigidbody.linearVelocity = Vector3.zero;
            OnBulletCollide?.Invoke(this);
        }

        public void Fire()
        {
            gameObject.SetActive(true);
            transform.SetParent(null);

            var screenCenterPoint = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
            var ray = Camera.main.ScreenPointToRay(screenCenterPoint);

            if (Physics.Raycast(ray, out var hit, 30f))
            {
                var direction = (hit.point - transform.position).normalized;
                var rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
                _rigidbody.AddForce(direction * _shootForce, ForceMode.Impulse);
            }
        }

        public void Init(Transform parent)
        {
            _parent = parent;
        }
    }
}