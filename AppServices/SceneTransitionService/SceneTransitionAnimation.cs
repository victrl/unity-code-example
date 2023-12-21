using DG.Tweening;
using UnityEngine;

namespace App.Core.AppServices
{
    public class SceneTransitionAnimation : MonoBehaviour
    {
        [SerializeField] private LoadingScreen loadingScreen;
        [SerializeField] private CanvasGroup transitionCanvas;
        
        public void ShowLoadingAnimation()
        {
            loadingScreen.StartAnimation();
        }
    
        public void HideLoadingAnimation()
        {
            loadingScreen.StopAnimation();
        }

        public Sequence HideScene()
        {
            var seq = DOTween.Sequence();
            transitionCanvas.alpha = 0;
            transitionCanvas.gameObject.SetActive(true);
            seq.Append(transitionCanvas.DOFade(1f, 0.5f));
            return seq;
        }

        public Sequence ShowScene()
        {
            var seq = DOTween.Sequence();
            seq.Append(transitionCanvas.DOFade(0f, 0.5f));
            seq.AppendCallback(() =>
            {
                transitionCanvas.gameObject.SetActive(false);
            });
            return seq;
        }

        public void Transition(string sceneName)
        {
        
        }
    }
}
