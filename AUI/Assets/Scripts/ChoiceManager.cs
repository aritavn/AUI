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
        /*floor = SpatialProcessing.Instance.floors.ElementAt(0).transform;
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
        rotation.z = 0f;*/
        Vector3 floorPosition0 = GameObject.Find("Plane").transform.position;
        System.Random rnd = new System.Random();
        Transform objects = new GameObject("Object").transform;
        objects.tag = "Objects";
        Transform other1;
        Transform other2;
        //da rimuovere poi
        floorPosition0.y = floorPosition0.y + 0.2f;
        Vector3 floorPosition1 = floorPosition0;
        Vector3 floorPosition2 = floorPosition0;
        int objectPosition = rnd.Next(0, 3);
        if(objectPosition==0)
        {
            floorPosition1.x = floorPosition1.x + 1f;
            floorPosition2.x = floorPosition2.x - 1f;
        }
        if (objectPosition == 1)
        {
            floorPosition0.x = floorPosition0.x + 1f;
            floorPosition2.x = floorPosition2.x - 1f;
        }
        if (objectPosition == 2)
        {
            floorPosition1.x = floorPosition1.x + 1f;
            floorPosition0.x = floorPosition0.x - 1f;
        }
        

        int secondChoice = 0;
        int thirdChoice = 0;
        int rightChild = 0;
        int childCount = objectPrefabs.transform.childCount;

        switch (state)
        {
            case 1://POSATE-->CIBO
                rightChild = 0;
                break;
            case 5://TAVOLO APPARECCHIATO-->CIBO
                rightChild = 0;
                break;
            case 12://BICCHIERE VUOTO-->ACQUA
                rightChild = 1;
                break;
            case 2://DENTI SPORCHI-->DENTIFRICIO
                rightChild = 2;
                break;
            case 3://DELFINO SPORCO-->SAPONETTA
                rightChild = 3;
                break;
            case 6://CINEMA-->? per ora sostituito con pizza
                rightChild = 4;
                break;
            case 4://LETTO-->CUSCINO
                rightChild = 5;
                break;
            case 7://CAMPO DA CALCIO-->PALLA
                rightChild = 6;
                break;
        }

        Transform food = objectPrefabs.transform.GetChild(rightChild);
        Instantiate(food, floorPosition0, food.rotation, objects);
        secondChoice = findRandom(rightChild, childCount);
        other1 = objectPrefabs.transform.GetChild(secondChoice);
        Instantiate(other1, floorPosition1, other1.rotation, objects);
        thirdChoice = findRandom2(rightChild, secondChoice, childCount);
        other2 = objectPrefabs.transform.GetChild(thirdChoice);
        Instantiate(other2, floorPosition2, other2.rotation, objects);

    }
    public override void GenerateObjectsInWorld()
    {
    }

    private int findRandom(int exclude1, int max)
    {
        int newNumber;
        System.Random rnd = new System.Random();
        do
        {
          newNumber = rnd.Next(0, max);
        } while (exclude1 == newNumber);

        return newNumber;
    }

    private int findRandom2(int exclude1, int exclude2, int max)
    {
        int newNumber;
        System.Random rnd = new System.Random();
        do
        {
            newNumber = rnd.Next(0, max);
        } while (exclude1 == newNumber || exclude2 == newNumber);

        return newNumber;
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
        Destroy(GameObject.Find("Object"));
        GameObject.Find("Box8").GetComponent<Roulette>().startRoulette();
    }

}

