using UnityEngine;

namespace ECSArchitecture.Core
{
    public class Engine : MonoBehaviour
    {
        [SerializeField] private GameSystem[] systems;

        private void Awake()
        {
            foreach (GameSystem system in systems)
                system.OnSystemAwake();
        }
        
        private void Start()
        {
            foreach (GameSystem system in systems)
                system.OnSystemStart();
        }
        
        private void Update()
        {
            foreach (GameSystem system in systems)
                system.OnSystemUpdate();
        }

        private void LateUpdate()
        {
            foreach (GameSystem system in systems)
            {
                system.OnSystemLateUpdate();
            }
        }

        private void FixedUpdate()
        {
            foreach (GameSystem system in systems)
                system.OnSystemFixedUpdate();
        }
        
        private void OnEnable()
        {
            foreach (GameSystem system in systems)
                system.OnSystemEnable();
        }
        
        private void OnDisable()
        {
            foreach (GameSystem system in systems)
                system.OnSystemDisable();
        }

        private void OnDestroy()
        {
            foreach (GameSystem system in systems)
            {
                system.OnSystemDestroy();
            }
        }
    }
}
