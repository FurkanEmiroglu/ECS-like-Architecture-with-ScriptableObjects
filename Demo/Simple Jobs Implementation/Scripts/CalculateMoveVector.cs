using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ECSArchitecture.Demo
{
    [BurstCompile]
    public struct CalculateMoveVector : IJobParallelFor
    {
        public NativeArray<Vector3> MoveVector;
        public NativeArray<float> Speeds;
        public NativeArray<Vector3> Directions;
        public float Delta;

        public void Execute(int index)
        {
            MoveVector[index] += Directions[index] * (Speeds[index] * Delta);
        }
    }
}