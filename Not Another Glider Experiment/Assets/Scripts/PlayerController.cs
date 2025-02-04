using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls;
    GliderController gliderController;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void Start()
    {
        gliderController = GetComponent<GliderController>();
    }
}
