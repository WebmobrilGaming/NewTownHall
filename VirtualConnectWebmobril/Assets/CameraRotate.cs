using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace VirtualConnect
{
    public class CameraRotate : MonoBehaviour
    {
        public float rotateSpeed = 1;
        public float moveSpeed = 1;

        private bool canRotate = false;

        private void Start()
        {
            VcController.OnplayerJoin += OnplayerJoin;
        }

        private void OnplayerJoin()
        {
            if(canRotate) return;
            canRotate = true;
        }

        private void Update()
        {
            if(!canRotate) return;
            
            float rotate = Input.GetAxisRaw("Horizontal");
            float move = Input.GetAxisRaw("Vertical");

            PlayerMove(move);

            transform.Rotate(0,rotate * rotateSpeed * Time.deltaTime,0);
        }

        private void PlayerMove(float move)
        {
            Vector3 playerPos = transform.position;

            if (playerPos.x < 8 && move < 0)
            {
                Vector3 targetpos = Vector3.left * (move * moveSpeed * Time.deltaTime);
                transform.position += targetpos;
            }
            else if (playerPos.x > -8 && move > 0)
            {
                Vector3 targetpos = Vector3.left * (move * moveSpeed * Time.deltaTime);
                transform.position += targetpos;
            }
        }
    }
}
