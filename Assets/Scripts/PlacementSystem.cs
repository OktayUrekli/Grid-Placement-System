using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] InputManager inputManager; // mouse pozisyonu ve t�klama-vazge�me durumlar�nda  yap�lacaklar� tetiklemek i�in referans
    [SerializeField] Grid grid; // unity nin kendi grid �zelli�inin eklendi�i game object 

    [SerializeField] ObjectDatabaseSO database; // yerle�tirilebilecek objelerin tutuldu�u so 

    [SerializeField] GameObject gridVisualization; // shader graph grid g�rselli�i sa�layan nesne 

    GridData floorData,furnitureData; 

    [SerializeField] PreviewSystem previewSystem;

    Vector3Int lastDetectedPosition=Vector3Int.zero;

    [SerializeField] ObjectPlacer objectPlacer;

    IBuildingState buildingState; 

    private void Start()
    {
        StopPlacement();
        gridVisualization.SetActive(false);
        floorData = new GridData();
        furnitureData = new GridData();
    }

    private void Update()
    {
        if (buildingState==null) // se�me yada silme i�lemi se�ilmediyse
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPos(); // gridin grid �zerindeki pozisyonu al�n�r
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // bu posziyonun hangi grid oldu�u belirlenir

        if (lastDetectedPosition!=gridPosition)
        {
            buildingState.UpdateState(gridPosition);    
            lastDetectedPosition = gridPosition;
        }
        
    }

    public void StartPlacement(int ID)  // butonlara atanan fonksiyon
    {
        StopPlacement(); // herhangi bir butona t�klan�ld���nda atamalar resetleniyor
        gridVisualization.SetActive(true); // grid g�r�n�m� aktif hale getiriliyor
        buildingState=new PlacementState(database,
                                         floorData,
                                         furnitureData,
                                         grid,
                                         ID,
                                         objectPlacer,
                                         previewSystem);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(floorData, furnitureData, grid, objectPlacer, previewSystem);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;

    }

    private void PlaceStructure() // obje yerle�tirme ger�ekle�tiren fonksiyon
    {
        if (inputManager.IsPointerOverUi()) // e�er mouse ui �zerindeyse
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);  // obje yerle�tirilecek ve ya silinecek konum bilgisi g�nderiliyor
    }

    //bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData; // e�er id 0 ise bu yer(floor) objesi de�ilse mobilya(furniture) objesi
    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    //}

    private void StopPlacement() // oyun ba�lang�c�nda ve yerle�tirimekten vazge�me durumunda �al��acak fonksiyon
    {
        if (buildingState==null) { return; }
        gridVisualization.gameObject.SetActive(false); // grid g�r�n�m�n� kapat�r
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure; // t�klanma durumunda obje yerle�tirme �al��mayacak
        inputManager.OnExit -= StopPlacement; 
        lastDetectedPosition=Vector3Int.zero;
        buildingState=null;
    }

  
}
