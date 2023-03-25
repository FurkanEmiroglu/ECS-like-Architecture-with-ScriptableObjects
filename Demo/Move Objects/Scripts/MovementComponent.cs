using ECSArchitecture.Core;
using UnityEngine;

namespace ECSArchitecture.Demo
{
    public class MovementComponent : SystemComponent
    {
        [SerializeField] private float speed;
        public float Speed { get { return speed; } }

        private void OnEnable()
        {
            RegisterComponent();
        }

        private void OnDisable()
        {
            RemoveComponent();
        }
    }
}