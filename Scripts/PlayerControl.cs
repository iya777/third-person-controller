using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Iya777
{
    public class PlayerControl : MonoBehaviour
    {
        //Key Bindings
        [SerializeField] private KeyCode walkKey = KeyCode.LeftShift;
        [SerializeField] private KeyCode showCursorKey = KeyCode.Escape;

        // Harus diisi secara manual di inspector
        // Tidak boleh null untuk kedua komponen berikut ini
        [SerializeField] private Camera cam;
        [SerializeField] private Animator anim;

        [SerializeField] private float rotateSpeed = 10f;    // Kecepatan player dalam memutar arah
        [SerializeField] private float walkTransitionSpeed = 10f; // Kecepatan player dalam berpindah dari jalan ke lari
        [SerializeField] private float walkStopTransitionSpeed = 5f; // Kecepatan player dalam berpindah dari jalan ke lari

        // Semua private variabel yang diperlukan untuk mengatur player controller
        private Vector3 dir = Vector3.zero; // direction (arah)
        private Vector3 rot = Vector3.zero; // rotation (putaran)
        private float walkVelocity = 0.0f; // Variable untuk mengatur nilai velocity ketika lari

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (Input.GetKey(showCursorKey))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // Variabel input untuk melakukan pergerakan
            dir.x = Input.GetAxis("Horizontal");
            dir.z = Input.GetAxis("Vertical");

            // Mengatur sprint / walk
            if (Input.GetKey(walkKey))
            {
                walkVelocity = Mathf.Lerp(walkVelocity, 0.8f, Time.fixedDeltaTime * walkTransitionSpeed);
            }
            else
            {
                walkVelocity = Mathf.Lerp(walkVelocity, 0.0f, Time.fixedDeltaTime * walkStopTransitionSpeed);
            }
            walkVelocity = Mathf.Clamp01(walkVelocity);

            // Mengatur kecepatan dari player di animator
            anim.SetFloat("Velocity", Vector3.ClampMagnitude(dir, 1 - walkVelocity).magnitude);

            // Mengatur rotasi dari player
            rot = cam.transform.TransformDirection(dir);
            rot.y = 0;
            transform.forward += Vector3.Lerp(transform.forward, rot, Time.fixedDeltaTime * rotateSpeed);
        }
    }

}
