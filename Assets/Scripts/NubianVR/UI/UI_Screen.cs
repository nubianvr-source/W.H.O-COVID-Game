using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace NubianVR.UI
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class UI_Screen : MonoBehaviour
    {
        #region Variables

        [Header("Main Properties")] 
        public Selectable m_StartSelectable;

        
        [Header("Screen Events")]
        public UnityEvent onScreenStart = new UnityEvent();
        public UnityEvent onScreenClose = new UnityEvent();
        
        private Animator animator;
        private CanvasGroup canvasGroup;
        
        #endregion

        #region MainMethods
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            if (m_StartSelectable)
            {
                EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
            }

          

        }

        // Update is called once per frame
        void Update()
        {

        }
        
        #endregion
        
        
        #region HelperMethods

        public virtual void StartScreen()
        {
            if (onScreenStart != null)
            {
                onScreenStart?.Invoke();
            }
            
            HandleAnimator("show");
            //canvasGroup.alpha = 1;
        }

        public virtual void CloseScreen()
        {
            if (onScreenClose != null)
            {
                onScreenClose?.Invoke();
            }
                
            HandleAnimator("hide");
            //canvasGroup.alpha = 0;
        }

        void HandleAnimator(string aTrigger)
        {
            if (animator)
            {
                print("Animation Trigger = " + aTrigger);
                animator.SetTrigger(aTrigger);
            }
        }

        #endregion
       
    }
}
