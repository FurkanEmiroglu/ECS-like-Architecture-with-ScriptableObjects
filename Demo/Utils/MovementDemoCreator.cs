using UnityEngine;

public class MovementDemoCreator : MonoBehaviour
{
    [SerializeField] private TestType testType;
    [SerializeField] private GameObject ecsPrefab;
    [SerializeField] private GameObject oopPrefab;
    
    [SerializeField] private int count;

    private void Awake()
    {
        CreateSceneObjects();
    }

    private void CreateSceneObjects()
    {
        GameObject activePrefab = testType == TestType.ECS ? ecsPrefab : oopPrefab;
        
        for (int i = 0; i < count; i++)
        {
            Transform t = Instantiate(activePrefab).transform;
            t.position = new Vector3(i*1.5f, 0, 0);
        }
    }

    private enum TestType
    {
        OOP,
        ECS
    }
}