using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> placedGameObjects = new();


    public int PlaceObject(GameObject prefab,Vector3 position)
    {

        GameObject newObject = Instantiate(prefab); // uygunsa obje oluþturulur
        newObject.transform.position = position;
        placedGameObjects.Add(newObject); // obje kaydedilir
        return placedGameObjects.Count - 1;
    }
}
