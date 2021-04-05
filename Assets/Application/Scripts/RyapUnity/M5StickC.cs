using RyapUnity.Network;
using UnityEngine;

namespace RyapUnity
{
    public class M5StickC : MonoBehaviour
    {
        private UDPReceiver receiver;
        private Quaternion baseQuaternion = Quaternion.identity;

        void Start()
        {
            receiver = FindObjectOfType<UDPReceiver>();
        }

        void Update()
        {
            if (receiver.AhrsData != null)
            {
                UpdateQuaternion(receiver.AhrsData);
            }

            if (receiver.IsButtonAClicked)
            {
                receiver.IsButtonAClicked = false;
                baseQuaternion = Quaternion.identity;
            }
        }

        private void UpdateQuaternion(float[] raw)
        {
            var quaternion = new Quaternion(raw[1], raw[3], raw[2], -raw[0]).normalized;

            if (baseQuaternion == Quaternion.identity)
            {
                baseQuaternion = quaternion;
            }

            transform.rotation = Quaternion.Inverse(baseQuaternion) * quaternion;
        }
    }
}