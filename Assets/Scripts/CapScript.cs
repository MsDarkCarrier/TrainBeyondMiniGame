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
                    capTree.SetBool("Hover", false);
                    transform.position = initPosition;
                    return;
                }
                transform.position = miniGameManager.cameraPosition.mousePosition;
                break;

            case CapStates.avalible:

                Vector2 mousePosition = Input.mousePosition;
                Ray rayCamera = Camera.main.ScreenPointToRay(mousePosition);


                if (Physics.Raycast(rayCamera, out RaycastHit hitRaycast, Mathf.Infinity, capMask))
                {
                    meshRenderer.material.SetColor("_BaseColor", colorOriginal * 0.8f);
                    capTree.SetBool("Hover", true);
                    if (Input.GetMouseButton(0))
                    {
                        capTree.SetBool("Hover", false);
                        capState = CapStates.selected;
                    }
                }
                else
                {
                    meshRenderer.material.SetColor("_BaseColor", colorOriginal);
                    capTree.SetBool("Hover", false);
                }
                break;
        }

    }
}
