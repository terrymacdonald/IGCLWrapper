using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t"]/*' />
    public unsafe partial struct ctl_video_processing_feature_caps_t
    {
        /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t.NumSupportedFeatures"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumSupportedFeatures;

        /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t.pFeatureDetails"]/*' />
        public ctl_video_processing_feature_details_t* pFeatureDetails;

        /// <include file='ctl_video_processing_feature_caps_t.xml' path='doc/member[@name="ctl_video_processing_feature_caps_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[16]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
