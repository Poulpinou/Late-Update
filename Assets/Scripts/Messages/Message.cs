using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    /// <summary>
    /// This class is a data container for <see cref="MessageManager"/>
    /// </summary>
    public class Message
    {
        public string Content { get; private set; }
        public string StackTrace { get; private set; }
        public LogType LogType { get; private set; }
        public DateTime Time { get; private set; }

        public Message(string content, LogType logType = LogType.Log)
        {
            Content = content;
            LogType = logType;
            Time = DateTime.Now;
        }

        public Message(string content, string stackTrace, LogType logType = LogType.Log) : this(content, logType)
        {
            StackTrace = stackTrace;
        }
    }
}
