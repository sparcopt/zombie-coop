using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float Amount = 0.1f;
    public float MaxAmount = 0.3f;
    public float SmoothAmount = 6.0f;
    private Vector3 initPosition;

    void Start()
    {
        initPosition = transform.localPosition;
    }

    void Update()
    {
        var moveX = -Input.GetAxis("Mouse X") * Amount;
        var moveY = -Input.GetAxis("Mouse Y") * Amount;
        moveX = Mathf.Clamp(moveX, -MaxAmount, MaxAmount);
        moveY = Mathf.Clamp(moveY, -MaxAmount, MaxAmount);

        var finalPositionToMove = new Vector3(moveX, moveY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPositionToMove + initPosition, Time.deltaTime * SmoothAmount);
    }
}
