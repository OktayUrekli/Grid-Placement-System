using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] Camera sceneCamera;
    Vector3 lastPosition;
    [SerializeField] LayerMask placementLayerMask; // objelerin yerle�tirilebilece�i katman

    public event Action OnClicked,OnExit; // mouse ile t�klanma ve vazge�me durumlar�nda �al��acak fonrksiyonlar� tetikleyen eventler

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // mouse sol t�k bas�lma
            OnClicked?.Invoke();
        if (Input.GetKey(KeyCode.Escape)) // obje yerle�tirmekten vazge�me
            OnExit?.Invoke();
    }

    // e�er mouse ui �zerindeyse true d�ner || grid �zerinde �ng�sterim yap�l�p yap�lmamas� i�in kontrolc� olarak kullan�l�r 
    public bool IsPointerOverUi() => EventSystem.current.IsPointerOverGameObject(); 

    public Vector3 GetSelectedMapPos()
    {
        Vector3 mousePos=Input.mousePosition; // ekran �zerinde mouse pozisyonu al�n�r
        mousePos.z = sceneCamera.nearClipPlane; // nearClipPlane de�erinde daha uzak konumlara yerle�tirilme yap�lacak
        Ray ray=sceneCamera.ScreenPointToRay(mousePos); // mouse pozisyonunu 3d uzayda bir ray e �eviriyor
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,100,placementLayerMask)) // e�er mouse max 100 birim mesafede belirlenen katmandan bir objenin �zerindeyse if i�erisine girer
        {
            lastPosition = hit.point; // dokunulan noktan�n verisi al�n�yor
        }

        return lastPosition;
    }
}
