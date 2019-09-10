using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquare : MonoBehaviour
{
    public Shader shader;
    // public Texture texture;
    public int mDivisions = 128;
    public float mSize = 1000;
    public float mHeight = 300;
    public PointLight pointLight;

    Vector3[] mVerts;
    Color[] mColors;
    int mVertsCount;

    void Start()
    {
        MeshFilter terrainMesh = this.gameObject.AddComponent<MeshFilter>();
        terrainMesh.mesh = this.CreateTerrain();
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        MeshCollider terrainCollider = this.gameObject.AddComponent<MeshCollider>();
        //terrainCollider.convex = true;
        //terrainCollider.isTrigger = true;

        renderer.material.shader = shader;
        // renderer.material.mainTexture = texture;
    }
    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();

        // Pass updated light positions to shader
        renderer.material.SetColor("_PointLightColor", this.pointLight.color);
        renderer.material.SetVector("_PointLightPosition", this.pointLight.GetWorldPosition());
    }


    // Update is called once per frame
    Mesh CreateTerrain()
    {
        mVertsCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertsCount];
        mColors = new Color[mVertsCount];
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

        //mVerts[0].y = -250;
        //mVerts[mDivisions].y = -250;
        //mVerts[mVerts.Length - 1].y = -250;
        //mVerts[mVerts.Length - 1 - mDivisions].y = 200;

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

        float maxHeight = 0.0f;
        float minHeight = 0.0f;

        for (int i = 0; i < mVerts.Length; ++i)
        {
            if (mVerts[i].y > maxHeight)
            {
                maxHeight = mVerts[i].y;
            }
            else if (mVerts[i].y < minHeight)
            {
                minHeight = mVerts[i].y;
            }
        }

        float firstLayer = 0.4f * (maxHeight - minHeight) + minHeight;
        float secondLayer = 0.7f * (maxHeight - minHeight) + minHeight;
        
        GameObject water = GameObject.Find("Water");
        water.GetComponent<MeshRenderer>().enabled = true;
        //if ( minHeight + firstLayer > 0.0f )
        //{
        //    water.transform.Translate(new Vector3(0.0f, minHeight + firstLayer/2, 0.0f));
        //}

        water.transform.Translate(new Vector3(0.0f, minHeight + 4 * (firstLayer-minHeight) / 5, 0.0f));

        float midLayer = 30.0f;
        float randomColor = 0.0f;

        for (int i = 0; i < mVerts.Length; ++i)
        {
            if (mVerts[i].y > secondLayer + midLayer)
            {
                mColors[i] = Color.white;
            }
            else if (secondLayer < mVerts[i].y && mVerts[i].y < (secondLayer + midLayer))
            {
                randomColor = Random.Range(0f, 1f);
                mColors[i] = new Color(randomColor, 1, randomColor, 1);
            }
            else if (mVerts[i].y < firstLayer)
            {
                mColors[i] = new Color(1f, 0.72f, 0.08f, 1);
            }
            else if (firstLayer <= mVerts[i].y && mVerts[i].y < firstLayer + midLayer)
            {
                randomColor = Random.Range(0f, 1f);
                mColors[i] = new Color(randomColor, 0.72f, 0.08f, 1);
            }
            else if (firstLayer + midLayer <= mVerts[i].y && mVerts[i].y <= secondLayer)
            {
                mColors[i] = new Color(0f, 0.72f, 0.08f, 1);
            }

        }

        //for (int i = 0; i < mVerts.Length; ++i)
        //{
        //    if (mVerts[i].y > 70)
        //    {
        //        uvs[i] = new Vector2(0.0f, 0.666f);
        //        uvs[++i] = new Vector2(0.0f, 1.0f);
        //        uvs[++i] = new Vector2(0.333f, 1.0f);
        //    }
        //    if (mVerts[i].y < 10)
        //    {
        //        uvs[i] = new Vector2(0.333f, 0.333f);
        //        uvs[++i] = new Vector2(0.666f, 0.0f);
        //        uvs[++i] = new Vector2(0.333f, 0.0f);
        //    }
        //    if (10 <= mVerts[i].y && mVerts[i].y <= 70)
        //    {
        //        uvs[i] = new Vector2(0.666f, 0.666f);
        //        uvs[++i] = new Vector2(0.333f, 0.666f);
        //        uvs[++i] = new Vector2(0.333f, 1.0f);
        //    }

        //}

        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.colors = mColors;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;

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