using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LateUpdate {
    public interface ISwitchable
    {
        bool IsOpen { get; }

        void Open();
        void Close();
    }

    public static class ISwitchableExtensions
    {
        public static void Switch(this ISwitchable switchable)
        {
            SetOpen(switchable, !switchable.IsOpen);
        }

        public static void SetOpen(this ISwitchable switchable, bool open)
        {
            if (open)
                switchable.Open();
            else
                switchable.Close();
        }
    }
}
