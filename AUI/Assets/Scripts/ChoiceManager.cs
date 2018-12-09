using HoloToolkit.Unity.SpatialMapping.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HoloToolkit.Unity.SpatialMapping;
using System;

public class ChoiceManager : TaskManager
{
    public GameObject objectPrefabs;
    private Transform floor;
    private Transform table;
    private SurfacePlane tablePlane;
    private SurfacePlane plane;
    private Vector3 floorPosition;
    private Quaternion rotation;

    public void GenerateObjectsInWorld(int state)
    {
        System.Random rnd = new System.Random();
        Transform objects = new GameObject("Object").transform;
        objects.tag = "Objects";
        Transform other1;
        Transform other2;
        switch (state)
        {
            case 1://POSATE-->CIBO
                Transform food = objectPrefabs.transform.GetChild(0);
                Instantiate(food, table.position, food.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(rnd.Next(1, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(rnd.Next(1, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 5://TAVOLO APPARECCHIATO-->CIBO
                Transform food1 = objectPrefabs.transform.GetChild(0);
                Instantiate(food1, table.position, food1.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(rnd.Next(1, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(rnd.Next(1, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 12://BICCHIERE VUOTO-->ACQUA
                Transform water = objectPrefabs.transform.GetChild(1);
                Instantiate(water, table.position, water.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(1, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(1, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 2://DENTI SPORCHI-->DENTIFRICIO
                Transform teeth = objectPrefabs.transform.GetChild(2);
                Instantiate(teeth, table.position, teeth.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(2, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(2, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 3://DELFINO SPORCO-->SAPONETTA
                Transform dolphin = objectPrefabs.transform.GetChild(3);
                Instantiate(dolphin, table.position, dolphin.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(3, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(3, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 6://CINEMA-->? per ora sostituito con pizza
                Transform movie = objectPrefabs.transform.GetChild(4);
                Instantiate(movie, table.position, movie.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(4, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(4, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 4://LETTO-->CUSCINO
                Transform pillow = objectPrefabs.transform.GetChild(5);
                Instantiate(pillow, table.position, pillow.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(5, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(5, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
            case 7://CAMPO DA CALCIO-->PALLA
                Transform ball = objectPrefabs.transform.GetChild(6);
                Instantiate(ball, table.position, ball.rotation, objects);
                other1 = objectPrefabs.transform.GetChild(Randomize(6, objectPrefabs.transform.childCount));
                Instantiate(other1, table.position, other1.rotation, objects);
                other2 = objectPrefabs.transform.GetChild(Randomize(6, objectPrefabs.transform.childCount));
                Instantiate(other2, table.position, other2.rotation, objects);
                break;
        }
    }
    public override void GenerateObjectsInWorld()
    {
    }
    public int Randomize(int ourChild, int maxValue)
    {
        int value;
        System.Random rnd = new System.Random();
        int chooseGroup = rnd.Next(0, 1);
        if (chooseGroup == 0)
        {
            value = rnd.Next(0, ourChild - 1);
        }
        else
        {
            value = rnd.Next(ourChild + 1, maxValue);
        }
        return value;
    }
    /// <summary>
    /// Adjusts the initial position of the object if it is being occluded by the spatial map.
    /// </summary>
    /// <param name="position">Position of object to adjust.</param>
    /// <param name="surfaceNormal">Normal of surface that the object is positioned against.</param>
    /// <returns></returns>
    private Vector3 AdjustPositionWithSpatialMap(Vector3 position, Vector3 surfaceNormal)
    {
        Vector3 newPosition = position;
        RaycastHit hitInfo;
        float distance = 0.5f;

        // Check to see if there is a SpatialMapping mesh occluding the object at its current position.
        if (Physics.Raycast(position, surfaceNormal, out hitInfo, distance, SpatialMappingManager.Instance.LayerMask))
        {
            // If the object is occluded, reset its position.
            newPosition = hitInfo.point;
        }

        return newPosition;
    }
    public override void Start()
    {
        floor = SpatialProcessing.Instance.floors.ElementAt(0).transform;
        table = SpatialProcessing.Instance.tables.ElementAt(0).transform;
        tablePlane = table.GetComponent<SurfacePlane>();
        plane = floor.GetComponent<SurfacePlane>();
        floorPosition = floor.transform.position + (plane.PlaneThickness * plane.SurfaceNormal);
        floorPosition = AdjustPositionWithSpatialMap(floorPosition, plane.SurfaceNormal);
        Vector3 gazePosition = new Vector3(0f, 0f, 0f);
        Vector3 objectPosition = gazePosition;
        Vector3 relativePos = Camera.main.transform.position - gazePosition;
        rotation = Quaternion.LookRotation(relativePos);
        rotation.x = 0f;
        rotation.z = 0f;
    }

    public override void Update()
    {
    }
    public void DestroyObjects()
    {
        Destroy(GameObject.Find("Object").transform.GetChild(1).gameObject);
        Destroy(GameObject.Find("Object").transform.GetChild(2).gameObject);
        StartCoroutine(Wait());
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(10f);
        Destroy(GameObject.Find("Object").transform.GetChild(2));
        //Chiama la roulette per passare allo stato successivo
    }

}

