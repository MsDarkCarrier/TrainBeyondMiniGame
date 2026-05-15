using UnityEngine;

public class ScrewScript : MonoBehaviour
{
    [SerializeField] private GameObject capPosition;
    public Vector3 vectorCap { get; private set; }
    private void Awake()
    {
        vectorCap = capPosition.transform.position;

    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
}
