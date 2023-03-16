using UnityEngine;

public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override int MaxEntities { get { return maxEntities; } }

    private MovementComponent[] _components;
    private Transform[] _transforms;
    private float[] _speeds;

    public override void OnAwake()
    {
        base.OnAwake();

        _components = new MovementComponent[MaxEntities];
        _transforms = new Transform[MaxEntities];
        _speeds = new float[MaxEntities];
    }

    public override void OnUpdate()
    {
        MoveForward();
    }
    
    private void MoveForward()
    {
        float deltaTime = Time.deltaTime;
        
        foreach (int id in ActiveEntityIDs)
        {
            _transforms[id].position += _transforms[id].forward * (_components[id].speed * deltaTime);
        }
    }

    public override void RegisterComponent(GameComponent component)
    {
        base.RegisterComponent(component);
        int id = component.EntityID;
        
        _components[id] = (MovementComponent)component;
        _transforms[id] = component.transform;
        _speeds[id] = ((MovementComponent)component).speed;
    }
}