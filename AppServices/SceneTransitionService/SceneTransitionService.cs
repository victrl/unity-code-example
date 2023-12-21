using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace App.Core.AppServices
{
    public class SceneTransitionService : AppService
    {
        private SceneTransitionAnimation sceneTransitionAnimation;
        private Dictionary<string, object> transitionData = new Dictionary<string, object>();
        private Stack<string> sceneHistory = new Stack<string>();

        public override void OnRegister()
        {
            sceneTransitionAnimation = Context.SceneTransitionAnimation;
            base.OnRegister();
        }

        public void OpenScene(string sceneName, object data = null)
        {
            if (data != null)
            {
                transitionData[sceneName] = data;
            }

            var seq = DOTween.Sequence();
            seq.Append(sceneTransitionAnimation.HideScene());
            seq.AppendCallback(() =>
            {
                sceneHistory.Push(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(sceneName);
            });

            seq.Append(sceneTransitionAnimation.ShowScene());
        }

        public void ToBack()
        {
            string sceneName = sceneHistory.Pop();

            if (string.IsNullOrEmpty(sceneName))
            {
                Logger.LogWarning($"[SceneTransitionService] => Back: This scene is first");
                return;
            }

            var seq = DOTween.Sequence();
            seq.Append(sceneTransitionAnimation.HideScene());
            seq.AppendCallback(() => { SceneManager.LoadScene(sceneName); });
            seq.Append(sceneTransitionAnimation.ShowScene());
        }
    }
}