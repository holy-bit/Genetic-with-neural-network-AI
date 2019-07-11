using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeasonsController : MonoBehaviour
{
    public Terrain terrain;

    [SerializeField]

    TimeController timeController;

    int lastSeason=0;
    int lastMonth = 0;

    float percentage = 0;

    private void Start()
    {
        UpdateTerrainTexture(1, terrain.terrainData, 1, 0);
        UpdateTerrainTexture(1, terrain.terrainData, 2, 0);
        UpdateTerrainTexture(1, terrain.terrainData, 3, 0);
    }
    void Update()
    {
        //if (timeController.world_calendar.season != lastSeason)
        //{
        //    UpdateTerrainTexture(terrain.terrainData, lastSeason, timeController.world_calendar.season);
        //    lastSeason = timeController.world_calendar.season;

        //}



        if (timeController.world_calendar.month != lastMonth )
        {
            percentage += 0.25f;

            if (lastSeason != 3) { UpdateTerrainTexture(percentage, terrain.terrainData, lastSeason, lastSeason + 1); }
            else { UpdateTerrainTexture(1, terrain.terrainData, lastSeason, 0); }


            if (timeController.world_calendar.season != lastSeason)
            {
                percentage = 0;
                if (timeController.world_calendar.season == 0 && lastSeason == 3)
                {
                    lastMonth = 0;
                }
                lastSeason = timeController.world_calendar.season;
                
            }

            lastMonth++;
        }
        


        //if (timeController.world_calendar.number_of_day != lastDay)
        //{


            //    UpdateTerrainTexture(terrain.terrainData, lastSeason, timeController.world_calendar.season);

            //    if (timeController.world_calendar.season != lastSeason) lastSeason = timeController.world_calendar.season;


            //}

    }
    static void UpdateTerrainTexture(float percentage,TerrainData terrainData, int textureNumberFrom, int textureNumberTo)
    {
        //get current paint mask
        float[,,] alphas = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        // make sure every grid on the terrain is modified
        for (int i = 0; i < terrainData.alphamapWidth; i++)
        {
            for (int j = 0; j < terrainData.alphamapHeight; j++)
            {
                //for each point of mask do:
                //paint all from old texture to new texture (saving already painted in new texture)
                alphas[i, j, textureNumberTo] = percentage;//Mathf.Max(alphas[i, j, textureNumberFrom], alphas[i, j, textureNumberTo]);
                //set old texture mask to zero
                alphas[i, j, textureNumberFrom] = 1 - percentage;
            }
        }
        // apply the new alpha
        terrainData.SetAlphamaps(0, 0, alphas);
    }
}
