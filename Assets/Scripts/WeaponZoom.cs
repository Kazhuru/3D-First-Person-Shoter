using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class WeaponZoom : MonoBehaviour
{
    [SerializeField] Camera FPSCamera;
    [SerializeField] float normalView = 60f;
    [SerializeField] float zoomView = 30f;
    [SerializeField] float zoomTransitionSpeed = 0.5f;
    [SerializeField] float normalSensivity = 2f;
    [SerializeField] float zoomSensivity = 0.5f;

    private Coroutine currentCorutine;
    private FirstPersonController firstPersonController;

    void Start()
    {
        firstPersonController = FindObjectOfType<FirstPersonController>();
    }

    public void ZoomCameraIn()
    {
        bool fovIsZoom = Mathf.Round(FPSCamera.fieldOfView - 1) <= zoomView;
        if (!fovIsZoom)
        {
            ChangeFieldOfView(zoomView);
            ChangeMouseSensivility(zoomSensivity);
        }
    }

    public void ZoomCameraOut()
    {
        bool fovIsNormal = Mathf.Round(FPSCamera.fieldOfView + 1) >= normalView;
        if (!fovIsNormal)
        {
            ChangeFieldOfView(normalView);
            ChangeMouseSensivility(normalSensivity);
        }
    }

    private void ChangeFieldOfView(float change)
    {
        if (currentCorutine != null)
            StopCoroutine(currentCorutine);

        currentCorutine = StartCoroutine(SmoothZoom(change));
    }

    private void ChangeMouseSensivility(float change)
    {
        firstPersonController.MouseLook.XSensitivity = change;
        firstPersonController.MouseLook.YSensitivity = change;
    }


    private IEnumerator SmoothZoom(float goalValue)
    {
        float currentFieldOfView = FPSCamera.fieldOfView;

        if(currentFieldOfView > goalValue)
        {   //Zoom in
            while (currentFieldOfView >= goalValue)
            {
                currentFieldOfView -= zoomTransitionSpeed * Time.deltaTime;
                FPSCamera.fieldOfView = currentFieldOfView;

                yield return null;
            }
        }
        else
        {   //Zoom out
            while (currentFieldOfView <= goalValue)
            {
                currentFieldOfView += zoomTransitionSpeed * Time.deltaTime;
                FPSCamera.fieldOfView = currentFieldOfView;

                yield return null;
            }
        }
        yield break;
    }
}
