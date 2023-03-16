using UnityEngine;

namespace GameCore
{
    public class Engine : MonoBehaviour
    {
        [SerializeField] private GameSystem[] systems;

        private void Awake()
        {
            foreach (GameSystem system in systems)
                system.OnAwake();
        }
        
        private void Start()
        {
            foreach (GameSystem system in systems)
                system.OnStart();
        }
        
        private void Update()
        {
            foreach (GameSystem system in systems)
                system.OnUpdate();
        }
        
        private void FixedUpdate()
        {
            foreach (GameSystem system in systems)
                system.OnFixedUpdate();
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
    }
}
