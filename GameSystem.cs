using System.Collections.Generic;

public abstract class GameSystem : UnityEngine.ScriptableObject
{
    protected List<int> ActiveEntityIDs;
    
    private List<int> _entityIDs;
    
    private int _idCounter = 0;
    
    protected abstract int MaxEntities { get; }

    public virtual void RegisterComponent(GameComponent component)
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

    public void RemoveComponent(GameComponent component)
    {
        ActiveEntityIDs.Remove(component.EntityID);
    }
    
    private void AssignID(GameComponent component)
    {
        component.EntityID = _idCounter;
        _entityIDs.Add(component.EntityID);
        _idCounter++;
    }
    
    private void AddToActiveList(int id)
    {
        ActiveEntityIDs.Add(id);
    }
    
    private void AddToEntitiesList(int id)
    {
        _entityIDs.Add(id);
    }
    
    private void ClearDirtyIDs()
    {
        _idCounter = 0;

        ActiveEntityIDs = new List<int>(MaxEntities);
        _entityIDs = new List<int>(MaxEntities);
    }

    #region MonoBehaviour Callbacks

    public virtual void OnAwake() => ClearDirtyIDs();

    public virtual void OnStart() { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

    public virtual void OnSystemEnable() { }

    public virtual void OnSystemDisable() { }

    #endregion
}