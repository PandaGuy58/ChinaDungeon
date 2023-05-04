using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamManager
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    public static CinemachineVirtualCamera ActiveCamera = null;

    public static void SwitchCamera(CinemachineVirtualCamera camera)
    {
        //Setting the priority to make one camera from the list active
        camera.Priority = 10;
        ActiveCamera = camera;
        //Setting the priority of other camers to 0 
        foreach (CinemachineVirtualCamera c in cameras)
        {
            if (c != camera && c.Priority != 0)
            {
                c.Priority = 0;
            }
        }
    }

    //Register the camera
    public static void Register(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }

    //Unregister the camera
    public static void Unregister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
