namespace GameCore.Editor.VContainer
{
    using System.Collections.Generic;
    using ReflexDI;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GameScope))]
    public class GameScopeEditor : Editor
    {
        private SerializedProperty autoInjectGameObjectsProps;
        private GameScope          gameScope;

        private void OnEnable()
        {
            this.gameScope                  = (GameScope)this.target;
            this.autoInjectGameObjectsProps = this.serializedObject.FindProperty("injectableObjects");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!this.autoInjectGameObjectsProps.isArray) return;
            if (!GUILayout.Button("Find Inject Object")) return;
            this.autoInjectGameObjectsProps.ClearArray();

            var listAutoInjectObj = new List<GameObject>();
            var allObjs           = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
            foreach (var obj in allObjs)
            {
                var monoComponents = obj.GetComponents<MonoBehaviour>();

                if (monoComponents.Length == 0) continue;
                if (listAutoInjectObj.Contains(obj)) continue;
                foreach (var mono in monoComponents)
                {
                    if (mono is GameScope) continue;
                    var type = mono.GetType();
                    if
                    (
                        type.GetInjectableFieldInfos().Length    != 0 ||
                        type.GetInjectableMethodInfos().Length   != 0 ||
                        type.GetInjectablePropertyInfos().Length != 0
                    )

                    {
                        listAutoInjectObj.Add(obj);
                    }
                }
            }

            for (var i = 0; i < listAutoInjectObj.Count; i++)
            {
                this.autoInjectGameObjectsProps.InsertArrayElementAtIndex(i);
                var element = this.autoInjectGameObjectsProps.GetArrayElementAtIndex(i);
                element.objectReferenceValue = listAutoInjectObj[i];
            }

            this.autoInjectGameObjectsProps.serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this.gameScope);
        }
    }
}