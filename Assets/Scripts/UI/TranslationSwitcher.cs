using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public class TranslationSwitcher : MonoBehaviour, ISwitchable
    {
        [Header("State")]
        [SerializeField] bool isOpenAtStart;

        [Header("Animation")]
        [SerializeField] Vector2 closedPosition;
        [SerializeField] Vector2 openedPosition;
        [SerializeField] float transitionTime = 0.5f;        

        float factor;

        public RectTransform RectTransform { get; private set; }
        public bool IsOpen => factor > 0;

        public void Close()
        {
            StopAllCoroutines();
            StartCoroutine(Transition(false));
        }

        public void Open()
        {
            StopAllCoroutines();
            StartCoroutine(Transition(true));
        }

        public void Switch()
        {
            ISwitchableExtensions.Switch(this);
        }

        void Init()
        {
            if (isOpenAtStart)
            {
                RectTransform.anchoredPosition = openedPosition;
                factor = 1;
            }
            else
            {
                RectTransform.anchoredPosition = closedPosition;
                factor = 0;
            }
        }

        IEnumerator Transition(bool open)
        {
            while((open && factor != 1) || (!open && factor != 0))
            {
                if (open)
                {
                    factor += Time.deltaTime / transitionTime;
                    if (factor > 1)
                        factor = 1;
                }
                else
                {
                    factor -= Time.deltaTime / transitionTime;
                    if (factor < 0)
                        factor = 0;
                }

                RectTransform.anchoredPosition = Vector2.Lerp(closedPosition, openedPosition, factor);
                yield return null;
            }
        }

        public void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
            Init();
        }
    }
}
