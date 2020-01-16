using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace LateUpdate {
    public class WorldObjectManager : StaticManager<WorldObjectManager>
    {
        [SerializeField] Transform worldObjectsRoot;

        Dictionary<string, List<WorldObject>> collections = new Dictionary<string, List<WorldObject>>();

        public static Transform WorldObjectsRoot => Active.worldObjectsRoot;
        public Dictionary<string, List<WorldObject>> Collections => collections;

        public static void AddToCollections(WorldObject worldObject)
        {
            foreach(string tag in worldObject.GetCollectionTags())
            {
                if (!Active.collections.ContainsKey(tag))
                {
                    Active.collections.Add(tag, new List<WorldObject>());
                }

                if (!Active.collections[tag].Contains(worldObject))
                {
                    Active.collections[tag].Add(worldObject);
                }
            }
        }

        public static void RemoveFromCollection(WorldObject worldObject)
        {
            foreach (string tag in worldObject.CollectionTags)
            {
                if (Active.collections.ContainsKey(tag))
                {
                    Active.collections[tag].Remove(worldObject);
                }
            }
        }

        public static TObject RequestObject<TObject>(string collectionTag, Func<TObject, bool> predicate) where TObject : WorldObject
        {
            return Active.collections[collectionTag].Where(o => o is TObject).Cast<TObject>().Where(predicate).FirstOrDefault();
        }

        public static TObject[] RequestObjects<TObject>(string collectionTag, Func<TObject, bool> predicate) where TObject : WorldObject
        {
            return Active.collections[collectionTag].Where(o => o is TObject).Cast<TObject>().Where(predicate).ToArray();
        }

        public static WorldObject RequestObject(string collectionTag, Func<WorldObject, bool> predicate)
        {
            return Active.collections[collectionTag].Where(predicate).FirstOrDefault();
        }

        public static WorldObject[] RequestObjects(string collectionTag, Func<WorldObject, bool> predicate)
        {
            return Active.collections[collectionTag].Where(predicate).ToArray();
        }

        public static TObject RequestObject<TObject>(string collectionTag, Func<IEnumerable<TObject>, IEnumerable<TObject>> request) where TObject : WorldObject
        {
            return request.Invoke(Active.collections[collectionTag].Where(o => o is TObject).Cast<TObject>()).FirstOrDefault();
        }
    }
}
