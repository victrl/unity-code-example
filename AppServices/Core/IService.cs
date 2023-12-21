namespace App.Core
{
    public interface IService
    {
        void Inject();
        void OnRegister();
        void SetContext(AppContext context);
        void PrepareService();
        void OnUnregister();
        bool IsInitialized { get; }
    }
}