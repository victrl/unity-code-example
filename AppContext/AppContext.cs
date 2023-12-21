using System;
using System.Threading.Tasks;
using App.Constants;
using App.Core.AppServices;
using App.Core.AppServices.ReadWriteSaveServices;
using UnityEngine;

namespace App.Core
{
    public class AppContext : InitializationContext
    {
        [Header("Services")] 
        [SerializeField] private SceneTransitionAnimation sceneTransitionAnimation;
        [SerializeField] private AudioStorageService audioStorageService;
        [SerializeField] private UIService uiService;

        public SceneTransitionAnimation SceneTransitionAnimation => sceneTransitionAnimation;
        public AudioStorageService AudioStorageService => audioStorageService;
        private ServicesStorage servicesStorage = new ServicesStorage();

        public static bool IsInitialized { get; private set; }

        protected override void Init()
        {
            sceneTransitionAnimation.ShowLoadingAnimation();
            RegistrationServices();
            base.Init();
        }

        private void RegistrationServices()
        {
            RegisterService<IReadWriteSaveService>(new PlayerPrefsReadWriteSaveService());
            RegisterService<ProfileSaveService>().AddDependency<IReadWriteSaveService>();
            RegisterService<RewardService>();
            RegisterService<SceneTransitionService>();
            RegisterService<LocalizationService>();

            RegisterMonoServices(audioStorageService, true);
            RegisterMonoServices(uiService, true);

            CheckFinishInitialization(() =>
            {
                IsInitialized = true;
                sceneTransitionAnimation.HideLoadingAnimation();
                var sceneTransitionService = servicesStorage.GetService<SceneTransitionService>();
                sceneTransitionService.OpenScene(SceneNames.GameMenu);
            });
        }

        private T RegisterService<T>() where T : AppService, new()
        {
            var service = ServicesCreator.CreateService<T>(this);
            servicesStorage.RegisterService(service);
            return service;
        }
        
        private T RegisterService<T>(T service) where T : IService
        {
            ServicesCreator.CreateService(this, service);
            servicesStorage.RegisterService(service);
            return service;
        }

        private void RegisterMonoServices<T>(T service, bool asSingle = false) where T : MonoBehaviour, IService
        {
            ServicesCreator.InitMonoServices(service, asSingle);
        }

        public T TryGetService<T>(Type type) where T : class, IService, new()
        {
            return servicesStorage.GetService<T>(type);
        }

        private async void CheckFinishInitialization(Action onCompleted)
        {
            while (servicesStorage.ServicesWasInitialized() == false)
            {
                await Task.Delay(500);
            }

            onCompleted?.Invoke();
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            foreach (var service in servicesStorage)
            {
                (service as AppService)?.OnPause(pauseStatus, DateTime.Now);
            }
        }

        private void OnDestroy()
        {
            foreach (var service in servicesStorage)
            {
                (service as AppService)?.OnCloseApp();
            }
        }
    }
}