using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterShowRotate : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform character;
    [SerializeField] private float rotateSpeed;


    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if(character != null)
        {
            float rotationY = -eventData.delta.x * rotateSpeed;

            character.Rotate(Vector3.up, rotationY, Space.World);
        }
    }
}
