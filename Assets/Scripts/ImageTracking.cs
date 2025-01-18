using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracking : MonoBehaviour
{

    [SerializeField] private List<GameObject> placeblePrefabs;
    private ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        foreach (var prefab in placeblePrefabs)
        {
            var instance = Instantiate(prefab);
            instance.name = prefab.name;
            spawnedPrefabs.Add(instance.name, instance);
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    private void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        
        foreach(var image in eventArgs.added)
        {
            
            UpdateImage(image);
        }
        foreach(var image in eventArgs.updated)
        {
            
            UpdateImage(image);
        }
        foreach(var image in eventArgs.removed)
        {
            spawnedPrefabs[image.referenceImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if(spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(true);
                spawnedPrefabs[trackedImage.referenceImage.name].transform.position = trackedImage.transform.position;
                spawnedPrefabs[trackedImage.referenceImage.name].transform.rotation = trackedImage.transform.rotation;

            }
        }
        else
        {
            if (spawnedPrefabs.ContainsKey(trackedImage.referenceImage.name))
            {
                spawnedPrefabs[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }
}
