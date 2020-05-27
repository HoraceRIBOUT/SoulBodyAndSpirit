using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class NuageLine : MonoBehaviour
{
    public float speed = 1f;

    public Vector2 posMinMax = new Vector2(-3, 3);
    public int orderInLayer = 0;

    [System.Serializable()]
    public struct CloudInfo
    {
        public Sprite sprite;
        public float sizeX;
        public float offsetY;
        public Vector2 scaleX;
    }

    public List<CloudInfo> listOfPossibleCloud = new List<CloudInfo>();
    public GameObject emptyPrefab;

    private List<GameObject> cloudSpawned = new List<GameObject>();
    private float currentXPosOfNextCloud;

    // Update is called once per frame
    void Update()
    {
        //will work on a movement and then a despawn when too far on one side and respawn the other side
    }


    [MyBox.ButtonMethod()]
    public void FillOffCloud()
    {
        DestroyAllCloud();
        cloudSpawned.Clear();

        currentXPosOfNextCloud = posMinMax.x; //start from left
        int lastRandom = -1;
        for (int security = 0; security < 100; security ++)
        {
            //The security number it's just t avoid creating an infinite number of cloud : 
            //Get a random cloud but not the previous one
            int random = Random.Range(0, listOfPossibleCloud.Count);
            if (random == lastRandom)
                random = (random == 0 ? listOfPossibleCloud.Count - 1 : random - 1);
            lastRandom = random;
            //


            GameObject newCloud = Instantiate(emptyPrefab, this.transform);
            SpriteRenderer sR = newCloud.AddComponent<SpriteRenderer>();
            sR.sprite = listOfPossibleCloud[random].sprite;
            sR.flipX = Random.Range(0.0f, 2.0f) > 1;
            sR.sortingOrder = orderInLayer;

            float randomSizeX = Random.Range(listOfPossibleCloud[random].scaleX.x, listOfPossibleCloud[random].scaleX.y);

            newCloud.transform.localPosition = Vector3.right * (currentXPosOfNextCloud + listOfPossibleCloud[random].sizeX * 0.5f * randomSizeX) + Vector3.up * listOfPossibleCloud[random].offsetY;
            newCloud.transform.localScale = new Vector3(randomSizeX, 1,1);

            currentXPosOfNextCloud += listOfPossibleCloud[random].sizeX * randomSizeX;


            newCloud.name = "Cloud " + security;
            cloudSpawned.Add(newCloud);

            if (currentXPosOfNextCloud < posMinMax.y)
                continue;
            //enough cloud.
            break;
        }

    }

    [MyBox.ButtonMethod()]
    public void DestroyAllCloud()
    {
        foreach (GameObject cloud in cloudSpawned)
        {
            DestroyImmediate(cloud);
        }
        cloudSpawned.Clear();
    }
}
