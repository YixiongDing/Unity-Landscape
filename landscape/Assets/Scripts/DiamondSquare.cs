using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour
{
    public Shader shader;
    public Texture texture;
    public int mDivisions;
    public float mSize;
    public float mHeight;

    Vector3[] mVerts;
    int mVertsCount;

    void Start()
    {
        CreateTerrain();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;
        renderer.material.mainTexture = texture;
    }

    // Update is called once per frame
    void CreateTerrain()
    {
        mVertsCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertsCount];
        Vector2[] uvs = new Vector2[mVertsCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;

        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize, 0.0f, halfSize - i * divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topleft = i * (mDivisions + 1) + j;
                    int botleft = (i + 1) * (mDivisions + 1) + j;

                    tris[triOffset] = topleft;
                    tris[triOffset + 1] = topleft + 1;
                    tris[triOffset + 2] = botleft + 1;

                    tris[triOffset + 3] = topleft;
                    tris[triOffset + 4] = botleft + 1;
                    tris[triOffset + 5] = botleft;

                    triOffset += 6;

                }
            }

        }


        mVerts[0].y = Random.Range(-mHeight, mHeight);
        mVerts[mDivisions].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-mHeight, mHeight);

        int interations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;
        for (int i = 0; i < interations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    DiamondSquareGenerator(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }
            numSquares *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;

        }

        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

       
    }

    void DiamondSquareGenerator(int row, int col, int size, float offset)
    {
        int halfsize = (int)(size * 0.5f);
        int topleft = row * (mDivisions + 1) + col;
        int botleft = (row + size) * (mDivisions + 1) + col;

        int mid = (int)(row + halfsize) * (mDivisions + 1) + (int)(col + halfsize);
        mVerts[mid].y = (mVerts[topleft].y + mVerts[topleft + size].y + mVerts[botleft].y + mVerts[botleft + size].y) * 0.25f + Random.Range(-offset, offset);

        mVerts[topleft + halfsize].y = (mVerts[topleft].y + mVerts[topleft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfsize].y = (mVerts[topleft].y + mVerts[botleft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);

        mVerts[mid + halfsize].y = (mVerts[topleft + size].y + mVerts[botleft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);

        mVerts[botleft + halfsize].y = (mVerts[botleft].y + mVerts[botleft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }
}