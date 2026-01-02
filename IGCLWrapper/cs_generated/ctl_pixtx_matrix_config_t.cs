using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t"]/*' />
    public partial struct ctl_pixtx_matrix_config_t
    {
        /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t.PreOffsets"]/*' />
        [NativeTypeName("double[3]")]
        public _PreOffsets_e__FixedBuffer PreOffsets;

        /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t.PostOffsets"]/*' />
        [NativeTypeName("double[3]")]
        public _PostOffsets_e__FixedBuffer PostOffsets;

        /// <include file='ctl_pixtx_matrix_config_t.xml' path='doc/member[@name="ctl_pixtx_matrix_config_t.Matrix"]/*' />
        [NativeTypeName("double[3][3]")]
        public _Matrix_e__FixedBuffer Matrix;

        /// <include file='_PreOffsets_e__FixedBuffer.xml' path='doc/member[@name="_PreOffsets_e__FixedBuffer"]/*' />
        [InlineArray(3)]
        public partial struct _PreOffsets_e__FixedBuffer
        {
            public double e0;
        }

        /// <include file='_PostOffsets_e__FixedBuffer.xml' path='doc/member[@name="_PostOffsets_e__FixedBuffer"]/*' />
        [InlineArray(3)]
        public partial struct _PostOffsets_e__FixedBuffer
        {
            public double e0;
        }

        /// <include file='_Matrix_e__FixedBuffer.xml' path='doc/member[@name="_Matrix_e__FixedBuffer"]/*' />
        [InlineArray(3 * 3)]
        public partial struct _Matrix_e__FixedBuffer
        {
            public double e0_0;
        }
    }
}
