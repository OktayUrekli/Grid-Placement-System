using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    GridData floorData;
    GridData furnitureData;
    Grid grid;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    int gameObjectIndex = -1;

    public RemovingState(
        GridData floorData,
        GridData furnitureData,
        Grid grid,
        ObjectPlacer objectPlacer,
        PreviewSystem previewSystem)
    {
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.grid = grid;
        this.objectPlacer = objectPlacer;
        this.previewSystem = previewSystem;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectData = null;
        if (furnitureData.CanPlaceObjectAt(gridPosition,Vector2Int.one)==false) // eðer konum dolu ise
        {
            selectData = furnitureData;
        }
        else if (floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one) == false)
        {
            selectData = floorData;
        }

        if (selectData != null) 
        {
            gameObjectIndex = selectData.GetRepresentationIndex(gridPosition); // pozisyonda silinebilecek obje var mý yok mu kontrol ediliyor
            if (gameObjectIndex == -1) // bu pozisyonda silinebilecek obje yok demek
                return;
            selectData.RemoveObjectAt(gridPosition);
            objectPlacer.RemoveObjectAt(gameObjectIndex);
        }
 

        Vector3 cellPosition=grid.CellToWorld(gridPosition);
        previewSystem.UpdatePosition(cellPosition,CheckIfSelectionIsValid(gridPosition));
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition) // bu konumda floor ya da furniture objesi var mý yok mu ona bakýyor
    {
        return !( furnitureData.CanPlaceObjectAt(gridPosition, Vector2Int.one)
            && floorData.CanPlaceObjectAt(gridPosition, Vector2Int.one));   
        // bu konuma obje yerleþtirilebilir ise false dönecek 
        
    }

    public void UpdateState(Vector3Int gridPosition) // mouse her hareket ettiðinde çalýþýr
    {
        bool validity=CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
