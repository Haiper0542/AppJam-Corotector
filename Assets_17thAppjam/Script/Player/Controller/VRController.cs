using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public enum PlayerState {  None, Attack, Teleport }

public class VRController : MonoBehaviour
{
    #region Variables

    private SteamVR_TrackedObject trackedObj;

    private SteamVR_Controller.Device controller
    {
        get
        { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    [SerializeField] private GameObject GWeapon;
    [SerializeField] private GameObject SWeapon;
    [SerializeField] private PlayerCtrl player;

    [Space, SerializeField] private Transform TeleportPos;

    private PlayerState playerState;

    private Transform tr;
    private LineRenderer lr;

    private Ray ray;
    private RaycastHit hit;

    private bool isTeleportOn = false;

    private float shottime = 0;

    #endregion

    #region LifeCycle

    private void Start()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
        tr = GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();

        lr.enabled = false;
    }

    private void Update()
    {
        Debug.DrawRay(tr.position, tr.forward * 10f, Color.red);

        if (GameManager.instance.playerClass == PlayerClass.Guardian)
        {
            if (GWeapon != null && controller.GetHairTriggerDown())
            {
                GWeapon.GetComponent<GunCtrl>().Shot();
            }
        }
        else if (GameManager.instance.playerClass == PlayerClass.Surprise)
        {
            if (SWeapon != null && controller.GetHairTrigger())
            {
                if(shottime<=0)
                {
                    SWeapon.GetComponent<GunCtrl>().Shot();
                }
                shottime += Time.deltaTime;
                if(shottime>=0.1f)
                {
                    shottime = 0;
                }
                
            }
        }
        if (controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && !player.isTeleportUsed)
        {
            TeleportOn();
        }
        if (controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad) && !player.isTeleportUsed)
        {
            Teleporting();
        }
        if(controller.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Teleport();
        }
    }

    #endregion

    #region Other Methods

    private void TeleportOn()
    {
        playerState = PlayerState.Teleport;
        lr.enabled = true;
    }

    private void Teleporting()
    {
        ray = new Ray(tr.position, tr.forward * 10f);

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.CompareTag("Ground"))
            {
                if (!isTeleportOn)
                {
                    isTeleportOn = true;
                    TeleportPos.gameObject.SetActive(true);
                }
                TeleportPos.position = hit.point;
            }
            else
            {
                if (isTeleportOn)
                {
                    isTeleportOn = false;
                    TeleportPos.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (isTeleportOn)
            {
                isTeleportOn = false;
                TeleportPos.gameObject.SetActive(false);
            }
        }
    }

    private void Teleport()
    {
        player.isTeleportUsed = true;
        GameManager.instance.VRRoomTr.position = TeleportPos.position;
        TeleportPos.gameObject.SetActive(false);
        playerState = PlayerState.None;
        isTeleportOn = false;
        lr.enabled = false;
    }

    #endregion
}
