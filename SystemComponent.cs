using UnityEngine;

public abstract class SystemComponent : MonoBehaviour
{
    [SerializeField] private GameSystem system;

    /// <summary>
    /// Ideally, only 'system' should change this ID. Negative values means its not assigned yet.
    /// </summary>
    public int EntityID { get; set; } = -1;
    
    /// <summary>
    /// Makes system
    /// </summary>
    protected void RegisterComponent()
    {
        system.RegisterComponent(this);
    }
    
    protected void RemoveComponent()
    {
        system.RemoveComponent(this);
    }
}