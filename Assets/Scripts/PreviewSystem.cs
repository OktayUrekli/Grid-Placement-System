using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField] float previewYOffset = 0.06f; // grid den yukarýda olmasýný saðlar - z fight yaþanmaz
    [SerializeField] GameObject cellIndicator;
    GameObject previewObject;

    [SerializeField] Material previewMaterialsPrefab;
    Material previewMaterialInstance;

    Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.gameObject.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }


    public void StartShowingPlacementPreview(GameObject prefab,Vector2Int size)
    {
        previewObject=Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareCursor(size);
        cellIndicator.SetActive(true);
    }

    private void PrepareCursor(Vector2Int size)
    {
        if (size.x>0  && size.y>0)
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y); // gösterge boyutunu obje boyutuna getiriyor
            cellIndicatorRenderer.material.mainTextureScale = size; // gösterge objenin kapladýðý her gridi göstermesi için            
        }
    }

    // yerleþtirilecek objenin öngösterimi için transparan material(preview material) atamasý gerçekleþtiriliyor
    void PreparePreview(GameObject previewObject) 
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }


    // yerleþtirme gerçekleþtikten sonra öngösterimi kapatan fonksiyon
    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
            Destroy(previewObject);
    }


    // ön gösterimin(preview objesi) konumunu güncelleyen fonksiyon
    // eðer uygunsa beyaz renkte deðilse kýrmýzý renkte gösterim yapýlacak
    public void UpdatePosition(Vector3 position,bool validity)
    {
        if (previewObject!=null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }

        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }

    private void ApplyFeedbackToPreview(bool validity) // preview rengi belirleniyor
    {
        Color c =validity? Color.white : Color.red;
        c.a = 0.5f;
        previewMaterialInstance.color = c;
    }

    private void ApplyFeedbackToCursor(bool validity)// hücre/grid rengi belirleniyor
    {
        Color c = validity ? Color.white : Color.red;
        c.a = 0.5f;
        cellIndicatorRenderer.material.color = c;
    }

    private void MoveCursor(Vector3 position)// grid konumu belirleniyor
    {
        cellIndicator.transform.position = position;    
    }

    private void MovePreview(Vector3 position)// preview konumu belirleniyor
    {
        previewObject.transform.position = new Vector3(position.x,position.y+previewYOffset,position.z);
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        PrepareCursor(Vector2Int.one);
        ApplyFeedbackToCursor(false);
    }
}
