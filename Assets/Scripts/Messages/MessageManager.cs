using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace LateUpdate {
    /// <summary>
    /// This class allows you to easily send messages at runtime (through <see cref="MessageUI"/>)
    /// </summary>
    public class MessageManager : StaticManager<MessageManager>
    {
        #region Serialized Fields
        [Header("Relations")]
        [SerializeField] MessageUI ui;

        [Header("Exceptions")]
        [SerializeField] bool catchExceptions = true;
        #endregion

        #region Private Fields
        List<Message> messages;
        #endregion

        #region Static Methods
        /// <summary>
        /// Sends a message to the system
        /// </summary>
        /// <param name="message">The content of the message</param>
        /// <param name="type">The type of the message</param>
        public static void Send(string message, LogType type)
        {
            Send(new Message(message, type));
        }

        /// <summary>
        /// Sends a message to the system
        /// </summary>
        /// <param name="message">The message to send</param>
        public static void Send(Message message)
        {
            Active.messages.Add(message);
            Active.ui.Refresh();
        }

        /// <summary>
        /// Request some message from filter
        /// </summary>
        /// <param name="amount">The amount of expected messages</param>
        /// <param name="filter">The filtering function</param>
        /// <returns>An array of messages</returns>
        public static Message[] RequestMessages(int amount, Func<Message, bool> filter)
        {
            return Active.messages.Where(filter).Take(amount).ToArray();
        }
        #endregion

        #region Private Methods
        void LogCallback(string message, string stackTrace, LogType type)
        {
            if (catchExceptions)
                Send(new Message(message, stackTrace, type));
        }
        #endregion

        #region Runtime Methods
        void OnEnable()
        {
            Application.logMessageReceived += LogCallback;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= LogCallback;
        }

        protected override void Awake()
        {
            base.Awake();
            messages = new List<Message>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                throw new Exception("Ma bite");

            if (Input.GetKeyDown(KeyCode.A))
                Send(new Message("Plein de blabla, des tonnes de blabla histoire de voir si tout s'affiche correctement. A vrai dire si ça fait de la merde j'ai le seum... Ouais pas très français mais on s'en balec!", LogType.Log));
        }
        #endregion
    }
}
