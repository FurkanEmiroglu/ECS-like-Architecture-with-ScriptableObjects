using System.Collections.Generic;

public abstract class GameSystem : UnityEngine.ScriptableObject
{
    /// <summary>
    /// Entities IDs that system is controlling actively
    /// </summary>
    protected List<int> ActiveEntityIDs;
    
    /// <summary>
    /// Every entity id, including inactive ones
    /// </summary>
    private List<int> _entityIDs;
    
    /// <summary>
    /// just a counter for generating entity ids
    /// </summary>
    private int _idCounter = 0;
    
    /// <summary>
    /// Maximum number of entities to support
    /// </summary>
    protected abstract int MaxEntities { get; }

    /// <summary>
    /// Adding a component to system. System will apply it's logic & manipulate the component after adding
    /// </summary>
    /// <param name="component"></param>
    public virtual void RegisterComponent(SystemComponent component)
    {
        if (component.EntityID < 0)
        {
            AssignID(component);
            RegisterComponent(component);
            return;
        }
        
        int id = component.EntityID;
            
        if (_entityIDs.Contains(id))
            AddToActiveList(id);
        else
        {
            AddToEntitiesList(id);
            AddToActiveList(id);
                
        }
    }

    /// <summary>
    /// Removing a component from the active entities
    /// </summary>
    /// <param name="component"></param>
    public void RemoveComponent(SystemComponent component)
    {
        ActiveEntityIDs.Remove(component.EntityID);
    }
    
    /// <summary>
    /// Assigning a unique (only 'in' this system) id to the component
    /// </summary>
    /// <param name="component"></param>
    private void AssignID(SystemComponent component)
    {
        component.EntityID = _idCounter;
        _entityIDs.Add(component.EntityID);
        _idCounter++;
    }
    
    /// <summary>
    /// Adding an entity to the active entities list
    /// </summary>
    /// <param name="id"></param>
    private void AddToActiveList(int id)
    {
        ActiveEntityIDs.Add(id);
    }
    
    /// <summary>
    /// Adding an entity to all entities list, this is necessary to prevent generating multiple ids
    /// if an entity goes inactive-active multiple times
    /// </summary>
    /// <param name="id"></param>
    private void AddToEntitiesList(int id)
    {
        _entityIDs.Add(id);
    }
    
    /// <summary>
    /// Clearing dirty data, including active/inactive entity ids & id counter
    /// This will be called in Engine's Awake because, GameSystem is inheriting from ScriptableObject
    /// </summary>
    private void ClearDirtyIDs()
    {
        _idCounter = 0;

        ActiveEntityIDs = new List<int>(MaxEntities);
        _entityIDs = new List<int>(MaxEntities);
    }

    #region MonoBehaviour Callbacks

    /// <summary>
    /// Called in Engine's MonoBehaviour Awake callback.
    /// </summary>
    public virtual void OnSystemAwake() => ClearDirtyIDs();

    /// <summary>
    /// Called in Engine's MonoBehaviour Start callback.
    /// </summary>
    public virtual void OnSystemStart() { }

    /// <summary>
    /// Called in Engine's MonoBehaviour Update callback.
    /// </summary>
    public virtual void OnSystemUpdate() { }
    
    /// <summary>
    /// Called in Engine's MonoBehaviour FixedUpdate callback.
    /// </summary>
    public virtual void OnSystemFixedUpdate() { }
    
    /// <summary>
    /// Called in Engine's MonoBehaviour LateUpdate callback.
    /// </summary>
    public virtual void OnSystemLateUpdate() { }

    /// <summary>
    /// Called in Engine's MonoBehaviour OnEnable callback.
    /// </summary>
    public virtual void OnSystemEnable() { }

    /// <summary>
    /// Called in Engine's MonoBehaviour OnDisable callback.
    /// </summary>
    public virtual void OnSystemDisable() { }

    /// <summary>
    /// Called in Engine's MonoBehaviour OnDestroy callback.
    /// </summary>
    public virtual void OnSystemDestroy() => ClearDirtyIDs();

    #endregion
}