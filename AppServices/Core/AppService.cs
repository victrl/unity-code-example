using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Core
{
    public class AppService : IService
    {
        public bool IsInitialized { get; private set; }
        protected AppContext Context { get; private set; }
        private readonly List<Type> dependencyTypes = new List<Type>();
        private readonly List<AppService> dependencies = new List<AppService>();

        public void SetContext(AppContext context)
        {
            Context = context;
        }

        public AppService AddDependency<T>()
        {
            dependencyTypes.Add(typeof(T));
            return this;
        }

        public async void PrepareService()
        {
            dependencies.Clear();
            while (dependencyTypes.Count > 0)
            {
                foreach (var dependencyType in dependencyTypes)
                {
                    var service = Context.TryGetService<AppService>(dependencyType);

                    if (service != null)
                    {
                        dependencies.Add(service);
                    }
                }

                foreach (var dependency in dependencies)
                {
                    dependencyTypes.Remove(dependency.GetType());
                }

                await Task.Delay(200);
            }

            Initialization();
        }

        protected virtual async void Initialization()
        {
            Inject();
            FinishInitialize();
        }

        protected virtual void FinishInitialize()
        {
            IsInitialized = true;
            Logger.Log($"[{GetType()}] => Initialization: Finished");
        }

        public virtual async void OnRegister()
        {
            Logger.Log($"[{GetType()}] => OnRegister");
        }

        public virtual void OnUnregister()
        {
            Logger.LogError($"[{GetType()}] => OnUnregister");
        }

        public virtual void OnPause(bool pauseStatus, DateTime pauseStartedTime)
        {
        }

        public virtual void OnCloseApp()
        {
        }

        public virtual void Inject()
        {
            DIInstaller.GlobalContainer.Inject(this);
        }
    }

    public class EmptyService : AppService
    {
    }
}