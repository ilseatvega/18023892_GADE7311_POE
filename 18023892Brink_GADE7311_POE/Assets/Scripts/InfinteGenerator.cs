using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//keeping track of tile
class Tile
{
    public GameObject tile;
    public float creationTime;

    public Tile(GameObject t, float ct)
    {
        tile = t;
        creationTime = ct;
    }
}

public class InfinteGenerator : MonoBehaviour
{
    public GameObject plane;
    public GameObject cam;

    //tile size
    int planeSize = 10;
    //how many tiles spawn around cam
    int halfTilesx = 5;
    int halfTilesz = 5;
    int seed;

    Vector3 startPos;

    Hashtable tiles = new Hashtable();
   
    // Start is called before the first frame update
    void Start()
    {
        seed = Random.Range(0,999999);
        plane.GetComponent<ProcGen>().seed = seed;

        this.gameObject.transform.position = Vector3.zero;
        startPos = Vector3.zero;

        float updateTime = Time.realtimeSinceStartup;

        //loops in x and z 
        for (int x = -halfTilesx; x < halfTilesx; x++)
        {
            for (int z = -halfTilesz; z < halfTilesz; z++)
            {
                Vector3 pos = new Vector3((x * planeSize + startPos.x), 0, (z * planeSize + startPos.z));
                GameObject t = (GameObject)Instantiate(plane, pos, Quaternion.identity);
                
                string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                t.name = tileName;
                Tile tile = new Tile(t, updateTime);
                //add tile to hashtable
                tiles.Add(tileName, tile);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //determine how far cam moved
        int xMove = (int)(cam.transform.position.x - startPos.x);
        int zMove = (int)(cam.transform.position.z - startPos.z);

        //if moved more than one plane
        if (Mathf.Abs(xMove) >= planeSize || Mathf.Abs(zMove) >= planeSize)
        {
            //so that we know which tiles are outisde range of player and needs to be removed
            float updateTime = Time.realtimeSinceStartup;
            //mathf.floor rounds down
            int camX = (int)(Mathf.Floor(cam.transform.position.x / planeSize) * planeSize);
            int camZ = (int)(Mathf.Floor(cam.transform.position.z / planeSize) * planeSize);

            for (int i = -halfTilesx; i < halfTilesx; i++)
            {
                for (int j = -halfTilesz; j < halfTilesz; j++)
                {
                    //generating tiles around cam
                    Vector3 pos = new Vector3((i * planeSize + camX), 0, (j * planeSize + camZ));

                    string tileName = "Tile_" + ((int)(pos.x)).ToString() + "_" + ((int)(pos.z)).ToString();
                    //if tiles havent been made
                    if (!tiles.ContainsKey(tileName))
                    {
                        GameObject t = (GameObject)Instantiate(plane, pos, Quaternion.identity);
                        t.name = tileName;
                        Tile tile = new Tile(t, updateTime);
                        tiles.Add(tileName, tile);
                    }
                    else
                    {
                        (tiles[tileName] as Tile).creationTime = updateTime;
                    }
                }
            }
            //destroy all tiles that weren't just created
            Hashtable newTiles = new Hashtable();
            foreach (Tile aTile in tiles.Values)
            {
                if (aTile.creationTime != updateTime)
                {
                    Destroy(aTile.tile);
                }
                else
                {
                    newTiles.Add(aTile.tile.name, aTile);
                }
            }
            tiles = newTiles;
            startPos = cam.transform.position;
        }
    }
}
