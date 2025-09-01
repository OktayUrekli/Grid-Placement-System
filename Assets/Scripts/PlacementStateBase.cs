using UnityEngine;

public class PlacementStateBase
{
    ObjectDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    Grid grid;
    int ID;
    ObjectPlacer objectPlacer;
    PreviewSystem previewSystem;
    int selecetedObjectIndex = -1;
}