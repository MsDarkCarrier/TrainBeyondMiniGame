using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class CapScript : MonoBehaviour
{
    public enum CapStates
    {
        avalible = 0,
        selected = 1,
        blocking = 2
    }

    private Animator capTree;
    private MeshRenderer meshRenderer;
    private Material runtimeMaterial; // <-- NUEVO: Guardamos la instancia aquí para evitar fugas
    private Color colorOriginal;
    private CapStates capState;
    private Vector3 initPosition, targetPosition;
    public float distanceTarget { get; private set; }
    private float velocity = 5;
    public bool rotateCap { get; private set; }
    private MiniGameManager miniGameManager;
    public Action capActionRotate;

    [SerializeField] private LayerMask capMask;

    void Start()
    {
        capTree = GetComponent<Animator>();
        meshRenderer = GetComponent<MeshRenderer>();

        runtimeMaterial = meshRenderer.material;
        colorOriginal = runtimeMaterial.GetColor("_BaseColor");

        capState = CapStates.avalible;
        miniGameManager = MiniGameManager.miniGameManager;
        initPosition = transform.position;
        rotateCap = false;
        targetPosition = transform.position;
    }

    void Update()
    {
        Vector3 direction = targetPosition - transform.position;
        distanceTarget = direction.magnitude;

        transform.position = (distanceTarget > 0.01f) ? Vector3.Lerp(transform.position, targetPosition, velocity * Time.deltaTime) : targetPosition;

        if (miniGameManager.pauseActive) return;

        switch (capState)
        {
            case CapStates.selected:
                if (Input.GetMouseButton(1))
                {
                    capState = CapStates.avalible;
                    targetPosition = initPosition;
                    capTree.SetBool("Hover", false);
                    miniGameManager.activeCap = null;
                    return;
                }
                capTree.SetBool("Hover", true);
                targetPosition = miniGameManager.cameraPosition.mousePosition;
                break;

            case CapStates.avalible:
                Vector2 mousePosition = Input.mousePosition;
                Ray rayCamera = Camera.main.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(rayCamera, out RaycastHit hitRaycast, 300, capMask) && hitRaycast.collider.TryGetComponent(out CapScript scriptObjeto) && scriptObjeto.name == name && !Physics.Raycast(transform.position, Vector3.up, out RaycastHit detection, 5))
                {
                    runtimeMaterial.SetColor("_BaseColor", colorOriginal * 0.5f);
                    if (Input.GetMouseButton(0))
                    {
                        capState = CapStates.selected;
                        miniGameManager.activeCap = this;
                        velocity = 50;
                    }
                }
                else runtimeMaterial.SetColor("_BaseColor", colorOriginal);

                break;
        }
    }
    public void CapCorrectColor() => runtimeMaterial.SetColor("_BaseColor", Color.green);
    public void CapIncorrectColor() => runtimeMaterial.SetColor("_BaseColor", Color.red);
    public void CapBlueColor() => runtimeMaterial.SetColor("_BaseColor", Color.blue);

    public void CapMovePosition(Vector3 positionMove)
    {
        velocity = 5;
        targetPosition = positionMove;
        capTree.SetBool("Hover", true);
    }

    public void CapSelected(Vector3 activeCap)
    {
        velocity = 10;
        capState = CapStates.blocking;
        targetPosition = activeCap;
        runtimeMaterial.SetColor("_BaseColor", colorOriginal * 0.2f);
    }

    public void CapDeselect()
    {
        velocity = 5;
        capState = CapStates.avalible;
        targetPosition = initPosition;
        rotateCap = false;
        capTree.SetBool("Hover", false);
        capTree.SetBool("Rotate", false);
        runtimeMaterial.SetColor("_BaseColor", colorOriginal);
    }

    public async Task RotateCap()
    {
        Vector3 positionReference = targetPosition;
        transform.position = targetPosition;
        miniGameManager.activeCap = null;
        capTree.SetBool("Rotate", true);

        rotateCap = true;
        velocity = 5;
        targetPosition = transform.position + new Vector3(0, 0, 1.5f);
        AnimatorStateInfo statusAnimation = capTree.GetCurrentAnimatorStateInfo(0);
        float animationDuration = statusAnimation.length / statusAnimation.speed;

        await Task.Delay((int)(animationDuration * 1000));
        capActionRotate?.Invoke();
        capActionRotate = null;
        if (positionReference != targetPosition) return;

        runtimeMaterial.SetColor("_BaseColor", colorOriginal * 0.5f);
        velocity = 20;
    }

}