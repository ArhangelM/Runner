using UnityEngine;

namespace Assets.Runner.Scripts.Common.Utils
{
    internal class SpawnOnCollisionEnter : MonoBehaviour
    {
        [SerializeField] private GameObject _prefabToSpawn;
        
        private void OnCollisionEnter(Collision collision)
        {
            var contact = collision.contacts[0];
            var rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Instantiate(_prefabToSpawn, contact.point, rotation);
        }
    }
}
