namespace RyapUnity.Ryap
{
    public struct ImuData
    {
        public uint Timestamp { get; private set; }
        public float[] Acc { get; private set; }
        public float[] Gyro { get; private set; }
        public float[] Quaternion { get; private set; }

        public ImuData(uint timestamp, float[] acc, float[] gyro, float[] quaternion)
        {
            Timestamp = timestamp;
            Acc = acc;
            Gyro = gyro;
            Quaternion = quaternion;
        }
    }
}
