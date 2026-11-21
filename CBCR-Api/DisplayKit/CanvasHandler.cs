using LabApi.Features.Console;
using Mirror;

namespace CustomRoleLib.DisplayKit;

internal class CanvasHandler<T> where T : ICanvasReference
{
    private bool _loaded;
    public T Reference { get; }
    public event Action OnConstructed;

    public CanvasHandler(T reference, bool loaded = false)
    {
        Reference = reference;
        _loaded = loaded;
        if (_loaded) return;
        Reference.DisplayCanvas.OnPlayerCanvasConstructed += OnCanvasConstructed;
    }

    private void OnCanvasConstructed(NetworkConnectionToClient connection)
    {
        _loaded = true;
        OnConstructed?.Invoke();
    }

    public void Effect(Action action)
    {
        if (_loaded)
        {
            action();
            return;
        }

        OnConstructed += action;
    }
}