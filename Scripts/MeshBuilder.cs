using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WallMeshBuilder
{
    public class MeshBuilder
    {
        private readonly MeshBuildSettings _settings;
        private readonly List<Vector3> _vertices = new List<Vector3>();
        private readonly List<Vector2> _uvs = new List<Vector2>();
        private readonly List<int> _triangles = new List<int>();

        public MeshBuilder(MeshBuildSettings settings)
        {
            _settings = settings;
        }

        public void Build()
        {
            Mesh mesh;
            if (_settings.OverrideMesh != null)
            {
                mesh = _settings.OverrideMesh;
            }
            else
            {
                mesh = new Mesh();
            }

            mesh.Clear();
            _vertices.Clear();
            _uvs.Clear();
            _triangles.Clear();

            if (_settings.HasFront)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.FrontUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            if (_settings.HasBack)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.BackUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            if (_settings.HasRight)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.RightUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            if (_settings.HasLeft)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.LeftUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            if (_settings.HasTop)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, _settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.TopUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            if (_settings.HasBottom)
            {
                AddMeshData(
                    _settings,
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, -_settings.Size.z / 2),
                    _settings.Center + new Vector3(_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.Center + new Vector3(-_settings.Size.x / 2, -_settings.Size.y / 2, _settings.Size.z / 2),
                    _settings.BottomUV,
                    _vertices,
                    _uvs,
                    _triangles);
            }

            mesh.SetVertices(_vertices);
            mesh.SetUVs(0, _uvs);
            mesh.SetTriangles(_triangles, 0);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            if (_settings.OverrideMesh == null)
            {
                mesh.name = _settings.FileName;
                var settingsPath = AssetDatabase.GetAssetPath(_settings);
                var directory = Path.GetDirectoryName(settingsPath);
                AssetDatabase.CreateAsset(mesh, Path.Combine(directory, mesh.name + ".asset"));
                _settings.OverrideMesh = mesh;
                EditorUtility.SetDirty(_settings);
            }
            else
            {
                EditorUtility.SetDirty(mesh);
            }
            AssetDatabase.SaveAssets();
        }

        public static void AddMeshData(
            MeshBuildSettings settings,
            Vector3 bottomLeft,
            Vector3 bottomRight,
            Vector3 topLeft,
            Vector3 topRight,
            Vector4 uv,
            List<Vector3> vertices,
            List<Vector2> uvs,
            List<int> triangles)
        {
            triangles.Add(vertices.Count);
            triangles.Add(vertices.Count + 1);
            triangles.Add(vertices.Count + 2);
            triangles.Add(vertices.Count);
            triangles.Add(vertices.Count + 2);
            triangles.Add(vertices.Count + 3);
            vertices.Add(bottomLeft);
            vertices.Add(topLeft);
            vertices.Add(topRight);
            vertices.Add(bottomRight);
            uvs.Add(new Vector2(uv.x, uv.y));
            uvs.Add(new Vector2(uv.x, uv.w));
            uvs.Add(new Vector2(uv.z, uv.w));
            uvs.Add(new Vector2(uv.z, uv.y));
        }
    }
}
