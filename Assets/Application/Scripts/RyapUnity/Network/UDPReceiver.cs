using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using RyapUnity.Extention;
using RyapUnity.Ryap;
using UnityEngine;

namespace RyapUnity.Network
{
    public class UDPReceiver : MonoBehaviour
    {
        private const int DATA_NUMBER = 4;
    
        [SerializeField] private int localPort = 22222;
    
        public float[] AhrsData { get; private set; } = new float[DATA_NUMBER];
        public bool IsButtonAClicked = false;

        private UdpClient udp;

        void Start()
        {
            udp = new UdpClient(localPort) {Client = {ReceiveTimeout = 500}};
            StartCoroutine(ThreadMethod());
        }

        private IEnumerator ThreadMethod()
        {
            IPEndPoint remoteEp = null;
            Header header = default;
            ImuData imu = default;
            ButtonData button = default;
            
            while (true)
            {
                try
                {
                    var data = udp.Receive(ref  remoteEp);
                    header = new Header(data.Take(HeaderDef.HeaderLength).ToArray());
                    var body = data.Skip(HeaderDef.HeaderLength).ToArray();
                    if (header.DataId == HeaderDef.ImuDataId)
                    {
                        if (TryReadImuData(body, ref imu))
                        {
                            AhrsData = imu.Quaternion;
                        }
                    }
                    else if (header.DataId == HeaderDef.ButtonDataId)
                    {
                        if (TryReadButtonData(body, ref button))
                        {
                            if (button.ButtonA == ButtonState.Push)
                            {
                                Debug.Log("ButtonA Clicked");
                                IsButtonAClicked = true;
                            }
                            if (button.ButtonB == ButtonState.Push)
                            {
                                Debug.Log("ButtonB Clicked");
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                }

                // 30ms is a little less waiting time than ryap(40ms)
                yield return new WaitForSeconds(0.03f);
            }
        }

        private bool TryReadImuData(byte[] bytes, ref ImuData imu) {
            var timestamp = bytes.ToUInt(0);
            var acc = bytes.ToVector3(4);
            var gyro = bytes.ToVector3(16);
            var quat = bytes.ToQuaternion(28);
            imu = new ImuData(timestamp, acc, gyro, quat);
            return true;
        }
        
        public bool TryReadButtonData(byte[]  bytes, ref ButtonData button) {
            var timestamp = bytes.ToUInt(0);
            button = new ButtonData(timestamp, bytes[4]);
            return true;
        }
    }
}