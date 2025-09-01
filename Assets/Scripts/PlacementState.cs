using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    ObjectDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    Grid grid;
    int ID;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    int selectedObjectIndex = -1;

    public PlacementState(ObjectDatabaseSO database,
                          GridData floorData,
                          GridData furnitureData,
                          Grid grid,
                          int ýD,
                          ObjectPlacer objectPlacer,
                          PreviewSystem previewSystem)
    {
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.grid = grid;
        ID = ýD;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;


        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID); // basýlan butondan gelen index database de hangi objeyi iþaret ediyor o tespit ediliyor

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else { throw new System.Exception($"no object with ýd {ID}"); }


    }


    public void EndState()
    {
        previewSystem.StopShowingPreview();

    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex); // obje yerleþtirilecek konumun uygunluðu 
        if (placementValidity == false) // uygun deðil ise
            return;

        // uygunsa aþaðýdaki kodlar çalýþýr
        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        // obje dataya kayýt ediliyor
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }


    bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData; // eðer id 0 ise bu yer(floor) objesi deðilse mobilya(furniture) objesi
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex); // bu konum obje yerleþtirmye uygun mu deðil mi bunun sorgusu yapýlýyor
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
