using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{

    [SerializeField] float speed;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        transform.RotateAround(new Vector3(0, 7, 0), new Vector3(0f, 1f, 0f), speed * Time.deltaTime);
    }
}
