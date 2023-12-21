using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Common.Configs;
using App.Core.AppServices.ConfigLoaders;
using UnityEngine;

namespace App.Core.AppServices
{
    public class LocalizationService : AppService
    {
        public static LocalizationService Current;

        private IConfigLoader configLoader;
        private Dictionary<string, LanguageData> localizationData;

        private SystemLanguage currentLocal;

        public SystemLanguage CurrentLocal
        {
            get => currentLocal;
            set
            {
                currentLocal = value;
                OnSwitchLocalization();
            }
        }

        private Dictionary<SystemLanguage, Func<LanguageData, string>> languageGetter;
        private ClipContainer clipContainer;

        protected override async void Initialization()
        {
            if (Current != null)
            {
                Logger.LogError($"[LocalizationService] => Initialization: service was initialized");
                return;
            }
            
            Current = this;
            configLoader = new LocalConfigLoader();
            configLoader.LoadData<Dictionary<string, LanguageData>>(AppPaths.Configs.LocalizationConfigPath, (data) => { localizationData = data; });
            InitLanguageGetter();
            LoadClipContainer();

            await CheckDependencies();
            base.Initialization();
        }

        private void OnSwitchLocalization()
        {
            LoadClipContainer();
        }

        private void LoadClipContainer()
        {
            clipContainer = Context.AudioStorageService.GetClipContainer(CurrentLocal);
        }
        
        private async Task CheckDependencies()
        {
            await Task.Delay(2000); // simulate async load
        }

        private void InitLanguageGetter()
        {
            languageGetter = new Dictionary<SystemLanguage, Func<LanguageData, string>>
            {
                {SystemLanguage.English, data => data.EN},
                {SystemLanguage.Russian, data => data.RU},
                {SystemLanguage.German, data => data.DE}
            };
        }

        public string GetText(string uid)
        {
            if (localizationData == null)
            {
                Logger.LogError("[LocalizationService] => Get: localizationData is null");
                return uid;
            }

            if (localizationData.TryGetValue(uid, out var textData))
            {
                return languageGetter[CurrentLocal]?.Invoke(textData);
            }

            Logger.LogError("[LocalizationService] => Get: uid is null");
            return uid;
        }

        public AudioClip GetAudion(string uid)
        {
            return clipContainer.GetAudioClip(uid);
        }

        public override void OnUnregister()
        {
            if (Current.Equals(this))
            {
                Current = null;
            }

            base.OnUnregister();
        }
    }
}