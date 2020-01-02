using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace LateUpdate {
    /// <summary>
    /// A contextual popup that allows the player to choose an amount of anything before calling <see cref="AmountCallback"/>
    /// </summary>
    public class AmountPopup : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] InputField field;
        #endregion

        #region Private Fields
        AmountCallback action;
        int value;
        int max;
        int min;
        #endregion

        #region Public Methods
        public void Configure(AmountCallback action, int defaultAmount, int maxAmount = 100, int minAmount = 0)
        {
            this.action = action;

            max = maxAmount;
            min = minAmount;

            SetValue(defaultAmount);
            field.onEndEdit.AddListener(OnFieldChanged);
            field.Select();
        }

        public void AddToValue(int amount)
        {
            SetValue(value + amount);
        }

        public void Confirm()
        {
            action.Perform(value);
            Destroy(gameObject);
        }

        public void Cancel()
        {
            Destroy(gameObject);
        }

        public void SetValue(int newValue)
        {
            if (newValue > max) newValue = max;
            if (newValue < min) newValue = min;

            value = newValue;
            field.text = value.ToString();
        }
        #endregion

        #region Private Methods
        void OnFieldChanged(string text)
        {
            int tmpVal;
            if (int.TryParse(text, out tmpVal))
                SetValue(tmpVal);
            else
                SetValue(value);
        }
        #endregion

        #region Runtime Methods
        private void Update()
        {
            if (Input.GetButtonDown("Submit")) Confirm();
            if (Input.GetButtonDown("Cancel")) Cancel();
        }
        #endregion
    }

    #region Container Classes
    /// <summary>
    /// This class contains the action and the target used by <see cref="AmountPopup"/>, use <see cref="AmountCallback{TTarget}"/> to set the target's type
    /// </summary>
    public abstract class AmountCallback
    {
        public abstract void Perform(int amount);
    }

    /// <summary>
    /// This class contains the action and the target used by <see cref="AmountPopup"/>
    /// </summary>
    /// <typeparam name="TTarget">The type of the target</typeparam>
    public class AmountCallback<TTarget> : AmountCallback
    {
        Action<TTarget, int> action;
        TTarget target;

        public AmountCallback(Action<TTarget, int> callback, TTarget target)
        {
            action = callback;
            this.target = target;
        }

        public override void Perform(int amount)
        {
            action.Invoke(target, amount);
        }
    }
    #endregion
}
