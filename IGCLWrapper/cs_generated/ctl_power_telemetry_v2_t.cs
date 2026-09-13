using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t"]/*' />
    public partial struct ctl_power_telemetry_v2_t
    {
        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.timeStamp"]/*' />
        public ctl_oc_telemetry_item_t timeStamp;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuEnergyCounter"]/*' />
        public ctl_oc_telemetry_item_t gpuEnergyCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuVoltage"]/*' />
        public ctl_oc_telemetry_item_t gpuVoltage;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuCurrentClockFrequency"]/*' />
        public ctl_oc_telemetry_item_t gpuCurrentClockFrequency;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuCurrentTemperature"]/*' />
        public ctl_oc_telemetry_item_t gpuCurrentTemperature;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.globalActivityCounter"]/*' />
        public ctl_oc_telemetry_item_t globalActivityCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.renderComputeActivityCounter"]/*' />
        public ctl_oc_telemetry_item_t renderComputeActivityCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.mediaActivityCounter"]/*' />
        public ctl_oc_telemetry_item_t mediaActivityCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuPowerLimited"]/*' />
        [NativeTypeName("bool")]
        public byte gpuPowerLimited;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuTemperatureLimited"]/*' />
        [NativeTypeName("bool")]
        public byte gpuTemperatureLimited;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuCurrentLimited"]/*' />
        [NativeTypeName("bool")]
        public byte gpuCurrentLimited;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuVoltageLimited"]/*' />
        [NativeTypeName("bool")]
        public byte gpuVoltageLimited;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuUtilizationLimited"]/*' />
        [NativeTypeName("bool")]
        public byte gpuUtilizationLimited;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramEnergyCounter"]/*' />
        public ctl_oc_telemetry_item_t vramEnergyCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramVoltage"]/*' />
        public ctl_oc_telemetry_item_t vramVoltage;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramCurrentClockFrequency"]/*' />
        public ctl_oc_telemetry_item_t vramCurrentClockFrequency;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramCurrentEffectiveFrequency"]/*' />
        public ctl_oc_telemetry_item_t vramCurrentEffectiveFrequency;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramReadBandwidthCounter"]/*' />
        public ctl_oc_telemetry_item_t vramReadBandwidthCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramWriteBandwidthCounter"]/*' />
        public ctl_oc_telemetry_item_t vramWriteBandwidthCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramCurrentTemperature"]/*' />
        public ctl_oc_telemetry_item_t vramCurrentTemperature;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.totalCardEnergyCounter"]/*' />
        public ctl_oc_telemetry_item_t totalCardEnergyCounter;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.psu"]/*' />
        [NativeTypeName("ctl_psu_info_t[5]")]
        public _psu_e__FixedBuffer psu;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.fanSpeed"]/*' />
        [NativeTypeName("ctl_oc_telemetry_item_t[5]")]
        public _fanSpeed_e__FixedBuffer fanSpeed;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuVrTemp"]/*' />
        public ctl_oc_telemetry_item_t gpuVrTemp;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramVrTemp"]/*' />
        public ctl_oc_telemetry_item_t vramVrTemp;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.saVrTemp"]/*' />
        public ctl_oc_telemetry_item_t saVrTemp;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuEffectiveClock"]/*' />
        public ctl_oc_telemetry_item_t gpuEffectiveClock;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuOverVoltagePercent"]/*' />
        public ctl_oc_telemetry_item_t gpuOverVoltagePercent;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuPowerPercent"]/*' />
        public ctl_oc_telemetry_item_t gpuPowerPercent;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.gpuTemperaturePercent"]/*' />
        public ctl_oc_telemetry_item_t gpuTemperaturePercent;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramReadBandwidth"]/*' />
        public ctl_oc_telemetry_item_t vramReadBandwidth;

        /// <include file='ctl_power_telemetry_v2_t.xml' path='doc/member[@name="ctl_power_telemetry_v2_t.vramWriteBandwidth"]/*' />
        public ctl_oc_telemetry_item_t vramWriteBandwidth;

        /// <include file='_psu_e__FixedBuffer.xml' path='doc/member[@name="_psu_e__FixedBuffer"]/*' />
        [InlineArray(5)]
        public partial struct _psu_e__FixedBuffer
        {
            public ctl_psu_info_t e0;
        }

        /// <include file='_fanSpeed_e__FixedBuffer.xml' path='doc/member[@name="_fanSpeed_e__FixedBuffer"]/*' />
        [InlineArray(5)]
        public partial struct _fanSpeed_e__FixedBuffer
        {
            public ctl_oc_telemetry_item_t e0;
        }
    }
}
