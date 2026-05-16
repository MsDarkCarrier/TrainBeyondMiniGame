using UnityEngine;

public class ScrewScript : MonoBehaviour
{
    [SerializeField] private GameObject capPosition;
    [SerializeField] private LayerMask maskCap;
    public Vector3 vectorCap { get; private set; }
    public bool capCollision { get; private set; }
    private CapScript capSelect = null;

    private MiniGameManager miniGameManager;

    private void Awake() => vectorCap = capPosition.transform.position;
    private void Start() => miniGameManager = MiniGameManager.miniGameManager;
    void Update()
    {
        if (miniGameManager.activeCap != null && Vector3.Distance(vectorCap, miniGameManager.activeCap.transform.position) <= 3.5f && Input.GetMouseButton(0))
        {
            miniGameManager.activeCap.CapSelected(vectorCap);
            capSelect = miniGameManager.activeCap;
        }
    }
}
