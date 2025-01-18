using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARPlanePrefab : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;

    private List<GameObject> placedPrefabList = new List<GameObject>();
    [SerializeField] private int maxPrefabSpawnCount;
    private int placedPrefabCount;

    public GameObject PlaceblePrefab;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetPosition(out Vector2 touchPosition)
    {
        if(Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Input.GetTouch(0).position;
                return true;
            }
        }
        touchPosition = default;
        return false;
    }

    private void Update()
    {
        if(!TryGetPosition(out Vector2 touchPosition))
        {
            return;
        }
        if(raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = s_Hits[0].pose;
            if (placedPrefabCount < maxPrefabSpawnCount)
            {
                SpawnPrefab(hitPose);
            }
        }
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(PlaceblePrefab, hitPose.position, hitPose.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }

    public void SetPrefabType(GameObject prefabType)
    {
        PlaceblePrefab = prefabType;
    }
}
