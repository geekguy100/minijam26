using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLineDrawer : MonoBehaviour
{
    [SerializeField] private GridField gridField;
    [SerializeField] private GameObject linePrefab;

    private void OnEnable()
    {
        gridField.onPopulationComplete.AddListener(Populate);
    }

    private void OnDisable()
    {
        gridField.onPopulationComplete.RemoveListener(Populate);
    }

    [ContextMenu("Populate")]
    private void Populate()
    {
        float height = gridField.GridHeight * gridField.GridHeightSpacing;
        float width = gridField.GridWidth * gridField.GridWidthSpacing;

        for (int col = 0; col <= gridField.GridWidth; ++col)
        {
            Transform lineClone = Instantiate(linePrefab, transform).transform;
            
            lineClone.localPosition = new Vector3(col * gridField.GridWidthSpacing - gridField.GridWidthSpacing / 2, 0, height / 2f);

            Vector3 scale = Vector3.one;
            scale.z = height;
            lineClone.localScale = scale;
        }

        for (int row = 0; row <= gridField.GridHeight; ++row)
        {
            Transform lineClone = Instantiate(linePrefab, transform).transform;
            
            lineClone.localPosition = new Vector3(width/2f, 0, row * gridField.GridHeightSpacing - gridField.GridHeightSpacing / 2);

            Vector3 scale = Vector3.one;
            scale.x = width;
            lineClone.localScale = scale;
        }
    }
}