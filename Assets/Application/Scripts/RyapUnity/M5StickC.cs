using RyapUnity.Network;
using UniRx;
using UnityEngine;

namespace RyapUnity
{
    public class M5StickC : MonoBehaviour
    {
        private UDPReceiver receiver;
        private Quaternion baseQuaternion = Quaternion.identity;

        public ReactiveProperty<bool> IsButtonAClicked = new ReactiveProperty<bool>();
        
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
            
            IsButtonAClicked.Value = receiver.IsButtonAClicked;
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