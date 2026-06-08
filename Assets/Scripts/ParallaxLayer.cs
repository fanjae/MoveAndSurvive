using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float parallaxFactor = 0.5f;
    [SerializeField] private Transform firstTransform; // 첫번째 배경
    [SerializeField] private Transform secondTransform; // 두번째 배경

    [SerializeField] private float spriteWidth = 40.0f;
    // 카메라 이동량 계산용
    private Vector3 prevCameraPosition;
    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        prevCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        // 이동
        MoveParallax();
        // 반복
        Repeat();

        prevCameraPosition = cameraTransform.position;
    }

    private void MoveParallax()
    {
        // 현재 카메라 이동량 계산
        // 현재 위치 - 이전 위치
        Vector3 cameraDelta = cameraTransform.position - prevCameraPosition;

        transform.position += new Vector3(cameraDelta.x * parallaxFactor, 0.0f, 0.0f);
    }
    private void Repeat()
    {
        float cameraX = cameraTransform.position.x;
        
        if(cameraX - firstTransform.position.x > spriteWidth)
        {
            firstTransform.position = new Vector3(secondTransform.position.x + spriteWidth, firstTransform.position.y, firstTransform.position.z);
        }
        if (cameraX - secondTransform.position.x > spriteWidth)
        {
            secondTransform.position = new Vector3(firstTransform.position.x + spriteWidth, secondTransform.position.y, secondTransform.position.z);
        }

        if(firstTransform.position.x - cameraX > spriteWidth)
        {
            firstTransform.position = new Vector3(secondTransform.position.x - spriteWidth, firstTransform.position.y, firstTransform.position.z);
        }
        if (secondTransform.position.x - cameraX > spriteWidth)
        {
            secondTransform.position = new Vector3(firstTransform.position.x - spriteWidth, secondTransform.position.y, secondTransform.position.z);
        }
    }
}
