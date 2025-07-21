using Assets.Runner.Scripts.Common.Utils;
using Assets.Runner.Scripts.Runtime.Weapons;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Runner.Scripts.Runtime.Enemy
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Material _damageMaterial;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private MeshRenderer _meshRenderer;

        private ActionTimer _actionTimer;
        private int _currentPointIndex = 0;

        private void Start()
        {
            _meshRenderer.material = _defaultMaterial;
            _actionTimer = new ActionTimer(ResetMaterial);
            SetDestination();
        }

        private void Update()
        {
            ChangedDirection();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<Bullet>(out _))
            {
                _actionTimer.Start();
                _meshRenderer.material = _damageMaterial;
            }
        }

        private void ChangedDirection()
        {
            if (Vector3.Distance(transform.position, _points[_currentPointIndex].position) < 1f)
            {
                Debug.Log("Reached point: " + _currentPointIndex);
                ChoicePoint();
                SetDestination();
            }
        }

        private void SetDestination()
        {
            _agent.SetDestination(_points[_currentPointIndex].position);
          
        }

        private void ChoicePoint()
        {
            _currentPointIndex++;
            if (_currentPointIndex >= _points.Length)
                _currentPointIndex = 0;
        }

        private async void ResetMaterial()
        {
            await UniTask.SwitchToMainThread();
            Debug.Log("Resetting material for enemy: " + gameObject.name);
            _meshRenderer.material = _defaultMaterial;
            _actionTimer.Stop();
        }
    }
}