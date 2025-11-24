namespace IGCLWrapper
{
    public unsafe partial struct _ctl_kmd_load_features_t
    {
        [NativeTypeName("ctl_application_id_t")]
        public _ctl_application_id_t ReservedFuncID;

        [NativeTypeName("bool")]
        public byte bLoad;

        [NativeTypeName("int64_t")]
        public long SubsetFeatureMask;

        [NativeTypeName("char *")]
        public sbyte* ApplicationName;

        [NativeTypeName("int8_t")]
        public sbyte ApplicationNameLength;

        [NativeTypeName("int8_t")]
        public sbyte CallerComponent;

        [NativeTypeName("int64_t[4]")]
        public fixed long Reserved[4];
    }
}
