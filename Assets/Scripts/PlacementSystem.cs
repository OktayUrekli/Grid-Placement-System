using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] InputManager inputManager; // mouse pozisyonu ve týklama-vazgeçme durumlarýnda  yapýlacaklarý tetiklemek için referans
    [SerializeField] Grid grid; // unity nin kendi grid özelliðinin eklendiði game object 

    [SerializeField] ObjectDatabaseSO database; // yerleþtirilebilecek objelerin tutulduðu so 

    [SerializeField] GameObject gridVisualization; // shader graph grid görselliði saðlayan nesne 

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
        if (buildingState==null) // seçme yada silme iþlemi seçilmediyse
            return;

        Vector3 mousePosition = inputManager.GetSelectedMapPos(); // gridin grid üzerindeki pozisyonu alýnýr
        Vector3Int gridPosition = grid.WorldToCell(mousePosition); // bu posziyonun hangi grid olduðu belirlenir

        if (lastDetectedPosition!=gridPosition)
        {
            buildingState.UpdateState(gridPosition);    
            lastDetectedPosition = gridPosition;
        }
        
    }

    public void StartPlacement(int ID)  // butonlara atanan fonksiyon
    {
        StopPlacement(); // herhangi bir butona týklanýldýðýnda atamalar resetleniyor
        gridVisualization.SetActive(true); // grid görünümü aktif hale getiriliyor
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

    private void PlaceStructure() // obje yerleþtirme gerçekleþtiren fonksiyon
    {
        if (inputManager.IsPointerOverUi()) // eðer mouse ui üzerindeyse
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPos();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);

        buildingState.OnAction(gridPosition);  // obje yerleþtirilecek ve ya silinecek konum bilgisi gönderiliyor
    }

    //bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData; // eðer id 0 ise bu yer(floor) objesi deðilse mobilya(furniture) objesi
    //    return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    //}

    private void StopPlacement() // oyun baþlangýcýnda ve yerleþtirimekten vazgeçme durumunda çalýþacak fonksiyon
    {
        if (buildingState==null) { return; }
        gridVisualization.gameObject.SetActive(false); // grid görünümünü kapatýr
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure; // týklanma durumunda obje yerleþtirme çalýþmayacak
        inputManager.OnExit -= StopPlacement; 
        lastDetectedPosition=Vector3Int.zero;
        buildingState=null;
    }

  
}
