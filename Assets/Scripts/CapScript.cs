using UnityEngine;

public enum CapStates
{
    avalible = 0,
    selected = 1,
    blocking = 2
}

public class CapScript : MonoBehaviour
{
    private Animator capTree;
    private MeshRenderer meshRenderer;
    private Color colorOriginal;
    private CapStates capState;
    private MiniGameManager miniGameManager;
    private Vector3 initPosition;

    [SerializeField] private LayerMask capMask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        capTree = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();
        colorOriginal = meshRenderer.material.GetColor("_BaseColor");
        capState = CapStates.avalible;
        miniGameManager = MiniGameManager.miniGameManager;
        initPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (capState)
        {
            case CapStates.selected:

                if (Input.GetMouseButton(1))
                {
                    capState = CapStates.avalible;
                    transform.position = initPosition;
                    capTree.SetBool("Hover", false);
                    miniGameManager.activeCap = null;
                    return;
                }
                capTree.SetBool("Hover", true);
                transform.position = miniGameManager.cameraPosition.mousePosition;
                break;

            case CapStates.avalible:

                Vector2 mousePosition = Input.mousePosition;
                Ray rayCamera = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(rayCamera, out RaycastHit hitRaycast, 300, capMask) && hitRaycast.collider.TryGetComponent(out CapScript scriptObjeto) && scriptObjeto.name == name && !Physics.Raycast(transform.position, Vector3.up, out RaycastHit detection, 30))
                {

                    meshRenderer.material.SetColor("_BaseColor", colorOriginal * 0.5f);
                    if (Input.GetMouseButton(0))
                    {
                        capState = CapStates.selected;
                        miniGameManager.activeCap = this;
                    }

                }
                else meshRenderer.material.SetColor("_BaseColor", colorOriginal);

                break;
        }

    }

    public void CapSelected(Vector3 activeCap)
    {
        capState = CapStates.blocking;
        transform.position = activeCap;
        meshRenderer.material.SetColor("_BaseColor", colorOriginal * 0.2f);
    }
}
