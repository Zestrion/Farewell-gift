using System.Collections.Generic;
using UnityEngine;

namespace Survivors
{
    public class ObjectsPooler<T>
        where T : Object
    {
        public List<T> UsedObjects => usedObjects;

        private T objectsPrefab;
        private Transform spawnedObjectsParent;

        private Queue<T> notUsedObjects;
        private List<T> usedObjects;

        internal ObjectsPooler(T _objectsPrefab, Transform _parent)
        {
            objectsPrefab = _objectsPrefab;
            spawnedObjectsParent = _parent;
            notUsedObjects = new Queue<T>();
            usedObjects = new List<T>();
        }

        internal T GetObject()
        {
            T _object;
            if (notUsedObjects.Count > 0)
            {
                _object = notUsedObjects.Dequeue();
            }
            else
            {
                _object = MonoBehaviour.Instantiate(objectsPrefab, spawnedObjectsParent);
            }
            usedObjects.Add(_object);
            return _object;
        }

        internal void MakeObjectFree(T _object)
        {
            if (usedObjects.Contains(_object))
            {
                usedObjects.Remove(_object);
                notUsedObjects.Enqueue(_object);
            }
            else
            {
                Debug.LogError("Cannot make the object free because the used objects does not contain this object: " + _object.name);
            }
        }

        internal void MakeAllObjectsFree()
        {
            for (int i = 0; i < usedObjects.Count; i++)
            {
                notUsedObjects.Enqueue(usedObjects[i]);
            }
            usedObjects.Clear();
        }
    }
}
