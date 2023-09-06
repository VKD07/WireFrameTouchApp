using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeBehavior : MonoBehaviour
{
    [Header("Selector settings")]
    [SerializeField] Transform spherParent;
    [SerializeField] LayerMask m_layerMask;
    [SerializeField] float m_maxHeightToPlay = 2f;
    [SerializeField] float m_rayCastLength;
    [SerializeField] float m_increaseRate = 200f;
    Quaternion targetRotation;
    RaycastHit hit;
    Vector3 spherePos;
    bool startTimer;
    public float time;

    [Header("Touch")]
    Vector3 sphereNewPos;
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    Vector3 btnNormalPos;
    public float x;
    float cameraTargetYPos;
    float cameraInitYPos;
    public float spherTargetYPos;
    bool hasSelectedSphere;
    [Header("Sphere Detected")]
    [SerializeField] GameObject sphereDetected;
    [SerializeField] Transform mainCamera;
    private void Start()
    {
        cameraInitYPos = mainCamera.transform.position.y;
    }
    void Update()
    {
        SelectSphere();
        Swipe();
        RotateSphereParent();
        MoveCameraAndSpherePositionAndActivateSphere();
    }

    private void SelectSphere()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, m_rayCastLength, m_layerMask))
        {
            sphereDetected = hit.transform.gameObject;
            //SelectAndDragUp();
        }
    }


    private void SelectAndDragUp()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            spherePos = new Vector3(hit.transform.position.x, Mathf.Clamp(hit.point.y, 0f, 4f), hit.transform.position.z);
            hit.transform.position = spherePos;
            //reset
        }
    }

    private void ResetSpherePosition()
    {
        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    if (btnSelected != null)
        //    {
        //        hit.transform.position = btnNormalPos;
        //    }
        //}
        if (Input.GetKeyUp(KeyCode.Mouse0) && sphereDetected != null)
        {
            btnNormalPos = new Vector3(sphereDetected.transform.position.x, 0f, sphereDetected.transform.position.z);
            sphereDetected.transform.position = btnNormalPos;
            //btns = GameObject.FindGameObjectsWithTag("B");
            //for (int i = 0; i < btns.Length; i++)
            //{
            //    btns[i].transform.position = new Vector3(btns[i].transform.position.x, 0f, btns[i].transform.position.z);
            //}
        }
    }
    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTimer = true;
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            startTimer = false;

            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            currentSwipe.Normalize();

            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                Debug.Log("up swipe");
                cameraTargetYPos = 9.75f;
                spherTargetYPos = 8.69f;
                StartCoroutine(ActivateOuterShell());
                hasSelectedSphere = true;
            }
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
            {
                hasSelectedSphere = false;
                Debug.Log("down swipe");
                cameraTargetYPos = cameraInitYPos;
                spherTargetYPos = 0;
                StopAllCoroutines();
                sphereDetected.GetComponent<OuterShell>().EnableOuterSphere(false);
            }

            if (!hasSelectedSphere)
            {
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("left swipe");

                    targetRotation = Quaternion.Euler(0, spherParent.localEulerAngles.y + 120f, 0);
                }
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    Debug.Log("right swipe");
                    targetRotation = Quaternion.Euler(0, spherParent.localEulerAngles.y - 120f, 0);
                }
            }
        }
    }

    void RotateSphereParent()
    {
        float rotationSpeed = 10f; // Adjust the speed as needed.
        float step = rotationSpeed * Time.deltaTime;
        spherParent.localRotation = Quaternion.Slerp(spherParent.localRotation, targetRotation, step);
    }

    void MoveCameraAndSpherePositionAndActivateSphere()
    {
        if (sphereDetected != null && cameraTargetYPos > 0)
        {
            float step = 3 * Time.deltaTime;
            mainCamera.position = new Vector3(mainCamera.position.x, Mathf.Lerp(mainCamera.position.y, cameraTargetYPos, step), mainCamera.position.z);
            sphereDetected.transform.position = new Vector3(sphereDetected.transform.position.x, Mathf.Lerp(sphereDetected.transform.position.y, spherTargetYPos, step), sphereDetected.transform.position.z);
        }
    }
    IEnumerator ActivateOuterShell()
    {
        yield return new WaitForSeconds(1);
        sphereDetected.GetComponent<OuterShell>().EnableOuterSphere(true);
    }
}
