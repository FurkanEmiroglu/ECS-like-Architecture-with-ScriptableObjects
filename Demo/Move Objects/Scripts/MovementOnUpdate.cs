using UnityEngine;

namespace ECSArchitecture.Demo
{
    public class MovementOnUpdate : MonoBehaviour
    {
        public float speed;

        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Update()
        {
            _transform.position += _transform.forward * (speed * Time.deltaTime);
        }
    }
}