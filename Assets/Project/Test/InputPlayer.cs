using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.XInput;

public struct KeyState
{
    public string Name, Device;
    public float Value;
    public bool Pressed;
    public KeyState(string name, string device, float value, bool pressed)
    {
        Name = name;
        Device = device;
        Value = value;
        Pressed = pressed;
    }
}
public class InputPlayer : MonoBehaviour, IPause
{
    public UnityEvent<float> OnMoveBody, OnRotateBody, OnRotateHead;
    public UnityEvent OnShoot, OnSwitchWeapon, OnUseSkill;
    private XInputController _xbox;
    private bool _leftShoulderUp;
    private void OnEnable()
    {
        _xbox = InputSystem.GetDevice<XInputController>();
        InputSystem.onDeviceChange += OnDeviceChange;
        PauseManager.PausedObjects.Add(this);
    }
    private void OnDestroy()
    {
        PauseManager.PausedObjects.Remove(this);
    }
    private void OnDisable()
    {
        PauseManager.PausedObjects.Remove(this);
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (device)
        {
            case XInputController xc when change == InputDeviceChange.Added:
                _xbox = xc;
                Debug.Log($"XInput контроллер подключен. subtype: {_xbox.subType}, flags: {_xbox.flags}");
                break;
            case XInputController when change == InputDeviceChange.Removed:
                _xbox = null;
                Debug.Log("XInput контроллер отключен.");
                break;
        }
    }
    /*
    private void OnEnable()
    {
        /*InputSystem.onAnyButtonPress.Call(OnAnyButton);#1#
        /*InputSystem.onEvent += OnInputEvent;#1#
    }
    private void OnDisable()
    {
        InputSystem.onEvent -= OnInputEvent;
    }
    private void OnAnyButton(InputControl control)
    {
        _keys.Add(new KeyState(control.name, control.device.name, 1, false));
        /*Debug.Log("Нажата кнопка: " + control.name);#1#
    }private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
    {
        foreach (InputControl control in device.allControls)
        {
            if (control is not AxisControl axis) continue;
            float val = axis.ReadValue();
            if (!(Mathf.Abs(val) > 0.01f) || axis.name == "x" || axis.name == "y" || axis.name == "pressure" ||
                axis.name == "press" || axis.name == "anyKey") continue;
            _keys.Add(new KeyState(axis.name, device.name, val, false));
            Debug.Log($"{device.name} {axis.name}: {val:F2}");
        }
    }*/

    public void UpdatePause()
    {
        if(Input.GetKeyDown(KeyCode.Space)) Shoot();
        if(Input.GetKeyDown(KeyCode.CapsLock)) UseSkill();
        if(Input.GetKeyDown(KeyCode.LeftShift)) SwitchWeapon();
        
        if(_xbox == null) return;
        if (_xbox.leftShoulder.isPressed && !_leftShoulderUp)
        {
            _leftShoulderUp = true;
            StartCoroutine(_xbox.ITemporaryVibration(1,0,0.5f));
            Shoot();
        }

        if (_xbox.leftShoulder.isPressed) return;
        _leftShoulderUp = false;
    }
    public void FixedUpdatePause()
    {
        if(Input.GetKey(KeyCode.W)) MoveBody(1);
        if(Input.GetKey(KeyCode.S)) MoveBody(-1);
        if(Input.GetKey(KeyCode.D)) RotateBody(1);
        if(Input.GetKey(KeyCode.A)) RotateBody(-1);
        if(Input.GetKey(KeyCode.E)) RotateHead(1);
        if(Input.GetKey(KeyCode.Q)) RotateHead(-1);
        
        if(_xbox == null) return;
        Vector2 stick = _xbox.leftStick.ReadValue();
        MoveBody(stick.y);
        RotateBody(stick.x * stick.x * stick.x);
        /*_xbox.SetMotorSpeeds(Mathf.Abs(stick.y), Mathf.Abs(stick.y));*/
    }
    public void LateUpdatePause()
    {
    }

    private void MoveBody(float speed)
    {
        OnMoveBody?.Invoke(speed);
    }

    private void RotateBody(float speed)
    {
        OnRotateBody?.Invoke(speed);
    }
    private void RotateHead(float speed)
    {
        OnRotateHead?.Invoke(speed);
    }
    private void Shoot()
    {
       OnShoot?.Invoke();
       Debug.Log("Shoot");
    }
    private void SwitchWeapon()
    {
        OnSwitchWeapon?.Invoke();
        Debug.Log("SwitchWeapon");
    }
    private void UseSkill()
    {
        OnUseSkill?.Invoke();
        Debug.Log("UseSkill");
    }
}
