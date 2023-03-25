using UnityEngine;

namespace ECSArchitecture.Core
{
    public abstract class SystemComponent : MonoBehaviour
    {
        [SerializeField] private GameSystem system;

        /// <summary>
        /// Ideally, only 'system' should change this ID. Negative values means its not assigned yet.
        /// </summary>
        public int EntityID { get; set; } = -1;

        /// <summary>
        /// Registering component to the referenced system
        /// </summary>
        protected void RegisterComponent()
        {
            system.RegisterComponent(this);
        }

        /// <summary>
        /// Removing component from the referenced system
        /// </summary>
        protected void RemoveComponent()
        {
            system.RemoveComponent(this);
        }
    }
}