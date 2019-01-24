using HoloToolkit.Unity.SpatialMapping.Tests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HoloToolkit.Unity.SpatialMapping;
using System;
using HoloToolkit.Unity;

public class ChoiceManager : TaskManager
{
    public GameObject objectPrefabs;
    private Transform floor;
    public Transform table;
    private SurfacePlane tablePlane;
    private SurfacePlane plane;
    private Vector3 floorPosition;
    private Quaternion rotation;
    Bounds tableColliderBounds;
    private int firstTime = 0;
    System.Random rnd = new System.Random();
    public int currentState;

    public void GenerateObjectsInWorld(int state)
    {
        currentState = state;

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

        Transform objects = new GameObject("Object").transform;
        objects.tag = "Objects";
        Transform other1;
        Transform other2;
        if (firstTime == 0)
        {
            firstTime = 1;
            table = TableSelect(SpatialProcessing.Instance.tables);
            tableColliderBounds = table.GetColliderBounds();
        }

        Vector3 center = tableColliderBounds.center;

        center.y = center.y + 0.2f;

        int quadrante1 = rnd.Next(0, 4);
        int quadrante2 = findRandom(quadrante1, 4);
        int quadrante3 = findRandom2(quadrante1, quadrante2, 4);

        Vector3 item1 = center;
        Vector3 item2 = center;
        Vector3 item3 = center;

        item1 = modifyPosition(item1, quadrante1);
        item2 = modifyPosition(item2, quadrante2);
        item3 = modifyPosition(item3, quadrante3);

        int objectPosition = rnd.Next(0, 3);
        /*if(objectPosition==0)
        {
            tableposition1.x = floorPosition1.x + 1f;
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
        
    */
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
        Instantiate(food, item1, food.rotation, objects);
        secondChoice = findRandom(rightChild, childCount);
        other1 = objectPrefabs.transform.GetChild(secondChoice);
        Instantiate(other1, item2, other1.rotation, objects);
        thirdChoice = findRandom2(rightChild, secondChoice, childCount);
        other2 = objectPrefabs.transform.GetChild(thirdChoice);
        Instantiate(other2, item3, other2.rotation, objects);

    }
    public override void GenerateObjectsInWorld()
    {
    }

    public int getGameState()
    {
        return currentState;
    }

    private Transform TableSelect(List<GameObject> tables)
    {
        Vector3 gazePosition = new Vector3(0f, 0f, 0f);
        RaycastHit hitInfo;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitInfo, 20f, Physics.DefaultRaycastLayers))
        {
            gazePosition = hitInfo.point;
        }

        float minDistance = 1000f;
        Transform nearestTable = null;
        foreach (GameObject table in tables)
        {
            Vector3 tableCenter = table.transform.GetColliderBounds().center;
            if (Vector3.Distance(tableCenter, gazePosition) <= minDistance)
            {
                minDistance = Vector3.Distance(tableCenter, gazePosition);
                nearestTable = table.transform;
            }
        }

        foreach (GameObject table in tables)
        {
            if (table.GetInstanceID() != nearestTable.gameObject.GetInstanceID())
            {
                Destroy(table.gameObject);
            }
        }

        return nearestTable;
    }

    private Vector3 modifyPosition(Vector3 item, int quadrante)
    {
        float random1 = rnd.Next(8, 25);
        float random2 = rnd.Next(8, 25);

        if (quadrante == 0)
        {
            item.x = item.x + random1 / 100;
            item.z = item.z + random2 / 100;
        }

        if (quadrante == 1)
        {
            item.x = item.x + random1 / 100;
            item.z = item.z - random2 / 100;
        }

        if (quadrante == 2)
        {
            item.x = item.x - random1 / 100;
            item.z = item.z + random2 / 100;
        }

        if (quadrante == 3)
        {
            item.x = item.x - random1 / 100;
            item.z = item.z - random2 / 100;
        }

        return item;
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

