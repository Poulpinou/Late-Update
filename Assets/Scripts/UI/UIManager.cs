using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace LateUpdate {
    public class UIManager : StaticManager<UIManager>
    {
        [Header("Settings")]
        [SerializeField] Vector2 defaultFloatingWindowPosition;

        [Header("Relations")]
        [SerializeField] RectTransform panelsRoot;

        [Header("Models")]
        [SerializeField] FloatingWindow floatingWindowModel;
        [SerializeField] UIPanel[] panelModels;

        public static FloatingWindow FloatingWindowModel => Active.floatingWindowModel;
        public static RectTransform PanelsRoot => Active.panelsRoot;

        public static Vector2 DefaultFloatingWindowPosition => Active.defaultFloatingWindowPosition;

        public static TPanel GetPanelModel<TPanel>(Func<TPanel, bool> filter = null) where TPanel : UIPanel
        {
            var query = Active.panelModels.Where(p => p is TPanel).Cast<TPanel>();

            if (filter != null)
                query = query.Where(filter);

            return query.FirstOrDefault();
        }

        public static TPanel CreatePanel<TPanel>(Action<TPanel> configureAction = null) where TPanel : UIPanel
        {
            return CreatePanel(null, configureAction);
        }

        public static TPanel CreatePanel<TPanel>(Func<TPanel, bool> filter, Action<TPanel> configureAction = null) where TPanel : UIPanel
        {
            TPanel panel = GetPanelModel(filter);

            if (panel == null)
                throw new Exception(string.Format("No panel of type {0} found for filter {1}, check if it exists in {2} PanelModels list", typeof(TPanel).Name, filter, Active.name));

            if (panel.IsUnique)
            {
                TPanel[] panels = PanelsRoot.GetComponentsInChildren<TPanel>();
                for (int i = 0; i < panels.Length; i++)
                {
                    panels[i].Close();
                }
            }

            panel = Instantiate(panel, PanelsRoot);

            if(configureAction != null)
            {
                configureAction.Invoke(panel);
            }

            return panel;
        }

        public static FloatingWindow CreateFloatingPanel<TPanel>(Vector2 position, Action<TPanel> configureAction = null) where TPanel : UIPanel
        {
            return CreateFloatingPanel(position, null, configureAction);
        }

        public static FloatingWindow CreateFloatingPanel<TPanel>(Vector2 position, Func<TPanel, bool> filter, Action<TPanel> configureAction = null) where TPanel : UIPanel
        {
            TPanel panel = CreatePanel(filter, configureAction);
            FloatingWindow window = Instantiate(FloatingWindowModel, PanelsRoot);
            window.transform.position = position;
            window.AppendPanel(panel);

            return window;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
                CreateFloatingPanel<UIPanel>(Input.mousePosition);
        }
    }
}
