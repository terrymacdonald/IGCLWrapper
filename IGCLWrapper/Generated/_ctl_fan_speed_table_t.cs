namespace IGCLWrapper
{
    public partial struct _ctl_fan_speed_table_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("int32_t")]
        public int numPoints;

        [NativeTypeName("ctl_fan_temp_speed_t[32]")]
        public _table_e__FixedBuffer table;

        public partial struct _table_e__FixedBuffer
        {
            public _ctl_fan_temp_speed_t e0;
            public _ctl_fan_temp_speed_t e1;
            public _ctl_fan_temp_speed_t e2;
            public _ctl_fan_temp_speed_t e3;
            public _ctl_fan_temp_speed_t e4;
            public _ctl_fan_temp_speed_t e5;
            public _ctl_fan_temp_speed_t e6;
            public _ctl_fan_temp_speed_t e7;
            public _ctl_fan_temp_speed_t e8;
            public _ctl_fan_temp_speed_t e9;
            public _ctl_fan_temp_speed_t e10;
            public _ctl_fan_temp_speed_t e11;
            public _ctl_fan_temp_speed_t e12;
            public _ctl_fan_temp_speed_t e13;
            public _ctl_fan_temp_speed_t e14;
            public _ctl_fan_temp_speed_t e15;
            public _ctl_fan_temp_speed_t e16;
            public _ctl_fan_temp_speed_t e17;
            public _ctl_fan_temp_speed_t e18;
            public _ctl_fan_temp_speed_t e19;
            public _ctl_fan_temp_speed_t e20;
            public _ctl_fan_temp_speed_t e21;
            public _ctl_fan_temp_speed_t e22;
            public _ctl_fan_temp_speed_t e23;
            public _ctl_fan_temp_speed_t e24;
            public _ctl_fan_temp_speed_t e25;
            public _ctl_fan_temp_speed_t e26;
            public _ctl_fan_temp_speed_t e27;
            public _ctl_fan_temp_speed_t e28;
            public _ctl_fan_temp_speed_t e29;
            public _ctl_fan_temp_speed_t e30;
            public _ctl_fan_temp_speed_t e31;

            public unsafe ref _ctl_fan_temp_speed_t this[int index]
            {
                get
                {
                    fixed (_ctl_fan_temp_speed_t* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }
}
