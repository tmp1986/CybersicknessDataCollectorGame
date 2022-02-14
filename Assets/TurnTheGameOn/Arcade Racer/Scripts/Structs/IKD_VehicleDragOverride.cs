namespace TurnTheGameOn.IKDriver
{
    [System.Serializable]
    public struct IKD_VehicleDragOverride
    {
        public bool enableDragOverride;
        public float maxDrag, maxAngularDrag;
        public float minDrag, minAngularDrag;
    }
}