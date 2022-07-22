using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Damage
{
    [SerializeField] public float Amount { get; set; }
    [SerializeField] public string Type { get; set; }

    public Damage(float amount, string type)
    {
        Amount = amount;
        Type = type;
    }
}


public class RotatableObject : MonoBehaviour
{
    private static float MAX_DELTA_ROTATION_ANGLE = -16.15399f;
    [SerializeField] private float _rotationSpeed;
    private Vector3 startPosition;
    private Vector3 currentPosition;

    private void Update() {
        // Не знаю как интерпретировать Input.touchInput и что такое Touch touch
        // но предполагаю, что Began это OnMouseDown. Длинна свайпа от точки начала до текущей точки свайпа
        // является показателем интенсивности вращения. В данном варианте вращение не останавливается, а наростает
        transform.RotateAround(transform.position, Vector3.up, CalcRotationIntensivity());
    }

    private float CalcRotationIntensivity()
    {
        return Mathf.Lerp(currentPosition.x, startPosition.x, Time.deltaTime * _rotationSpeed);
    }

    private void OnMouseDown() {
        startPosition = Input.mousePosition;
    }

    private void OnMouseDrag() {
        currentPosition = Input.mousePosition;
    }
}
