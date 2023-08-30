using System.Collections.Generic;
using UnityEngine;

namespace Survivors
{
    public class ClassUpdater : UtilityBase<ClassUpdater>
    {
        private List<IUpdatable> classesToUpdate;

        public override void Init()
        {
            classesToUpdate = new List<IUpdatable>();
        }

        public void AddClass(IUpdatable _class)
        {
            if (classesToUpdate.Contains(_class))
            {
                Debug.Log("Already updating this class " + _class.ToString());
                return;
            }
            classesToUpdate.Add(_class);
        }

        public void RemoveClass(IUpdatable _class)
        {
            if (!classesToUpdate.Contains(_class))
            {
                Debug.Log("Class not updating to be removed");
                return;
            }
            classesToUpdate.Remove(_class);
        }

        private void UpdateClasses()
        {
            if (classesToUpdate == null || classesToUpdate.Count == 0)
            {
                return;
            }
            for (int i = 0; i < classesToUpdate.Count; i++)
            {
                classesToUpdate[i].Update();
            }
        }

        private void LateUpdate()
        {
            UpdateClasses();
        }
    }
}
