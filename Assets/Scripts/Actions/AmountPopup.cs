using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace LateUpdate {
    public class AmountPopup : MonoBehaviour
    {
        [SerializeField] InputField field;

        AmountCallback action;
        int value;
        int max;
        int min;

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

        void OnFieldChanged(string text)
        {
            int tmpVal;
            if (int.TryParse(text, out tmpVal))
                SetValue(tmpVal);
            else
                SetValue(value);
        }

        public void SetValue(int newValue)
        {
            if (newValue > max) newValue = max;
            if (newValue < min) newValue = min;

            value = newValue;
            field.text = value.ToString();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Submit")) Confirm();
            if (Input.GetButtonDown("Cancel")) Cancel();
        }
    }

    public abstract class AmountCallback
    {
        public abstract void Perform(int amount);
    }

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
}
