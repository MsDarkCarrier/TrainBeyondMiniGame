using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using UnityEngine;

public class GlobalCameraScript : MonoBehaviour
{
    public Vector3 mousePosition { get; private set; }
    private Vector3 mouseWorld;
    [SerializeField] private LayerMask maskCollider;

    // Update is called once per frame
    void Update()
    {
        mouseWorld = Input.mousePosition;
        Ray rayCamera = Camera.main.ScreenPointToRay(mouseWorld);


        if (Physics.Raycast(rayCamera, out RaycastHit hitRaycast, Mathf.Infinity, maskCollider)) mousePosition = hitRaycast.point;
        
    }
}
