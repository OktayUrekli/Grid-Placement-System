using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] GameObject mouseIndicator,cellIndicator;
    [SerializeField] InputManager inputManager;
    [SerializeField] Grid grid;

    private void Update()
    {
        Vector3 mousePosition=inputManager.GetSelectedMapPos();
        Vector3Int gridPosition=grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition; 
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
