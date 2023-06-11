using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogTileBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject fogPrefab;
    
    // Start is called before the first frame update
    private void Start()
    {
        GameObject clone = Instantiate(fogPrefab, GameObject.FindGameObjectWithTag("FogTileRenderer").transform);
        clone.transform.localPosition = transform.position;
        clone.transform.SetSiblingIndex(0);
    }
}
