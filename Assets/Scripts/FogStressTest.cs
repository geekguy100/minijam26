
using UnityEngine;

public class FogStressTest : MonoBehaviour
{
    private int currentRow;
    private int currentCol;
    
    [SerializeField] private int itemsPerCol;
    [SerializeField] private float dist;
    [SerializeField] private Transform gridStart;
    [SerializeField] private GameObject fogTilePrefab;

    private void AddFog()
    {
        Vector3 position = gridStart.position;
        position += new Vector3(currentCol, 0, currentRow) * dist;

        Instantiate(fogTilePrefab, position, Quaternion.identity);

        ++currentCol;
        if (currentCol >= itemsPerCol)
        {
            currentCol = 0;
            currentRow++;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            AddFog();
        }
    }
}