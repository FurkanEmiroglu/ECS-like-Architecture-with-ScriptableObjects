# ECS-Like Game Architecture wScriptableObjects

#### This package is a simple game architecture system promotes data driven programming & C# jobs system, using scriptable objects.


System has three core classes.

- **Engine:** Because scriptableobject's doesn't have MonoBehaviour callbacks, we need a wrapper.
Engine knows System reference via inspector, and connects their logic to the Unity's callbacks.
- **Component:** This is a monobehaviour used for data storage and handling registration/unregistration of a gameobject to the systems. Also stores ENTITY ID.
- **System:** A scriptable object contains all the logic for the gameobjects.

*note:* Remember that systems can also contain logic that will run outside of callbacks. Since they are scriptableobjects, you can call any public method outside of the callback, anywhere you want by binding its references via the inspector. This idea is inspired by Ryan Hipple's ScriptableObject Runtime Sets idea.

### Instructions & Demo Explanation

**1. Create a monobehaviour class which derives from "SystemComponent" class.** \
**2. Put every data exclusive to that gameobject which will be used in your logic.**

```java
using UnityEngine

public class MovementComponent : SystemComponent
{
    [SerializeField] private float speed;
    
    public float Speed
    {
        get
        {
            return speed;
        }
    }        
}

```


**3. Depending on when you want the game object to move, register in the system where the action starts.**

```java
private void OnEnable()
{
    RegisterComponent();
}

private void OnDisable()
{
    RemoveComponent();
}

```

*Final version of movement component*

```java
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
```

**4. Create a system class which derives from GameSystem that will contain your logic.** 

```java
public class MovementSystem : GameSystem
{
    protected override void MaxEntities {get;}
}
```

**5. Add a field declaring maximum entity (gameobject) count.**
```java
public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override void MaxEntities {get {return maxEntities; } }
}
```

**6. Lets create an array of transforms & components, to maintain objects that registered to the system.**

```java
public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override void MaxEntities {get {return maxEntities; } }
    
    private MovementComponent[] _components;
    private Transform[] _transforms;
    private float[] _speeds;
}
```

**7. Lets populate these arrays on component register operation by overriding RegisterComponent method**
Base method will assign unique id's to each component & the id will be used as it's index.
```java
public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override void MaxEntities {get {return maxEntities; } }
    
    private MovementComponent[] _components;
    private Transform[] _transforms;
    private float[] _speeds;
    
    public override void RegisterComponent(SystemComponent component)
    {
    base.RegisterComponent(component);
    int id = component.EntityID;

    _components[id] = (MovementComponent)component;
    _transforms[id] = component.transform;
    _speeds[id] = ((MovementComponent)component).Speed;
    }
}
```


**7. You need to clear/redefine these array references at awake, because of how scriptableobjects work.**

```java
public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override void MaxEntities {get {return maxEntities; } }
    
    private MovementComponent[] _components;
    private Transform[] _transforms;
    private float[] _speeds;
    
    // remember these virtual callbacks will be connected to the unity via Engine.cs
    public override void OnSystemAwake()
    {
        base.OnSystemAwake();

        _components = new MovementComponent[MaxEntities];
        _transforms = new Transform[MaxEntities];
        _speeds = new float[MaxEntities];
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
```

**8. Lets create a struct to define our movement logic via C# jobs system**
```java
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
```

**9. Define our Movement logic inside MovementSystem.cs and call the function on Update**
```java
public class MovementSystem : GameSystem
{
    [SerializeField] private int maxEntities;
    protected override void MaxEntities {get {return maxEntities; } }
    
    private MovementComponent[] _components;
    private Transform[] _transforms;
    private float[] _speeds;
    
    // remember these virtual callbacks will be connected to the unity via Engine.cs
    public override void OnSystemAwake()
    {
        base.OnSystemAwake();

        _components = new MovementComponent[MaxEntities];
        _transforms = new Transform[MaxEntities];
        _speeds = new float[MaxEntities];
    }
    
    public override void RegisterComponent(SystemComponent component)
    {
    base.RegisterComponent(component);
    int id = component.EntityID;

    _components[id] = (MovementComponent)component;
    _transforms[id] = component.transform;
    _speeds[id] = ((MovementComponent)component).Speed;
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
}
```

**10. Add Asset Menu Attribute to the movement system**

```java
[UnityEngine.CreateAssetMenu]
public class MovementSystem : GameSystem
```

**11. Create an empty gameobject in scene, and add Engine.cs component.**\
**12. Create an instance of MovementComponent via Create asset menu via right click => create in projects window.**\
**13. Add MovementComponent.cs to the gameobjects that needs to move, adjust speed field & populate the system reference via inspector."**\
**14. Populate the system reference via Engine.cs inspector.**

## Developer Notes 
- The amount of performance gained through this approach is directly related to your knowledge of the C# jobs system and the multithread efficiency of your algorithm.
- Do not forget that Burst Compiler gives very different performance results in editor than build, and always test your applications on the target platform.
- Since you can easily inject the references of GameSystem scriptableobjects into monobehaviours, feel free to add the methods to be called outside of Unity callbacks as public to your systems and call the added method anywhere you want.
- Changing Engine.cs execution order to just before default execution order is highly recommended.
- This was just an idea about scriptableobjects. I don't think it's production ready yet.


How To Install
--------------

This package uses the [scoped registry] feature to resolve package
dependencies. Open the Package Manager page in the Project Settings window and
add the following entry to the Scoped Registries list:

### 1. Open Edit / Project Settings in Unity
![Project Settings](https://raw.githubusercontent.com/FurkanEmiroglu/FurkanEmiroglu/main/Project%20Settings.jpg)

### 2. Go to Package Manager
![Package Manager](https://raw.githubusercontent.com/FurkanEmiroglu/FurkanEmiroglu/main/Package%20Manager.jpg)

### 3. On Scoped Registries Window add the keys below
![Scoped Registry](https://raw.githubusercontent.com/FurkanEmiroglu/FurkanEmiroglu/main/Scoped%20Registries.jpg)
- Name: `FEmiroglu`
- URL: `https://registry.npmjs.com`
- Scope: `com.femiroglu`

### 4. Open Window / Package Manager in Unity

![Open Package Manager](https://raw.githubusercontent.com/FurkanEmiroglu/FurkanEmiroglu/main/Open%20Package%20Manager.jpg)

### 5. Select My Registries Option
![My Registries](https://raw.githubusercontent.com/FurkanEmiroglu/FurkanEmiroglu/main/Select%20My%20Registries.jpg)

Now you can install the package from My Registries page in the Package Manager
window.

![My Registries](https://user-images.githubusercontent.com/343936/162576825-4a9a443d-62f9-48d3-8a82-a3e80b486f04.png)

