using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LateUpdate.Stats.UI {
    public class StatUIText : MonoBehaviour
    {
        [SerializeField] bool useFullName = true;
        [SerializeField] Text label;
        [SerializeField] Text value;
        [SerializeField] Text bonus;
        [SerializeField] Color positiveBonusColor;
        [SerializeField] Color negativeBonusColor;

        Stat stat;

        public Stat Stat {
            get => stat;
            set
            {
                stat = value;
                RefreshDisplay();
            }
        }

        public void RefreshDisplay()
        {
            label.text = useFullName? Stat.Name : Stat.ShortName;
            value.text = stat.IntValue.ToString() + stat.Unit;

            ModifiableStat modStat = Stat as ModifiableStat;
            if (modStat != null)
            {
                int bonusValue = modStat.IntBonusValue;
                if (bonusValue > 0)
                {
                    bonus.text = string.Format("(+{0}{1})", bonusValue, stat.Unit);
                    bonus.color = positiveBonusColor;
                }
                else if (bonusValue < 0)
                {
                    bonus.text = string.Format("(-{0}{1})", bonusValue, stat.Unit);
                    bonus.color = negativeBonusColor;
                }
                else
                {
                    bonus.text = "";
                    bonus.color = value.color;
                }
            }
            else
            {
                bonus.text = "";
            }
        }
    }
}
