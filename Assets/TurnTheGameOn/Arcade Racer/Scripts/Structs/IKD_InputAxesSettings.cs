namespace TurnTheGameOn.IKDriver
{
    [System.Serializable]
    public struct IKD_InputAxesSettings
    {
        public string name;
        public string steering;
        public string throttle;
        public string brake;
        public string nitro;
        public string handBrake;
        public string shiftUp;
        public string shiftDown;
        public string lookBack;
        public string cycleCamera;
        public string horizontalCameraRotation;
        public bool invertFootBrake;
        public bool releaseBrakeToReverse;
    }
}