using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformers
{
    public class CameraController : MonoBehaviour
    {

        public GameObject player;
        private Vector3 offset;
        // Start is called before the first frame update
        void Start()
        {
            offset = transform.position;
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            transform.position = player.transform.position + offset;
        }
    
    }
}
