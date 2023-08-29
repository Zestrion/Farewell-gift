using Spine.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Spine.Unity.Editor
{
    public static class BoneFollowerInspectorExtension
    {
        static bool foldoutOpen = false;
        static int maxParentsShown = 5;
        static bool includeParentInSearch = false;
        static string searchValue = "";

        public static void BoneSearch(this BoneFollowerInspector inspector)
        {
            var skeletonRenderer = inspector.serializedObject.FindProperty("skeletonRenderer");

            EditorGUILayout.Space();

            foldoutOpen = EditorGUILayout.BeginFoldoutHeaderGroup(foldoutOpen, "Bone Search");
            if (!foldoutOpen)
            {
                return;
            }
            EditorGUI.indentLevel = 1;

            includeParentInSearch = EditorGUILayout.Toggle("Include Parents in Search: ", includeParentInSearch);

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            searchValue = EditorGUILayout.TextField(searchValue);
            EditorGUILayout.EndHorizontal();

            maxParentsShown = EditorGUILayout.IntSlider("Max Parents Shown: ", maxParentsShown, 0, 20);

            SkeletonRenderer skeletonRef = skeletonRenderer.objectReferenceValue as SkeletonRenderer;
            Spine.ExposedList<Spine.BoneData> bones = skeletonRef.skeletonDataAsset.GetSkeletonData(quiet: true).Bones;

            foreach (Spine.BoneData bone in bones.Items)
            {
                string jointName = bone.Name;
                string fullName = bone.Name;
                var iterator = bone;
                int parentsAdded = 0;
                while ((iterator = iterator.Parent) != null)
                {
                    fullName = string.Format("{0}/{1}", iterator.Name, fullName);
                    if (parentsAdded < maxParentsShown)
                    {
                        jointName = string.Format("{0}/{1}", iterator.Name, jointName);
                        parentsAdded++;
                    }
                }

                if (includeParentInSearch && !fullName.Contains(searchValue)) { continue; }
                if (!includeParentInSearch && !bone.Name.Contains(searchValue)) { continue; }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button('\u25B6'.ToString(), GUILayout.Width(50)))
                {
                    inspector.serializedObject.FindProperty("boneName").stringValue = bone.Name;
                    inspector.serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.LabelField(jointName, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
            EditorGUI.indentLevel = 0;
        }


        static List<bool> showFoldoutList = new List<bool>();
        static int i;
        static BoneFollowerInspector lastInspector;
        static bool Show
        {
            get
            {
                if (showFoldoutList.Count > i)
                {
                    return showFoldoutList[i];
                }
                else
                {
                    showFoldoutList.Add(false);
                    return false;
                }
            }
            set
            {
                if (showFoldoutList.Count > i)
                {
                    showFoldoutList[i] = value;
                }
                else
                {
                    showFoldoutList.Add(value);
                }
            }
        }

        public static void ShowBoneHierarchy(this BoneFollowerInspector inspector)
        {
            if (lastInspector != inspector)
            {
                showFoldoutList = new List<bool>();
                i = 0;
            }

            var skeletonRenderer = inspector.serializedObject.FindProperty("skeletonRenderer");

            EditorGUILayout.Space();

            includeParentInSearch = EditorGUILayout.Toggle("Include Parents in Search: ", includeParentInSearch);

            EditorGUILayout.BeginHorizontal("Box");
            EditorGUILayout.LabelField("Search: ", EditorStyles.boldLabel);
            searchValue = EditorGUILayout.TextField(searchValue);
            EditorGUILayout.EndHorizontal();

            //maxParentsShown = EditorGUILayout.IntSlider("Max Parents Shown: ", maxParentsShown, 0, 20);

            i = 0;
            Show = EditorGUILayout.Foldout(Show, "Bone Hierarchy");
            if (!Show) { return; }
            EditorGUI.indentLevel = ++i;

            SkeletonRenderer skeletonRef = skeletonRenderer.objectReferenceValue as SkeletonRenderer;
            ExposedList<BoneData> bones = skeletonRef.skeletonDataAsset.GetSkeletonData(quiet: true).Bones;

            string currentParentName = null;
            foreach (BoneData bone in bones.Items)
            {
                if (bone.Parent != null && bone.Parent.Name != currentParentName)
                {
                    Show = EditorGUILayout.Foldout(Show, bone.Parent.Name);
                    EditorGUI.indentLevel = ++i;
                    currentParentName = bone.Parent.Name;
                }

                string jointName = bone.Name;
                string fullName = bone.Name;
                var iterator = bone;
                int parentsAdded = 0;
                while ((iterator = iterator.Parent) != null)
                {
                    fullName = string.Format("{0}/{1}", iterator.Name, fullName);
                    if (parentsAdded < maxParentsShown)
                    {
                        jointName = string.Format("{0}/{1}", iterator.Name, jointName);
                        parentsAdded++;
                    }
                }

                //if (includeParentInSearch && !fullName.Contains(searchValue)) { continue; }
                //if (!includeParentInSearch && !bone.Name.Contains(searchValue)) { continue; }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button('\u25B6'.ToString(), GUILayout.Width(50)))
                {
                    inspector.serializedObject.FindProperty("boneName").stringValue = bone.Name;
                    inspector.serializedObject.ApplyModifiedProperties();
                }
                EditorGUILayout.LabelField(bone.Name, EditorStyles.boldLabel);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = 0;
        }
    }
}