using System;
using System.Threading.Tasks;
using UnityEngine;

public class ScrewScript : MonoBehaviour
{
    public enum ScrewStatus
    {
        nullCap = 0,
        activeCap = 1,
        disableStatus = 2
    }


    [SerializeField] private GameObject capPosition;
    [SerializeField] private LayerMask maskCap;
    public Vector3 vectorCap { get; private set; }
    public int screwId;

    public CapScript capSelect = null;
    private ScrewStatus screwStatus;

    private MiniGameManager miniGameManager;

    private void Awake() => vectorCap = capPosition.transform.position;
    private void Start()
    {
        miniGameManager = MiniGameManager.miniGameManager;
        screwStatus = ScrewStatus.nullCap;
    }
    void Update()
    {
        if (miniGameManager.pauseActive) return;


        switch (screwStatus)
        {
            case ScrewStatus.nullCap:

                if (miniGameManager.activeCap != null && Vector3.Distance(vectorCap, miniGameManager.activeCap.transform.position) <= 5f && Input.GetMouseButtonDown(0))
                {
                    ScrewAddCap(miniGameManager.activeCap);
                    _ = TimeAwaitCap();
                }

                break;

            case ScrewStatus.activeCap:

                bool distanceMouse = Vector3.Distance(vectorCap, miniGameManager.cameraPosition.mousePosition) <= 10 && !capSelect.rotateCap;
                if (distanceMouse && Input.GetMouseButton(1))
                {
                    capSelect.CapDeselect();
                    capSelect = null;
                    screwStatus = ScrewStatus.nullCap;
                }
                else if (distanceMouse && Input.GetMouseButton(0)) ScrewRotateCap();

                break;
        }

    }

    public void ScrewAddCap(CapScript cap)
    {
        cap.CapSelected(vectorCap);
        capSelect = cap;
        screwStatus = ScrewStatus.disableStatus;
    }

    public void ScrewRotateCap()
    {
        _ = capSelect.RotateCap();
        miniGameManager.orderScrew.Enqueue(this);
    }


    public async Task TimeAwaitCap()
    {
        await Awaitable.WaitForSecondsAsync(0.2f);
        screwStatus = ScrewStatus.activeCap;
    }

    public void ResetScrew()
    {
        screwStatus = ScrewStatus.nullCap;

        if (capSelect == null) return;
        capSelect.CapDeselect();
        capSelect = null;
    }
}
