using System.Collections.Generic;
using System.Linq;
using App.Core.AppServices;
using UnityEngine;

public class AudioStorageService : MonoService
{
    public List<ClipContainer> ClipContainers = new List<ClipContainer>();

    public ClipContainer GetClipContainer(SystemLanguage language)
    {
        var clipContainer = ClipContainers.Find(container => container.name.Equals(language.ToString()));

        if (clipContainer == null)
        {
            Logger.LogWarning($"[AudioStorageService] => GetClipContainer: Unsupported language {language.ToString()}");
            return ClipContainers.First();
        }

        return clipContainer;
    }
}