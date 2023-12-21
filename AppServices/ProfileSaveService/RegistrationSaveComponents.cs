using System;
using System.Collections.Generic;

namespace App.Core.AppServices
{
    public partial class ProfileSaveService
    {
        private void RegistrationSaveComponents()
        {
            Register<RewardSaveComponent>();
        }

        private void Register<T>() where T : SaveComponent, new()
        {
            if (components == null)
            {
                components = new Dictionary<Type, SaveComponent>();
            }
            
            if (components.TryGetValue(typeof(T), out var component))
            {
                Logger.Log($"[ProfileSaveService] => Register: {typeof(T)} was registered: \n {component}");
                return;
            }

            components.Add(typeof(T), new T());
        }
    }
}