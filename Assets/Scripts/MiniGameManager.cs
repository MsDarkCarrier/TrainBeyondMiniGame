using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager miniGameManager;
    public CapScript activeCap = null;

    [SerializeField] private ScrewScript[] screwArray = new ScrewScript[8];
    [SerializeField] public GlobalCameraScript cameraPosition;

    private void Awake() => miniGameManager = this;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
