using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    Vector3 lastPosition;
    [SerializeField] LayerMask placementLayerMask; // objelerin yerleþtirilebileceði katman

    public event Action OnClicked,OnExit; // mouse ile týklanma ve vazgeçme durumlarýnda çalýþacak fonrksiyonlarý tetikleyen eventler

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // mouse sol týk basýlma
            OnClicked?.Invoke();
        if (Input.GetKey(KeyCode.Escape)) // obje yerleþtirmekten vazgeçme
            OnExit?.Invoke();
    }

    // eðer mouse ui üzerindeyse true döner || grid üzerinde öngösterim yapýlýp yapýlmamasý için kontrolcü olarak kullanýlýr 
    public bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject(); 

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos=Input.mousePosition; // ekran üzerinde mouse pozisyonu alýnýr
        mousePos.z = sceneCamera.nearClipPlane; // nearClipPlane deðerinde daha uzak konumlara yerleþtirilme yapýlacak
        Ray ray=sceneCamera.ScreenPointToRay(mousePos); // mouse pozisyonunu 3d uzayda bir ray e çeviriyor
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100,placementLayerMask)) // eðer mouse max 100 birim mesafede belirlenen katmandan bir objenin üzerindeyse if içerisine girer
        {
            lastPosition = hit.point; // dokunulan noktanýn verisi alýnýyor
        }

        return lastPosition;
    }
}
