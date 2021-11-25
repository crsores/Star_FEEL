using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HeightMap : MonoBehaviour
{
    private MeshFilter mf = null;
    private MeshRenderer mr = null;

    public int sizeW = 10;  // Width
    public int sizeH = 10;  // Height

    [SerializeField] private Texture2D diffuseMap = null;
    [SerializeField] private Texture2D heightMap = null;


    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();

        mr.material = new Material(Shader.Find("Standard"));
        mr.material.mainTexture = diffuseMap;
    }

    private void Start()
    {
        sizeW = heightMap.width;
        sizeH = heightMap.height;

        Build();
    }

    private void Build()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Terrain Plane";

        int totalCnt = sizeW * sizeH;

        // Vertex
        Vector3[] vertices = new Vector3[totalCnt];
        for (int i = 0; i < totalCnt; ++i)
        {
            float x = (float)(i % sizeW);
            float z = (float)(i / sizeW);

            float y = heightMap.GetPixel(
                i % sizeW,
                i / sizeW
                ).grayscale;

            vertices[i] = new Vector3(x, y * 20f, z);
        }
        mesh.vertices = vertices;

        // Index
        int idxBufSize = (sizeW - 1) * (sizeH - 1) * 6;
        int[] indices = new int[idxBufSize];
        int idx = 0;
        for (int z = 0; z < sizeH - 1; ++z)
            for (int x = 0; x < sizeW - 1; ++x)
            {
                int v1 = (z * sizeW) + x;
                int v2 = v1 + 1;
                int v3 = v1 + sizeW;
                int v4 = v3 + 1;

                indices[idx++] = v3;
                indices[idx++] = v4;
                indices[idx++] = v1;
                indices[idx++] = v1;
                indices[idx++] = v4;
                indices[idx++] = v2;
            }
        mesh.triangles = indices;

        // Normal
        // TODO: 바뀐 정점 위치에 맞춰 노멀 값 다시 계산해야 함.
        Vector3[] normals = new Vector3[totalCnt];
        for (int i = 0; i < totalCnt; ++i)
        {
            normals[i] = new Vector3(0f, 1f, 0f);
        }
        mesh.normals = normals;

        // UV
        Vector2[] uvs = new Vector2[totalCnt];
        for (int i = 0; i < totalCnt; ++i)
        {
            float u = (float)(i % sizeW) / (sizeW - 1);
            float v = (float)(i / sizeW) / (sizeH - 1);
            uvs[i] = new Vector2(u, v);
        }
        mesh.uv = uvs;


        mf.mesh = mesh;
    }

    //private void OnDrawGizmos()
    //{
    //    if (!mf) return;

    //    List<Vector3> vertices = new List<Vector3>();
    //    mf.mesh.GetVertices(vertices);

    //    foreach (Vector3 ver in vertices)
    //    {
    //        Gizmos.DrawSphere(ver, 0.1f);
    //    }
    //}
}