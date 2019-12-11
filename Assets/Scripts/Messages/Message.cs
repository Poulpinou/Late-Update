using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    /// <summary>
    /// Temp version of messaging system
    /// </summary>
    public class Message
    {
        public static void Send(string message)
        {
            Debug.Log(message);
        }
    }
}
