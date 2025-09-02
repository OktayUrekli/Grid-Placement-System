using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData 
{
    Dictionary<Vector3Int, PlacementData> placeObjects = new();  // yerle�tirilen objelerin konum id index bilgilerinin yer ald��� dict

    public void AddObjectAt(Vector3Int gridPosition,Vector2Int objectSize,int ID,int placedObcejtIndex)
    {
        List<Vector3Int> positionsToOccupy=CalculatePositions(gridPosition, objectSize); // kaplan�lacak grid konumlar� tespit ediliyor
        PlacementData data = new PlacementData(positionsToOccupy, ID, placedObcejtIndex); // datalar kay�t ediliyor

        // neden yaz�ld� anlamad�m �ok da bir i�levi yok gibi
        // ��nk� konumun uygunlu�u daha �nce PlacementSystem kodu i�inde CheckPlacementValidity metodunda kontrol edildi
        foreach (var pos in positionsToOccupy) 
        {
            if (placeObjects.ContainsKey(pos))
            {
                throw new Exception($"dicrionary already contains this cell positions{pos}");
            }
            placeObjects[pos]= data;
        }
    }

    // daha �nce obje yerle�tirilmi� alanlar�n konumlar�na g�re kaplad�klar� gridleri tespit eden fonksiyon
    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();// dolu grid h�crelerinin konumlar�n� tutar 
        for (int x  = 0; x < objectSize.x; x++) 
        {
            for (int y = 0; y < objectSize.y; y++) 
            {
                returnVal.Add(gridPosition+new Vector3Int(x,0,y));
            } 
        }

        return returnVal;
    }

    // g�nderilen konuma, g�nderilen obje yerle�tirilebilir mi kontrol eden fonksiyon
    public bool CanPlaceObjectAt(Vector3Int gridPosition,Vector2Int objectSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);

        foreach (var pos in positionToOccupy) // yerle�tirilecek objenin daha �nceki objelerle bir �ak��mas� var m� yok mu kontrol ediliyor
        {
            if (placeObjects.ContainsKey(pos)) // varsa bu konuma yerle�tirilemez oldu�u i�in false d�n�yor
            {
                return false; 
            }
        }
        return true; // uygunsa true
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placeObjects.ContainsKey(gridPosition)==false) // e�er bu pozisyonda silinebilecek obje yoksa -1 d�ner
        {
            Debug.Log(gridPosition.ToString());
            return -1;
        }
        return placeObjects[gridPosition].PlacedObjectIndex; // varsa o objenin kay�ttaki indexini d�ner

    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var obj in placeObjects[gridPosition].occupiedPositions)  // kaplan�lan grid konumlar� list olarak obj ye atan�yor. 
        {
            placeObjects.Remove(obj); // e�er kay�tl� konumlar aras�nda obj listesi varsa bu konumlar� ve bu konumlara kay�tl� yerle�tirme bilgisini siliyor.
         }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions; // objenin konuldu�u konum bilgisi
    public int ID {  get; private set; } // t�r�n�n tespiti i�in id bilgisi tutar
    public int PlacedObjectIndex {  get; private set; } // yerle�tirilen ka��nc� obje oldu�unun bilgisini tutar

    public PlacementData(List<Vector3Int> _occupiedPositions,int id,int _placedObjectIndex)
    {
        this.occupiedPositions =_occupiedPositions;
        ID = id;
        PlacedObjectIndex = _placedObjectIndex;
    }
}
