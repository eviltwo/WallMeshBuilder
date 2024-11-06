using UnityEditor;
using UnityEngine;

namespace WallMeshBuilder
{
    [CreateAssetMenu(fileName = "WallMeshBuildSettings", menuName = "WallMeshBuilder/WallMeshBuildSettings")]
    public class MeshBuildSettings : ScriptableObject
    {
        [Header("Shape")]
        [SerializeField]
        public Vector3 Center = new Vector3(0, 0, 0);

        [SerializeField]
        public Vector3 Size = new Vector3(1, 1, 1);

        [Header("UV")]
        [SerializeField]
        public bool HasFront = true;

        [SerializeField]
        public Vector4 FrontUV = new Vector4(0, 0, 1, 1);

        [SerializeField]
        public bool HasBack = true;

        [SerializeField]
        public Vector4 BackUV = new Vector4(0, 0, 1, 1);

        [SerializeField]
        public bool HasRight = true;

        [SerializeField]
        public Vector4 RightUV = new Vector4(0, 0, 1, 1);

        [SerializeField]
        public bool HasLeft = true;

        [SerializeField]
        public Vector4 LeftUV = new Vector4(0, 0, 1, 1);

        [SerializeField]
        public bool HasTop = true;

        [SerializeField]
        public Vector4 TopUV = new Vector4(0, 0, 1, 1);

        [SerializeField]
        public bool HasBottom = true;

        [SerializeField]
        public Vector4 BottomUV = new Vector4(0, 0, 1, 1);

        [Header("Output")]
        [SerializeField]
        public Mesh OverrideMesh = null;

        [SerializeField]
        public string FileName = "Wall";
    }

    [CustomEditor(typeof(MeshBuildSettings))]
    public class MeshBuildSettingsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Build"))
            {
                var settings = (MeshBuildSettings)target;
                var builder = new MeshBuilder(settings);
                builder.Build();
            }
        }
    }
}
