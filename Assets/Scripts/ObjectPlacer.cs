using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject>  placedGameObjects = new(); // gride yerleþtirilen objeleri tutan liste 

    // objeyi gride yerleþtirme iþlemi yapan fonksiyon
    public int PlaceObject(GameObject prefab,Vector3 position)
    {

        GameObject newObject = Instantiate(prefab); // uygunsa obje oluþturulur
        newObject.transform.position = position;
        placedGameObjects.Add(newObject); // obje kaydedilir
        return placedGameObjects.Count - 1;
    }


    // gride yerleþtirilen objelerden ilgili obje yi indexine göre yok eden fonksiyon
    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex]==null )
            return;
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
