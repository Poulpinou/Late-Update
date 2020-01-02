using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate {
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] Vector2 padding;
        [SerializeField] Text text;
        [SerializeField] RectTransform background;

        public static Tooltip Instance;

        public static void Show(string value)
        {
            Instance.text.text = value;
            Instance.background.sizeDelta = new Vector2(
                Instance.text.preferredWidth + Instance.padding.x * 2,    
                Instance.text.preferredHeight + Instance.padding.y * 2   
            );
            Instance.background.anchoredPosition = -Instance.padding;
            Instance.text.gameObject.SetActive(true);
            Instance.background.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            Instance.text.gameObject.SetActive(false);
            Instance.background.gameObject.SetActive(false);
        }

        private void Awake()
        {
            Instance = this;
            Hide();
        }

        private void Update()
        {
            transform.position = Input.mousePosition;   
        }
    }
}
