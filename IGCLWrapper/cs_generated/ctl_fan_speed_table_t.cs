using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_fan_speed_table_t.xml' path='doc/member[@name="ctl_fan_speed_table_t"]/*' />
    public partial struct ctl_fan_speed_table_t
    {
        /// <include file='ctl_fan_speed_table_t.xml' path='doc/member[@name="ctl_fan_speed_table_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_fan_speed_table_t.xml' path='doc/member[@name="ctl_fan_speed_table_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_fan_speed_table_t.xml' path='doc/member[@name="ctl_fan_speed_table_t.numPoints"]/*' />
        [NativeTypeName("int32_t")]
        public int numPoints;

        /// <include file='ctl_fan_speed_table_t.xml' path='doc/member[@name="ctl_fan_speed_table_t.table"]/*' />
        [NativeTypeName("ctl_fan_temp_speed_t[32]")]
        public _table_e__FixedBuffer table;

        /// <include file='_table_e__FixedBuffer.xml' path='doc/member[@name="_table_e__FixedBuffer"]/*' />
        [InlineArray(32)]
        public partial struct _table_e__FixedBuffer
        {
            public ctl_fan_temp_speed_t e0;
        }
    }
}
