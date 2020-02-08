using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LateUpdate {
    public abstract class WorldObject : MonoBehaviour, ICollectionTaggable
    {
        public abstract string[] CollectionTags { get; }
        public WorldObjectComponent[] Components => GetComponents<WorldObjectComponent>();


        public bool TryGetWorldObjectComponent<TComponent>(out TComponent component) where TComponent : WorldObjectComponent
        {
            component = GetWorldObjectComponent<TComponent>();
            return component != null;
        }

        public bool TryGetWorldObjectComponents<TComponent>(out TComponent[] components) where TComponent : WorldObjectComponent
        {
            components = GetWorldObjectComponents<TComponent>();
            return components != null && components.Length > 0;
        }

        public TComponent GetWorldObjectComponent<TComponent>() where TComponent : WorldObjectComponent
        {
            return GetComponent<TComponent>();
        }

        public TComponent[] GetWorldObjectComponents<TComponent>() where TComponent : WorldObjectComponent
        {
            return GetComponents<TComponent>();
        }

        public bool DoOnWorldObjectComponent<TComponent>(Action<TComponent> action) where TComponent : WorldObjectComponent
        {
            TComponent component;
            if (!TryGetWorldObjectComponent(out component)) return false;
            action.Invoke(component);
            return true;
        }

        public bool DoOnWorldObjectComponent<TComponent>(Func<TComponent, bool> action) where TComponent : WorldObjectComponent
        {
            TComponent component;
            if (!TryGetWorldObjectComponent(out component)) return false;
            return action.Invoke(component);
        }

        public string[] GetCollectionTags()
        {
            List<string> tags = new List<string>();
            foreach (ICollectionTaggable taggable in GetComponents<ICollectionTaggable>())
            {
                tags.AddRange(taggable.CollectionTags);
            }
            return tags.ToArray();
        }

        protected virtual void Awake()
        {
            WorldObjectManager.AddToCollections(this);

            if (transform.parent == null)
                transform.parent = WorldObjectManager.WorldObjectsRoot;
        }

        protected virtual void Reset()
        {
            WorldObject[] previous = GetComponents<WorldObject>();
            for (int i = 0; i < previous.Length; i++)
            {
                if (previous[i] != this)
                {
                    string message = string.Format(
                        "Can't add {0} component, {1} already have a WorldObject attached on it : {2}",
                        GetType().Name,
                        gameObject.name,
                        previous.GetType().Name
                    );
                    DestroyImmediate(this);
                    throw new Exception(message);
                }
            }
        }

        protected virtual void OnDestroy()
        {
            WorldObjectManager.RemoveFromCollection(this);
        }
    }
}
