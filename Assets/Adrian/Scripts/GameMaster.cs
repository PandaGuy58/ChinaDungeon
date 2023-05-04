using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCamera;
    [SerializeField] CinemachineVirtualCamera gateCameraOne;
    [SerializeField] CinemachineVirtualCamera cutsceneOne;
    [SerializeField] CinemachineVirtualCamera cutsceneTwo;
    [SerializeField] CinemachineVirtualCamera gateCameraTwo;
    [SerializeField] CinemachineVirtualCamera cutsceneThree;

    public List<Vector3> directions = new List<Vector3>();

    public int cameraId;
    public static float activation = -1;
    public float cinemachineTime;

    public GameObject playerGameObject;
    public GameObject currentTriggerZone;

    public bool playerActive;
    public bool cinemachineActive;
    public bool darkScreenFade;

    // Start is called before the first frame update
    void Start()
    {
        darkScreenFade = false;
        cinemachineActive = false;
        cameraId = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        CamManager.Register(mainCamera);
        CamManager.Register(gateCameraOne);
        CamManager.Register(cutsceneOne);
        CamManager.Register(cutsceneTwo);
        CamManager.Register(gateCameraTwo);
        CamManager.Register(cutsceneThree);
    }
    //Unregistering cameras
    private void OnDisable()
    {
        CamManager.Unregister(mainCamera);
        CamManager.Unregister(gateCameraOne);
        CamManager.Unregister(cutsceneOne);
        CamManager.Unregister(cutsceneTwo);
        CamManager.Unregister(gateCameraTwo);
        CamManager.Unregister(cutsceneThree);
    }

    void CameraChange()
    {
        switch (cameraId)
        {
            case 0:
                CamManager.SwitchCamera(mainCamera);
                break;
            case 1:
                CamManager.SwitchCamera(gateCameraOne);
                break;
            case 2:
                CamManager.SwitchCamera(cutsceneOne);
                break;
            case 3:
                CamManager.SwitchCamera(cutsceneTwo);
                break;
            case 4:
                CamManager.SwitchCamera(gateCameraTwo);
                break;
            case 5:
                CamManager.SwitchCamera(cutsceneThree);
                break;
            default:
                Debug.Log("Default");
                break;
        }
    }

    void BackToMainCamera()
    {
        float timePassed = Time.time - activation;
        if (timePassed > cinemachineTime)
        {
            cameraId = 0;
            if (playerActive)
            {
                Debug.Log("ONCE");
                playerGameObject.SetActive(true);
                if (cinemachineActive)
                {
                    Destroy(currentTriggerZone);
                    cinemachineActive = false;
                }
                if(darkScreenFade)
                {
                    GameObject.Find("GameMaster").GetComponent<FadeScreen>().activation = Time.time;
                    GameObject.Find("GameMaster").GetComponent<FadeScreen>().fadeIn = true;
                    GameObject.Find("GameMaster").GetComponent<FadeScreen>().once = true;
                    darkScreenFade = false;
                }

                playerActive = false;
                if (playerGameObject.activeSelf)
                {
                    //GameObject.Find("Ground Check").GetComponent<CheckGround>().groundObjectsDetected = 0;
                    //GameObject.Find("Ground Check").GetComponent<BodyCollisionCheck>().groundObjectsDetected = 0;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        BackToMainCamera();
        CameraChange();
    }
}
