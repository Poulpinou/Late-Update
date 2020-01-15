using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate.Stats.UI {
    public class StatUIBlock : MonoBehaviour
    {
        [SerializeField] Stat.StatCategory category;
        [SerializeField] Text categoryName;
        [SerializeField] RectTransform statUITextsParent;
        [SerializeField] StatUIText statTextModel;

        Stat[] stats;

        public Stat[] Stats
        {
            get => stats;
            set
            {
                stats = value;
                RefreshDisplay();
            }
        }

        public Stat.StatCategory Category => category;

        public void RefreshDisplay()
        {
            StatUIText[] texts = statUITextsParent.GetComponentsInChildren<StatUIText>();

            for (int i = 0; i < stats.Length || i < texts.Length; i++)
            {
                if(i < stats.Length)
                {
                    if(i < texts.Length)
                    {
                        texts[i].Stat = stats[i];
                    }
                    else
                    {
                        StatUIText statText = Instantiate(statTextModel, statUITextsParent);
                        statText.Stat = stats[i];
                    }
                }
                else
                {
                    Destroy(texts[i].gameObject);
                }
            }
        }

        private void OnValidate()
        {
            name = category + "StatBlock";
            categoryName.text = category.ToString();
        }
    }
}
