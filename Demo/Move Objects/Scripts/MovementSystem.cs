using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using ECSArchitecture.Core;

namespace ECSArchitecture.Demo
{
    public class MovementSystem : GameSystem
    {
        [SerializeField] private int maxEntities;
        protected override int MaxEntities { get { return maxEntities; } }

        private MovementComponent[] _components;
        private Transform[] _transforms;
        private float[] _speeds;

        public override void OnSystemAwake()
        {
            base.OnSystemAwake();

            _components = new MovementComponent[MaxEntities];
            _transforms = new Transform[MaxEntities];
            _speeds = new float[MaxEntities];
        }

        public override void OnSystemUpdate()
        {
            MoveForward();
        }

        private void MoveForward()
        {
            float deltaTime = Time.deltaTime;

            NativeArray<Vector3> moveVectors = new NativeArray<Vector3>(ActiveEntityIDs.Count, Allocator.TempJob);
            NativeArray<float> speeds = new NativeArray<float>(_speeds, Allocator.TempJob);
            NativeArray<Vector3> directions = new NativeArray<Vector3>(ActiveEntityIDs.Count, Allocator.TempJob);

            for (int i = 0; i < ActiveEntityIDs.Count; i++)
            {
                Transform t = _transforms[ActiveEntityIDs[i]];
                directions[i] = t.forward;
                speeds[i] = _speeds[i];
            }

            CalculateMoveVector job = new CalculateMoveVector
            {
                MoveVector = moveVectors, Speeds = speeds, Directions = directions, Delta = deltaTime
            };

            JobHandle jobHandle = job.Schedule(ActiveEntityIDs.Count, 64);

            jobHandle.Complete();

            speeds.Dispose();
            directions.Dispose();

            for (int i = 0; i < ActiveEntityIDs.Count; i++)
            {
                Transform t = _transforms[ActiveEntityIDs[i]];
                t.position += moveVectors[i];
            }

            moveVectors.Dispose();
        }

        public override void RegisterComponent(SystemComponent component)
        {
            base.RegisterComponent(component);
            int id = component.EntityID;

            _components[id] = (MovementComponent)component;
            _transforms[id] = component.transform;
            _speeds[id] = ((MovementComponent)component).Speed;
        }
    }
}