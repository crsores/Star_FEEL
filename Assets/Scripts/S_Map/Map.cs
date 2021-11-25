using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Map : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;
    public int xSize = 100;
    public int zSize = 100;


    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh; 
    }


    void Update()
    {
        UpdateMesh();
        createShape();
    }

    void createShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        // 사각형을 만들려면 정점의 갯수를 만들고자 하는 
        // 사각형의 개수보다 1개가 더 많아야 됨


        for (int i = 0, z = 0; z <= zSize; z++) // 정점찍기
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x * 0.3f, z * 0.3f) * 2f;
                // ㄴ맵 울퉁불퉁하게 만들기
                vertices[i] = new Vector3(x, y, z); // 평지로 만들고 싶으면 y를 0으로 두면 됨
                i++;
            }
        }

        // 삼각형은 꼭지점이 3개가 필요, 사각형 만들려면 6개 필요
        triangles = new int[xSize * zSize * 6];
        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0; 
                triangles[tris + 1] = vert + xSize + 1; 
                triangles[tris + 2] = vert + 1; 
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2; // 여기까지 한번 돌면 사각형 1개 완성

                vert++;
                tris += 6;
            }
            vert++;
        }

    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    //private void OnDrawGizmos() // 정점의 위치에 동그라미 그리기
    //{ // 기즈모는 퍼포먼스 많이 잡아 먹어서 굳이 동그라미 정점은 안그리는게 나음
    //    if (vertices == null)
    //    { // 정점이 없을 경우에는 기즈모가 안 생기게 리턴
    //        return;
    //    }
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], 0.1f);
    //    }
    //}

}
