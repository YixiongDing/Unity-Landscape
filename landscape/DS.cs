using UnityEngine;
using System.Collections;

public class DS : MonoBehaviour
{

    /**
	 * Based on script from:
	 * http://stackoverflow.com/questions/2755750/diamond-square-algorithm
	 * Modified by Eddy To for COMP30019 07-09-16
	 *
	 * Generates randomised DS based on Diamond-Square algorithm. Additionally
	 * applies the array as height map for a Unity terrain object.
	 */

    Terrain terrain;
    TerrainData tData;

    int size = 1025;

    void Start()
    {
        terrain = transform.GetComponent<Terrain>();
        tData = terrain.terrainData;

        generateDS();
    }

    void generateDS()
    {

        float[,] DS = new float[size, size];
        float val = 0;   // Square/Diamond values calculated
        float rand = 0;  // Random factor
        float b = 0.5f;  // Bumpiness factor
        int sideLen = 0; // Length of map sides
        int x = 0;
        int y = 0;
        int halfLen = 0; // Half of sideLen

        /** Initialise four corners */
        DS[0, 0] = 1;
        DS[size - 1, 0] = 1;
        DS[0, size - 1] = 1;
        DS[size - 1, size - 1] = 1;


        /** Loop to divide map */
        for (sideLen = size - 1; sideLen >= 2; sideLen /= 2)
        {
            halfLen = sideLen / 2;

            /** Calculations for square steps */
            for (x = 0; x < size - 1; x += sideLen)
            {
                for (y = 0; y < size - 1; y += sideLen)
                {
                    val = DS[x, y];
                    val += DS[x + sideLen, y];
                    val += DS[x, y + sideLen];
                    val += DS[x + sideLen, y + sideLen];

                    val /= 4.0f;

                    rand = (Random.value * 2.0f * b) - b;
                    val = Mathf.Clamp01(val + rand);

                    DS[x + halfLen, y + halfLen] = val;
                }
            }

            /** Calculations for diamond steps */
            for (x = 0; x < size - 1; x += halfLen)
            {
                for (y = (x + halfLen) % sideLen; y < size - 1;
                     y += sideLen)
                {
                    val = DS[(x - halfLen + size - 1) % (size - 1), y];
                    val += DS[(x + halfLen) % (size - 1), y];
                    val += DS[x, (y + halfLen) % (size - 1)];
                    val += DS[x, (y - halfLen + size - 1) % (size - 1)];

                    val /= 4.0f;

                    rand = (Random.value * 2.0f * b) - b;
                    val = Mathf.Clamp01(val + rand);

                    DS[x, y] = val;

                    if (x == 0)
                    {
                        DS[size - 1, y] = val;
                    }
                    if (y == 0)
                    {
                        DS[x, size - 1] = val;
                    }
                }
            }
            b /= 2.0f;
        }

        /** Apply diamond square array onto terrain as height map*/
        tData.SetHeights(0, 0, DS);
        Debug.Log("Terrain generation complete");
    }

    void Update()
    {

    }
}