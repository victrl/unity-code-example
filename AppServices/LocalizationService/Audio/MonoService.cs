using App.Core;
using UnityEngine;

public class MonoService : MonoBehaviour, IService
{
    private AppContext context;
    
    public virtual void Inject()
    {
        DIInstaller.GlobalContainer.Inject(this);
        FinishInitialize();
    }

    protected virtual void FinishInitialize()
    {
        IsInitialized = true;
    }

    public void OnRegister()
    {
        Logger.Log($"[{GetType()}] => OnRegister");
    }

    public void SetContext(AppContext context)
    {
        this.context = context;
    }

    public void PrepareService()
    {
        Logger.Log($"[{GetType()}] => PrepareService");
    }

    public void OnUnregister()
    {
        Logger.Log($"[{GetType()}] => OnUnregister");
    }

    public bool IsInitialized { get; private set; }
}