using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class RandomMap : MonoBehaviour
{
    public int size = 10;
    public GameObject point;
    public GameObject grass;
    public GameObject stone;
    public GameObject sand;
	public GameObject tree;

    List<GameObject> spawnpoints = new List<GameObject>();

    void Awake ()
    {
        int max = size / 2;
        int min = 0 - max;

        for (int i = min; i < max; i++)
        {
            for (int j = min; j < max; j++)
            {
                Vector2 location = new Vector2(j, i);

                if (location.y == max - 1 || location.y == min || location.x == max - 1 || location.x == min)
                {
					Instantiate (stone, location, Quaternion.identity);
                }
                else
                {
                    int rand = Random.Range(0, 100);
                    if (rand <= 20)
                    {
						int treeorstone = Random.Range (0, 2);

						if (treeorstone == 0)
						{
							Instantiate(tree, location, Quaternion.identity);
						}
						else if (treeorstone == 1)
						{
							Instantiate(stone, location, Quaternion.identity);
						}

                        
                    }
                    else if (rand > 20 && rand < 30)
                    {
                        Instantiate(sand, location, Quaternion.identity);
                        GameObject spawnpoint = (GameObject)Instantiate(point, location, Quaternion.identity);

                        spawnpoints.Add(spawnpoint);
                    }
                    else
                    {
                        Instantiate(grass, location, Quaternion.identity);
                        GameObject spawnpoint = (GameObject)Instantiate(point, location, Quaternion.identity);

                        spawnpoints.Add(spawnpoint);
                    }
                }
            }
        }
	}

    public List<GameObject> returnSpawnpoints ()
    {
        return spawnpoints;
    }
}
