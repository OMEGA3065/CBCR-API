namespace CustomRoleLib.API.DefaultComponents.Generic;

public interface IInitializable
{
    public void OnInitialized();
}

public class InitializerComponent<T> : ComponentBase<T>
    where T : RoleInstanceBase, IInitializable
{
    public override void OnCreatedInstance(T itemInstance)
    {
        itemInstance.OnInitialized();
    }
}