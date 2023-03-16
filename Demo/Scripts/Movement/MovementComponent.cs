public class MovementComponent : GameComponent
{
    public float speed;

    private void OnEnable()
    {
        RegisterComponent();
    }

    private void OnDisable()
    {
        RemoveComponent();
    }
}