public class MovementComponent : SystemComponent
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