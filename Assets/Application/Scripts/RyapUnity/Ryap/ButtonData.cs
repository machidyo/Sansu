namespace RyapUnity.Ryap
{
    public enum ButtonState
    {
        Release,
        Push
    }

    public struct ButtonData
    {
        public uint Timestamp { get; private set; }
        public ButtonState ButtonA { get; private set; }
        public ButtonState ButtonB { get; private set; }

        public ButtonData(uint timestamp, byte bits)
        {
            Timestamp = timestamp;
            ButtonA = (bits & 0x01) == 0 ? ButtonState.Release : ButtonState.Push;
            ButtonB = (bits & 0x02) == 0 ? ButtonState.Release : ButtonState.Push;
        }
    }
}