using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager miniGameManager;
    public CapScript activeCap = null;
    private int[] capCorrectOrder = new int[8] { 3, 6, 0, 1, 4, 7, 5, 2 };
    public Queue<ScrewScript> orderScrew = new Queue<ScrewScript>();
    public bool pauseActive { get; private set; }

    [SerializeField] private ScrewScript[] screwArray = new ScrewScript[8];
    [SerializeField] private CapScript[] capArray = new CapScript[8];
    [SerializeField] public GlobalCameraScript cameraPosition;
    [SerializeField] private Button uiComprobe, uiTutorial;
    [SerializeField] private GameObject textComponent;
    [SerializeField] private GameObject positionMoveCap;

    private Animator textAnimator;
    private TextMeshProUGUI text;

    private void Awake() => miniGameManager = this;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int countScrew = 0; countScrew < screwArray.Length; countScrew++) screwArray[countScrew].screwId = countScrew;
        textAnimator = textComponent.GetComponent<Animator>();
        text = textComponent.GetComponent<TextMeshProUGUI>();

        pauseActive = false;
        uiComprobe.onClick.AddListener(ComprobeOrder);
        uiTutorial.onClick.AddListener(CorrectOrderTesting);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ComprobeOrder()
    {
        if (pauseActive) return;
        activeCap?.CapDeselect();
        pauseActive = true;
        text.text = "Cheking in progress....";
        textAnimator.SetBool("Opacity", true);
        textAnimator.SetFloat("LetterStatus", 0);
        StartCoroutine(ComprobeCaps());
    }

    public void CorrectOrderTesting()
    {
        if (pauseActive) return;
        activeCap?.CapDeselect();
        ResetScrew();
        pauseActive = true;
        text.text = "Running in the correct order....";
        textAnimator.SetBool("Opacity", true);
        textAnimator.SetFloat("LetterStatus", 1);
        StartCoroutine(OrderCaps());
    }

    public IEnumerator OrderCaps()
    {
        Queue<int> correctOrder = new Queue<int>(capCorrectOrder);
        Queue<CapScript> capSelected = new Queue<CapScript>(capArray);
        Vector3 moveCap = positionMoveCap.transform.position;

        while (correctOrder.Count > 0)
        {
            CapScript cap = capSelected.Dequeue();
            ScrewScript screw = screwArray[correctOrder.Dequeue()];
            cap.capActionRotate = () =>
            {
                cap.CapBlueColor();
            };

            cap.CapMovePosition(moveCap);
            while (cap.distanceTarget > 0.01f) yield return null;
            yield return new WaitForSeconds(2);
            screw.ScrewAddCap(cap);
            yield return new WaitForSeconds(0.2f);
            cap.CapCorrectColor();
            screw.ScrewRotateCap();
        }
        yield return new WaitForSeconds(1f);
        foreach (CapScript colorCap in capArray) colorCap.CapCorrectColor();
        text.text = "Complete Correct Order!!";
        yield return new WaitForSeconds(2f);
        ResetScrew();
    }

    public IEnumerator ComprobeCaps()
    {
        Queue<int> correctOrder = new Queue<int>(capCorrectOrder);
        float velocityWait = 1.5f;
        while (orderScrew.Count > 0)
        {
            ScrewScript screw = orderScrew.Dequeue();
            if (screw.screwId == correctOrder.Dequeue()) screw.capSelect.CapCorrectColor();
            else
            {
                screw.capSelect.CapIncorrectColor();
                textAnimator.SetFloat("LetterStatus", 0.5f);
                text.text = "Error OrderCap Detected!!...";
                velocityWait = 0.5f;
            }

            yield return new WaitForSeconds(velocityWait);
        }
        ResetScrew();
    }

    private void ResetScrew()
    {
        foreach (ScrewScript screw in screwArray) screw.ResetScrew();
        orderScrew.Clear();
        miniGameManager.activeCap = null;
        textAnimator.SetBool("Opacity", false);
        pauseActive = false;
    }
}
