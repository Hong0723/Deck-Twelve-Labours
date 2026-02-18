using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cainos.PixelArtTopDown_Basic
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target;
        public float lerpSpeed = 1.0f;

        private Vector3 offset;
        private Vector3 targetPos;

        // 🔥 카메라 이동 제한 값
        public float minX = -19.35f;
        public float maxX = 24f;
        public float minY = -16f;
        public float maxY = 24f;

        private float camHalfHeight;
        private float camHalfWidth;

        private void Start()
        {
            if (target == null) return;

            offset = transform.position - target.position;

            // 카메라 화면 절반 크기 계산
            Camera cam = GetComponent<Camera>();
            camHalfHeight = cam.orthographicSize;
            camHalfWidth = camHalfHeight * cam.aspect;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            targetPos = target.position + offset;

            // 🔥 화면 절반 고려해서 Clamp
            float clampedX = Mathf.Clamp(
                targetPos.x,
                minX + camHalfWidth,
                maxX - camHalfWidth
            );

            float clampedY = Mathf.Clamp(
                targetPos.y,
                minY + camHalfHeight,
                maxY - camHalfHeight
            );

            Vector3 finalPos = new Vector3(clampedX, clampedY, transform.position.z);

            transform.position = Vector3.Lerp(
                transform.position,
                finalPos,
                lerpSpeed * Time.deltaTime
            );
        }
    }
}