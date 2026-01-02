using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_config_t.xml' path='doc/member[@name="ctl_pixtx_config_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_pixtx_config_t
    {
        /// <include file='ctl_pixtx_config_t.xml' path='doc/member[@name="ctl_pixtx_config_t.OneDLutConfig"]/*' />
        [FieldOffset(0)]
        public ctl_pixtx_1dlut_config_t OneDLutConfig;

        /// <include file='ctl_pixtx_config_t.xml' path='doc/member[@name="ctl_pixtx_config_t.ThreeDLutConfig"]/*' />
        [FieldOffset(0)]
        public ctl_pixtx_3dlut_config_t ThreeDLutConfig;

        /// <include file='ctl_pixtx_config_t.xml' path='doc/member[@name="ctl_pixtx_config_t.MatrixConfig"]/*' />
        [FieldOffset(0)]
        public ctl_pixtx_matrix_config_t MatrixConfig;
    }
}
