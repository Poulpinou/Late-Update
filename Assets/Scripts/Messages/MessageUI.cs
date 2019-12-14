using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace LateUpdate {
    public class MessageUI : MonoBehaviour
    {
        [Header("Relations")]
        [SerializeField] Text textModel;
        [SerializeField] Transform textsParent;

        [Header("Display")]
        [SerializeField] int maxMessagesCount = 100;
        [Tooltip("0 => Message, 1 => Time, 2 => Type, 3 => Stack Trace")]
        [SerializeField] string testFormat = "<b>{1}</b> - <i>{0}</i> ( {2} {3} )";
        [SerializeField] bool displayStackTrace = false;

        [Header("Colors")]
        [SerializeField] Color messageColor = Color.white;
        [SerializeField] Color errorColor = Color.red;

        Func<Message, bool> filter = f => true;

        public void Refresh()
        {
            Text[] texts = textsParent.GetComponentsInChildren<Text>();
            Message[] messages = MessageManager.RequestMessages(maxMessagesCount, filter);

            for (int i = 0; i < messages.Length || i < texts.Length; i++)
            {
                if (i < messages.Length)
                {
                    if (i < texts.Length)
                    {
                        ConfigureText(texts[i], messages[i]);
                    }
                    else
                    {
                        Text text = Instantiate(textModel, textsParent);
                        ConfigureText(text, messages[i]);
                    }
                }
                else
                {
                    Destroy(texts[i].gameObject);
                }
            }
        }

        void ConfigureText(Text text, Message message)
        {
            text.text = string.Format(testFormat, message.Content, message.Time.ToString("hh:mm:ss"), message.LogType, displayStackTrace? message.StackTrace : "");
            text.color = message.LogType == LogType.Error || message.LogType == LogType.Exception ? errorColor : messageColor;
        }

        public void DisplayErrorToggle(bool value)
        {
            if (value)
            {
                filter = f => true;
            }
            else
            {
                filter = f => f.LogType != LogType.Error && f.LogType != LogType.Exception;
            }
            Refresh();
        }
    }
}
