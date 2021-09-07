using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;


namespace NubianVR.UI
{
    public class SplashUI_Screen : UI_Screen 
    {
        #region Variables
        public VideoPlayer videoPlayer;
        public RawImage splashVideoDisplay;
        public VideoClip[] splashVideos;
        public UnityEvent onSplashVideosFinished = new UnityEvent();
        
        private int _currentIndex;
        private int _nextIndex;
        #endregion

        #region MainMethods

        void Update()
        {
           // videoPlayer.loopPointReached += player => { PlayNextVideo();};
        }

        #endregion
        #region HelperMethods

        public override void StartScreen()
        {
            base.StartScreen();
            videoPlayer.loopPointReached += player =>
            {
                PlayNextVideo();
            };
        }

        public void PlayNextVideo()
        {
            StopCoroutine(playVideo());

            if (_nextIndex >= splashVideos.Length)
            {
                print("Next Screen");
                onSplashVideosFinished.Invoke();
            }
            else
            {
                _currentIndex = _nextIndex;
                _nextIndex++;
                videoPlayer.clip = splashVideos[_currentIndex];
                StartCoroutine(playVideo());
            }
        }
        
        IEnumerator playVideo()
        {
            videoPlayer.Prepare();
            while (!videoPlayer.isPrepared)
            {
                yield return null;
            }

            splashVideoDisplay.texture = videoPlayer.texture;
            videoPlayer.Play();
        }
        
        #endregion
    }
}
