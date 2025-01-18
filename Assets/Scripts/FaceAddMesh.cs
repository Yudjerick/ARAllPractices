using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class FaceAddMesh : MonoBehaviour
{
    [SerializeField] ARFace face = null;
    [SerializeField] int[] mousePointsIndices = new[] { 164 };
    [SerializeField] float pointerScale = 0.01f;
    [SerializeField] GameObject optionalPointerPrefab = null;
    Dictionary<int, Transform> pointers  = new Dictionary<int, Transform>();

    private void Awake()
    {
        face.updated += delegate
        {
            for (var i = 0; i < mousePointsIndices.Length; i++)
            {
                var vertexIndex = mousePointsIndices[i];
                var pointer = GetPointer(i);
                pointer.position = face.transform.TransformPoint(face.vertices[vertexIndex]);
                pointer.rotation = face.transform.rotation;
            }
        };
    }

    private Transform GetPointer(int id)
    {
        if (pointers.TryGetValue(id, out var existing))
        {
            return existing;
        }
        else
        {
            var newPointer = CreateNewPointer();
            pointers[id] = newPointer;
            return newPointer;
        }

    }

    private Transform CreateNewPointer()
    {
        var result = InstantiatePointer();
        return result;
    }

    private Transform InstantiatePointer()
    {
        if(optionalPointerPrefab != null)
        {
            return Instantiate(optionalPointerPrefab).transform;
        }
        else
        {
            var result = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            result.localScale = Vector3.one * pointerScale;
            return result;
        }
    }

    private void OnDestroy()
    {
        for (var i = 0; i < mousePointsIndices.Length; i++)
        {
            Destroy(GetPointer(i).gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
