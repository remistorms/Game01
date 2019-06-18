using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Manager : MonoBehaviour
{

    [SerializeField] private int mapSize = 4;
    private float tileSize = 10;
    [SerializeField] private GameObject HomeTile;
    [SerializeField] private GameObject[] RandomTiles;

    public List<GameObject> allMapTiles;
    public Dictionary<Vector3, GameObject> TileCoordinatesDictionary;

    public void ClearMap()
    {
        //Destroys previous map
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        TileCoordinatesDictionary = new Dictionary<Vector3, GameObject>();
        allMapTiles.Clear();
    }

    public void GenerateMap()
    {
        ClearMap();

        GameObject selectedTile;
        //Create the map tiles from -mapSize, to mapSize
        for (int x = -mapSize; x < mapSize +1; x++)
        {
            for (int y = -mapSize; y < mapSize +1; y++)
            {
                Vector3 position = new Vector3( x * tileSize, 0, y * tileSize );
                Vector3 mapCoordinate = new Vector3(x, 0, y);

                if (x == 0 && y == 0)
                {
                    // use home tile
                    selectedTile = HomeTile;
                }
                else
                {
                    //use random tile
                    int random = Random.Range(0, RandomTiles.Length);
                    selectedTile = RandomTiles[random];
                }

                GameObject instantiatedTile = Instantiate(selectedTile, this.transform) as GameObject;

                instantiatedTile.GetComponent<Tile>().isActiveTile = false;
                instantiatedTile.GetComponent<Tile>().coordinateOnMap = mapCoordinate;

                allMapTiles.Add(instantiatedTile);
                TileCoordinatesDictionary.Add(mapCoordinate, instantiatedTile);

                instantiatedTile.transform.localPosition = position;
            }
        }
    }

    public List<GameObject> GetNeigbouringTiles (Vector3 coordinate)
    {
        List<GameObject> neighbourgs = new List<GameObject>();
        //check if coordinate exists inside the dictionary
        if (TileCoordinatesDictionary.ContainsKey(coordinate))
        {
            //Get Up Neighbour
            Vector3 northCoordinate = new Vector3(coordinate.x, 0, coordinate.z + 1);
            GameObject northTile; 
            TileCoordinatesDictionary.TryGetValue( northCoordinate, out northTile );

            //Get South Neighbour
            Vector3 southCoordinate = new Vector3(coordinate.x, 0, coordinate.z - 1);
            GameObject southTile;
            TileCoordinatesDictionary.TryGetValue(southCoordinate, out southTile);

            //Get West Neighbour
            Vector3 westCoordinate = new Vector3(coordinate.x - 1, 0, coordinate.z);
            GameObject westTile;
            TileCoordinatesDictionary.TryGetValue(westCoordinate, out westTile);

            //Get East Neighbour
            Vector3 eastCoordinate = new Vector3(coordinate.x + 1, 0, coordinate.z);
            GameObject eastTile;
            TileCoordinatesDictionary.TryGetValue(eastCoordinate, out eastTile);

            //Get SouthEast Neighbour
            Vector3 seCoordinate = new Vector3(coordinate.x + 1, 0, coordinate.z - 1);
            GameObject seTile;
            TileCoordinatesDictionary.TryGetValue(seCoordinate, out seTile);

            //Get SouthWest Neighbour
            Vector3 swCoordinate = new Vector3(coordinate.x - 1, 0, coordinate.z - 1);
            GameObject swTile;
            TileCoordinatesDictionary.TryGetValue(swCoordinate, out swTile);

            //Get NorthEast Neighbour
            Vector3 neCoordinate = new Vector3(coordinate.x + 1, 0, coordinate.z + 1);
            GameObject neTile;
            TileCoordinatesDictionary.TryGetValue(neCoordinate, out neTile);

            //Get NorthWest Neighbour
            Vector3 nwCoordinate = new Vector3(coordinate.x - 1, 0, coordinate.z + 1);
            GameObject nwTile;
            TileCoordinatesDictionary.TryGetValue(nwCoordinate, out nwTile);

            neighbourgs.Add(northTile);
            neighbourgs.Add(neTile);
            neighbourgs.Add(eastTile);
            neighbourgs.Add(seTile);
            neighbourgs.Add(southTile);
            neighbourgs.Add(swTile);
            neighbourgs.Add(westTile);
            neighbourgs.Add(nwTile);
        }

        return neighbourgs;
    }

    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.G) )
        {
            GenerateMap();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Vector3 randomCoordinate = new Vector3(Random.Range(-mapSize, mapSize), 0, Random.Range(-mapSize, mapSize));
            List<GameObject> neightboughts = GetNeigbouringTiles( randomCoordinate );

            for (int i = 0; i < allMapTiles.Count; i++)
            {
                if (allMapTiles[i] != null)
                {
                    allMapTiles[i].SetActive(false);
                    allMapTiles[i].GetComponent<Tile>().isActiveTile = false;
                }
            }

            GameObject randomTile;

            TileCoordinatesDictionary.TryGetValue( randomCoordinate,out randomTile );

            randomTile.SetActive(true);
            randomTile.GetComponent<Tile>().isActiveTile = true;

            for (int i = 0; i < neightboughts.Count; i++)
            {
                if (neightboughts[i] != null)
                    neightboughts[i].SetActive(true);
            }
        }
    }

}
