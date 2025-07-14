using Assets.Runner.Scripts.Common.Signals;
using System.Collections.Generic;
using Tools.Extensions;
using Tools.SignalBus;
using UnityEngine;

namespace Assets.Runner.Scripts.Runtime.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header("Weapon Settings")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private int _clipSize = 30;

        private Queue<Bullet> _clip = new();

        private void Awake()
        {
            GenerateClip();
        }

        private void OnEnable()
        {
            SubscribeEvens();
        }

        private void OnDisable()
        {
            UnsubscribeEvens();
        }

        private void GenerateClip()
        {
            if (!_bulletPrefab.HasValue())
                Debug.LogError("Bullet prefab is not assigned in the Weapon script.");

            for (int i = 0; i < _clipSize; i++)
            {
                Bullet bullet = Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity, _firePoint);
                bullet.Init(_firePoint);
                bullet.OnBulletCollide += ReturnBulletInClip;
                bullet.gameObject.SetActive(false);
                _clip.Enqueue(bullet);
            }
        }

        private void OnFire(FireSignal signal)
        {
            if (_clip.Count == 0)
            {
                Debug.LogWarning("No bullets left in the clip.");
                return;
            }

            Bullet bullet = _clip.Dequeue();
            bullet.Fire();
        }

        private void ReturnBulletInClip(Bullet bullet)
        {
            _clip.Enqueue(bullet);
        }

        private void SubscribeEvens()
        {
            SignalBus.Instance.Subscribe<FireSignal>(OnFire);
        }

        private void UnsubscribeEvens()
        {
            SignalBus.Instance.Unsubscribe<FireSignal>(OnFire);

        }
    }
}