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
        public bool active;
        public Sprite sprite;
        public float sizeX;
        public float offsetY;
        public Vector2 scaleX;
    }

    public List<CloudInfo> listOfPossibleCloud = new List<CloudInfo>();
    public GameObject emptyPrefab;

    public List<infoNuage> cloudSpawned = new List<infoNuage>();
    private float currentXPosOfNextCloud;

    private void Start()
    {
        if (!Application.isPlaying)
            return;
        FillOffCloud();
    }

    // Update is called once per frame
    void Update()
    {
        //will work on a movement and then a despawn when too far on one side and respawn the other side
        if (!Application.isPlaying)
            return;

        float moveX = speed * Time.deltaTime;
        currentXPosOfNextCloud -= moveX;
        Vector3 move = Vector3.left * moveX;

        foreach (infoNuage info in cloudSpawned)
        {
            info.transform.Translate(move * this.transform.localScale.x);
            float halfSize = (info.size / 2f);
            if (info.transform.localPosition.x < posMinMax.x - halfSize)
            {
                currentXPosOfNextCloud += halfSize;
                info.transform.localPosition = Vector3.right * currentXPosOfNextCloud + Vector3.up * info.transform.localPosition.y;
                currentXPosOfNextCloud += halfSize;
            }
        }

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

            //Because I don't want to destroy what I have make
            if (!listOfPossibleCloud[random].active)
                continue;


            GameObject newCloud = Instantiate(emptyPrefab, this.transform);
            SpriteRenderer sR = newCloud.AddComponent<SpriteRenderer>();
            sR.sprite = listOfPossibleCloud[random].sprite;
            sR.flipX = Random.Range(0.0f, 2.0f) > 1;
            sR.sortingOrder = orderInLayer;

            float randomSizeX = Random.Range(listOfPossibleCloud[random].scaleX.x, listOfPossibleCloud[random].scaleX.y);

            newCloud.transform.localPosition = Vector3.right * (currentXPosOfNextCloud + listOfPossibleCloud[random].sizeX * 0.5f * randomSizeX) + Vector3.up * listOfPossibleCloud[random].offsetY;
            newCloud.transform.localScale = new Vector3(randomSizeX, 1,1);
            newCloud.name = "Cloud " + security;

            currentXPosOfNextCloud += listOfPossibleCloud[random].sizeX * randomSizeX;

            infoNuage infoNuage = newCloud.AddComponent<infoNuage>();
            infoNuage.size = listOfPossibleCloud[random].sizeX * randomSizeX;

            cloudSpawned.Add(infoNuage);

            if (currentXPosOfNextCloud < posMinMax.y)
                continue;
            //enough cloud.
            break;
        }

    }

    [MyBox.ButtonMethod()]
    public void DestroyAllCloud()
    {
        foreach (infoNuage cloud in cloudSpawned)
        {
            DestroyImmediate(cloud.gameObject);
        }
        cloudSpawned.Clear();
    }
}
