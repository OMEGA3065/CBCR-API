namespace CustomRoleLib.Compat;

internal static class RaCustomMenuCompat
{
    private static bool _hasInicialized = false;
    public static void Init()
    {
        if (_hasInicialized) return;
        RaCustomMenu.API.Provider.RegisterProvider(new RaCustomMenuCompatability.RoleNamespaceProvider());
        _hasInicialized = true;
    }
}