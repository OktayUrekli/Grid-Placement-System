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
                          int �D,
                          ObjectPlacer objectPlacer,
                          PreviewSystem previewSystem)
    {
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.grid = grid;
        ID = �D;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;


        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID); // bas�lan butondan gelen index database de hangi objeyi i�aret ediyor o tespit ediliyor

        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else { throw new System.Exception($"no object with �d {ID}"); }


    }


    public void EndState()
    {
        previewSystem.StopShowingPreview();

    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex); // obje yerle�tirilecek konumun uygunlu�u 
        if (placementValidity == false) // uygun de�il ise
            return;

        // uygunsa a�a��daki kodlar �al���r
        int index = objectPlacer.PlaceObject(database.objectData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));

        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData;
        // obje dataya kay�t ediliyor
        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }


    bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectData[selectedObjectIndex].ID == 0 ? floorData : furnitureData; // e�er id 0 ise bu yer(floor) objesi de�ilse mobilya(furniture) objesi
        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex); // bu konum obje yerle�tirmye uygun mu de�il mi bunun sorgusu yap�l�yor
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
