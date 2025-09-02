using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    Dictionary<Vector3Int, PlacementData> placeObjects = new();  // yerleþtirilen objelerin konum id index bilgilerinin yer aldýðý dict

    public void AddObjectAt(Vector3Int gridPosition,Vector2Int objectSize,int ID,int placedObcejtIndex)
    {
        List<Vector3Int> positionsToOccupy=CalculatePositions(gridPosition, objectSize); // kaplanýlacak grid konumlarý tespit ediliyor
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObcejtIndex); // datalar kayýt ediliyor

        // neden yazýldý anlamadým çok da bir iþlevi yok gibi
        // çünkü konumun uygunluðu daha önce PlacementSystem kodu içinde CheckPlacementValidity metodunda kontrol edildi
        foreach (var pos in positionsToOccupy) 
        {
            if (placeObjects.ContainsKey(pos))
            {
                throw new Exception($"dicrionary already contains this cell positions{pos}");
            }
            placeObjects[pos]= data;
        }
    }

    // daha önce obje yerleþtirilmiþ alanlarýn konumlarýna göre kapladýklarý gridleri tespit eden fonksiyon
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();// dolu grid hücrelerinin konumlarýný tutar 
        for (int x  = 0; x < objectSize.x; x++) 
        {
            for (int y = 0; y < objectSize.y; y++) 
            {
                returnVal.Add(gridPosition+new Vector3Int(x,0,y));
            } 
        }

        return returnVal;
    }

    // gönderilen konuma, gönderilen obje yerleþtirilebilir mi kontrol eden fonksiyon
    public bool CanPlaceObjectAt(Vector3Int gridPosition,Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (var pos in positionToOccupy) // yerleþtirilecek objenin daha önceki objelerle bir çakýþmasý var mý yok mu kontrol ediliyor
        {
            if (placeObjects.ContainsKey(pos)) // varsa bu konuma yerleþtirilemez olduðu için false dönüyor
            {
                return false; 
            }
        }
        return true; // uygunsa true
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placeObjects.ContainsKey(gridPosition)==false) // eðer bu pozisyonda silinebilecek obje yoksa -1 döner
        {
            Debug.Log(gridPosition.ToString());
            return -1;
        }
        return placeObjects[gridPosition].PlacedObjectIndex; // varsa o objenin kayýttaki indexini döner

    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var obj in placeObjects[gridPosition].occupiedPositions)  // kaplanýlan grid konumlarý list olarak obj ye atanýyor. 
        {
            placeObjects.Remove(obj); // eðer kayýtlý konumlar arasýnda obj listesi varsa bu konumlarý ve bu konumlara kayýtlý yerleþtirme bilgisini siliyor.
         }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions; // objenin konulduðu konum bilgisi
    public int ID {  get; private set; } // türünün tespiti için id bilgisi tutar
    public int PlacedObjectIndex {  get; private set; } // yerleþtirilen kaçýncý obje olduðunun bilgisini tutar

    public PlacementData(List<Vector3Int> _occupiedPositions,int id,int _placedObjectIndex)
    {
        this.occupiedPositions =_occupiedPositions;
        ID = id;
        PlacedObjectIndex = _placedObjectIndex;
    }
}
