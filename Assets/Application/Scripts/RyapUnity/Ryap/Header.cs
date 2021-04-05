using RyapUnity.Extention;

namespace RyapUnity.Ryap
{
    internal static class HeaderDef
    {
        public const int HeaderLength = 4;
        public const int ImuDataId = 1;
        public const int ButtonDataId = 2;
    }

    internal struct Header
    {
        public int DataId { get; private set; }
        public int DataLength { get; private set; }

        public Header(byte[] header)
        {
            DataId = header.ToUShort(0);
            DataLength = header.ToUShort(2);
        }
    }
}
