namespace ReflexDI
{
    using System.Linq;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class ReflexDISetting : ScriptableObject
    {
        [SerializeField] private GameScope rootGameScopePrefab;

        private bool isInitialized;

        public static ReflexDISetting Instance { get; private set; }

        private void OnEnable()
        {
            if (Application.isPlaying) Instance = this;
        }

        private void OnDisable()
        {
            this.isInitialized = false;
            Instance           = null;
        }

        public void InitializeRootScopeIfNeed()
        {
            if (this.isInitialized) return;
            this.isInitialized = true;
            var rootScopeInstance = Instantiate(this.rootGameScopePrefab);
            DontDestroyOnLoad(rootScopeInstance);
        }

        public void BuildScopeIfNeed(GameScope gameScope)
        {
            if (gameScope.IsBuild) return;
            gameScope.RegisterInstance<IResolver>(ReflexDIExtensions.DIContainer);

            gameScope.Build(ReflexDIExtensions.DIContainer);
        }

#if UNITY_EDITOR
        [MenuItem("Assets/Create/ReflexDI/Setting")]
        public static void CreateAsset()
        {
            var path = EditorUtility.SaveFilePanelInProject("ReflexDISetting", "ReflexDISetting", "asset", string.Empty);

            if (string.IsNullOrEmpty(path))
                return;

            var instanceAsset = CreateInstance<ReflexDISetting>();
            AssetDatabase.CreateAsset(instanceAsset, path);
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            var asset           = preloadedAssets.FirstOrDefault(x => x is ReflexDISetting);
            preloadedAssets.Remove(asset);

            preloadedAssets.Add(instanceAsset);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeRootGameScope()
        {
            var preloadedAsset = PlayerSettings.GetPreloadedAssets().FirstOrDefault(asset => asset is ReflexDISetting);
            if (preloadedAsset is ReflexDISetting reflexDiSetting)
            {
                reflexDiSetting.OnDisable();
                reflexDiSetting.OnEnable();
            }
        }
#endif
    }
}