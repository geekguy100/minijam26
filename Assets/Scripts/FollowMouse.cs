
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Transform target;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 10f;

        target.position = mousePos;
    }
}
