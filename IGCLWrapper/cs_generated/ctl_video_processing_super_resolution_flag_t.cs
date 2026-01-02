namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t"]/*' />
    public enum ctl_video_processing_super_resolution_flag_t
    {
        /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t.CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_DISABLE"]/*' />
        CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_DISABLE = (1 << 0),

        /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t.CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_DEFAULT_SCENARIO_MODE"]/*' />
        CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_DEFAULT_SCENARIO_MODE = (1 << 1),

        /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t.CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_CONFERENCE_SCENARIO_MODE"]/*' />
        CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_CONFERENCE_SCENARIO_MODE = (1 << 2),

        /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t.CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_CAMERA_SCENARIO_MODE"]/*' />
        CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_ENABLE_CAMERA_SCENARIO_MODE = (1 << 3),

        /// <include file='ctl_video_processing_super_resolution_flag_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_flag_t.CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_MAX"]/*' />
        CTL_VIDEO_PROCESSING_SUPER_RESOLUTION_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
