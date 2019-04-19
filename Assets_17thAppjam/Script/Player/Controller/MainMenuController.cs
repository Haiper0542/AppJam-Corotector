using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    #region Variables

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller
    {
        get
        { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    private PlayerState playerState;

    private Transform tr;
    private LineRenderer lr;

    private Ray ray;
    private RaycastHit hit;

    private VRUI_Button btn;

    #endregion

    #region LifeCycle

    private void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        tr = GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();
        
    }
    
    private void Update()
    {
        Debug.DrawRay(tr.position, tr.forward * 10f, Color.red);
        ray = new Ray(tr.position, tr.forward * 10f);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("VRBtn"))
            {
                if (btn != null)
                {
                    btn.HighlightOff();
                    btn = null;
                }
                btn = hit.collider.GetComponent<VRUI_Button>();
                btn.HighlightOn();
                if(controller.GetHairTriggerDown())
                {
                    btn.OnClick();
                }
            }
            else
            {
                if (btn != null)
                {
                    btn.HighlightOff();
                    btn = null;
                }
            }
        }
        else
        {
            if (btn != null)
            {
                btn.HighlightOff();
                btn = null;
            }
        }
    }

    #endregion

    #region Other Methods

    #endregion
}
