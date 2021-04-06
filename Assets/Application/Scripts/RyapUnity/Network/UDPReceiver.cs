using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
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
        private CancellationTokenSource cancel = new CancellationTokenSource();

        void Start()
        {
            udp = new UdpClient(localPort) {Client = {ReceiveTimeout = 500}};
            ThreadMethod().Forget();
        }

        void OnDestroy()
        {
            cancel.Cancel();
        }

        private async UniTask ThreadMethod()
        {
            IPEndPoint remoteEp = null;
            Header header = default;
            ImuData imu = default;
            ButtonData button = default;
            var hasError = false;

            await UniTask.SwitchToThreadPool();
            
            while (!cancel.IsCancellationRequested)
            {
                hasError = false;
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
                    hasError = true;
                }

                if (hasError)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(3), cancellationToken: cancel.Token);
                }
                else
                {
                    await UniTask.Delay(1, cancellationToken: cancel.Token);
                }
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