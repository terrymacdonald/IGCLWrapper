using Xunit;
using Newtonsoft.Json;
using IGCLWrapper;

namespace IGCLWrapper.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void DisplayProperties_Serialization_ShouldWork()
        {
            // Create a display properties structure
            var originalProps = new ctl_display_properties_t();
            // Note: We can't use Marshal.SizeOf on SWIG-generated classes
            // Size and Version will be set by the actual IGCL API when needed
            originalProps.Type = ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_DISPLAYPORT;
            
            // Serialize to JSON using Newtonsoft.Json
            string json = JsonConvert.SerializeObject(originalProps, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedProps = JsonConvert.DeserializeObject<ctl_display_properties_t>(json);
            Assert.NotNull(deserializedProps);

            // Verify key properties
            Assert.Equal(originalProps.Type, deserializedProps.Type);
        }

        [Fact]
        public void AdapterProperties_Serialization_ShouldWork()
        {
            // Create an adapter properties structure
            var originalProps = new ctl_device_adapter_properties_t();
            originalProps.device_type = ctl_device_type_t.CTL_DEVICE_TYPE_GRAPHICS;
            originalProps.pci_vendor_id = 0x8086; // Intel vendor ID
            originalProps.pci_device_id = 0x1234;
            originalProps.Frequency = 1800; // 1.8 GHz

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalProps, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedProps = JsonConvert.DeserializeObject<ctl_device_adapter_properties_t>(json);
            Assert.NotNull(deserializedProps);

            // Verify key properties
            Assert.Equal(originalProps.device_type, deserializedProps.device_type);
            Assert.Equal(originalProps.pci_vendor_id, deserializedProps.pci_vendor_id);
            Assert.Equal(originalProps.pci_device_id, deserializedProps.pci_device_id);
            Assert.Equal(originalProps.Frequency, deserializedProps.Frequency);
        }

        [Fact]
        public void SharpnessSettings_Serialization_ShouldWork()
        {
            // Create sharpness settings
            var originalSettings = new ctl_sharpness_settings_t();
            originalSettings.Enable = true;
            originalSettings.FilterType = (uint)ctl_sharpness_filter_type_flag_t.CTL_SHARPNESS_FILTER_TYPE_FLAG_ADAPTIVE;
            originalSettings.Intensity = 0.75f;

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalSettings, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedSettings = JsonConvert.DeserializeObject<ctl_sharpness_settings_t>(json);
            Assert.NotNull(deserializedSettings);

            // Verify properties
            Assert.Equal(originalSettings.Enable, deserializedSettings.Enable);
            Assert.Equal(originalSettings.FilterType, deserializedSettings.FilterType);
            Assert.Equal(originalSettings.Intensity, deserializedSettings.Intensity);
        }

        [Fact]
        public void PowerOptimizationSettings_Serialization_ShouldWork()
        {
            // Create power optimization settings
            var originalSettings = new ctl_power_optimization_settings_t();
            originalSettings.PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED;
            originalSettings.PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR;
            originalSettings.Enable = true;

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalSettings, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedSettings = JsonConvert.DeserializeObject<ctl_power_optimization_settings_t>(json);
            Assert.NotNull(deserializedSettings);

            // Verify properties
            Assert.Equal(originalSettings.PowerOptimizationPlan, deserializedSettings.PowerOptimizationPlan);
            Assert.Equal(originalSettings.PowerOptimizationFeature, deserializedSettings.PowerOptimizationFeature);
            Assert.Equal(originalSettings.Enable, deserializedSettings.Enable);
        }

        [Fact]
        public void ScalingSettings_Serialization_ShouldWork()
        {
            // Create scaling settings
            var originalSettings = new ctl_scaling_settings_t();
            originalSettings.Enable = true;
            originalSettings.ScalingType = (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX;
            originalSettings.CustomScalingX = 95;
            originalSettings.CustomScalingY = 95;

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalSettings, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedSettings = JsonConvert.DeserializeObject<ctl_scaling_settings_t>(json);
            Assert.NotNull(deserializedSettings);

            // Verify properties
            Assert.Equal(originalSettings.Enable, deserializedSettings.Enable);
            Assert.Equal(originalSettings.ScalingType, deserializedSettings.ScalingType);
            Assert.Equal(originalSettings.CustomScalingX, deserializedSettings.CustomScalingX);
            Assert.Equal(originalSettings.CustomScalingY, deserializedSettings.CustomScalingY);
        }

        [Fact]
        public void DisplaySettings_Serialization_ShouldWork()
        {
            // Create display settings
            var originalSettings = new ctl_display_settings_t();
            originalSettings.Set = false; // Get operation
            originalSettings.LowLatency = ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED;
            originalSettings.ContentType = ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING;
            originalSettings.QuantizationRange = ctl_display_setting_quantization_range_t.CTL_DISPLAY_SETTING_QUANTIZATION_RANGE_FULL_RANGE;

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalSettings, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedSettings = JsonConvert.DeserializeObject<ctl_display_settings_t>(json);
            Assert.NotNull(deserializedSettings);

            // Verify properties
            Assert.Equal(originalSettings.LowLatency, deserializedSettings.LowLatency);
            Assert.Equal(originalSettings.ContentType, deserializedSettings.ContentType);
            Assert.Equal(originalSettings.QuantizationRange, deserializedSettings.QuantizationRange);
        }

        [Fact]
        public void FrequencyState_Serialization_ShouldWork()
        {
            // Create frequency state
            var originalState = new ctl_freq_state_t();
            originalState.request = 1800.0; // 1.8 GHz
            originalState.actual = 1750.0;  // 1.75 GHz
            originalState.tdp = 2000.0;     // 2.0 GHz
            originalState.efficient = 300.0; // 300 MHz
            originalState.throttleReasons = (uint)ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_THERMAL_LIMIT;

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalState, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedState = JsonConvert.DeserializeObject<ctl_freq_state_t>(json);
            Assert.NotNull(deserializedState);

            // Verify properties
            Assert.Equal(originalState.request, deserializedState.request);
            Assert.Equal(originalState.actual, deserializedState.actual);
            Assert.Equal(originalState.tdp, deserializedState.tdp);
            Assert.Equal(originalState.efficient, deserializedState.efficient);
            Assert.Equal(originalState.throttleReasons, deserializedState.throttleReasons);
        }

        [Fact]
        public void MemoryState_Serialization_ShouldWork()
        {
            // Create memory state
            var originalState = new ctl_mem_state_t();
            originalState.free = 8L * 1024 * 1024 * 1024; // 8 GB free
            originalState.size = 16L * 1024 * 1024 * 1024; // 16 GB total

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalState, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedState = JsonConvert.DeserializeObject<ctl_mem_state_t>(json);
            Assert.NotNull(deserializedState);

            // Verify properties
            Assert.Equal(originalState.free, deserializedState.free);
            Assert.Equal(originalState.size, deserializedState.size);
        }

        [Fact]
        public void PowerLimits_Serialization_ShouldWork()
        {
            // Create power limits
            var originalLimits = new ctl_power_limits_t();
            originalLimits.sustainedPowerLimit = new ctl_power_sustained_limit_t();
            originalLimits.sustainedPowerLimit.enabled = true;
            originalLimits.sustainedPowerLimit.power = 250000; // 250W
            originalLimits.sustainedPowerLimit.interval = 1000; // 1 second
            originalLimits.burstPowerLimit = new ctl_power_burst_limit_t();
            originalLimits.burstPowerLimit.enabled = true;
            originalLimits.burstPowerLimit.power = 300000; // 300W

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalLimits, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedLimits = JsonConvert.DeserializeObject<ctl_power_limits_t>(json);
            Assert.NotNull(deserializedLimits);

            // Verify properties
            Assert.NotNull(deserializedLimits.sustainedPowerLimit);
            Assert.Equal(originalLimits.sustainedPowerLimit.enabled, deserializedLimits.sustainedPowerLimit.enabled);
            Assert.Equal(originalLimits.sustainedPowerLimit.power, deserializedLimits.sustainedPowerLimit.power);
            Assert.Equal(originalLimits.sustainedPowerLimit.interval, deserializedLimits.sustainedPowerLimit.interval);
            Assert.NotNull(deserializedLimits.burstPowerLimit);
            Assert.Equal(originalLimits.burstPowerLimit.enabled, deserializedLimits.burstPowerLimit.enabled);
            Assert.Equal(originalLimits.burstPowerLimit.power, deserializedLimits.burstPowerLimit.power);
        }

        [Fact]
        public void OverclockProperties_Serialization_ShouldWork()
        {
            // Create overclock properties
            var originalProps = new ctl_oc_properties_t();
            originalProps.bSupported = true;
            originalProps.gpuFrequencyOffset = new ctl_oc_control_info_t();
            originalProps.gpuFrequencyOffset.min = -500.0; // -500 MHz
            originalProps.gpuFrequencyOffset.max = 200.0;  // +200 MHz
            originalProps.gpuFrequencyOffset.Default = 0.0; // 0 MHz offset
            originalProps.powerLimit = new ctl_oc_control_info_t();
            originalProps.powerLimit.min = 100000; // 100W
            originalProps.powerLimit.max = 350000; // 350W

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(originalProps, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedProps = JsonConvert.DeserializeObject<ctl_oc_properties_t>(json);
            Assert.NotNull(deserializedProps);

            // Verify properties
            Assert.Equal(originalProps.bSupported, deserializedProps.bSupported);
            Assert.NotNull(deserializedProps.gpuFrequencyOffset);
            Assert.Equal(originalProps.gpuFrequencyOffset.min, deserializedProps.gpuFrequencyOffset.min);
            Assert.Equal(originalProps.gpuFrequencyOffset.max, deserializedProps.gpuFrequencyOffset.max);
            Assert.Equal(originalProps.gpuFrequencyOffset.Default, deserializedProps.gpuFrequencyOffset.Default);
            Assert.NotNull(deserializedProps.powerLimit);
            Assert.Equal(originalProps.powerLimit.min, deserializedProps.powerLimit.min);
            Assert.Equal(originalProps.powerLimit.max, deserializedProps.powerLimit.max);
        }

        [Fact]
        public void ComplexConfiguration_Serialization_ShouldWork()
        {
            // Create a complex configuration object that might be used in an application
            var config = new
            {
                AdapterIndex = 0,
                DisplaySettings = new ctl_display_settings_t
                {
                    LowLatency = ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED,
                    ContentType = ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING
                },
                PowerSettings = new ctl_power_limits_t
                {
                    sustainedPowerLimit = new ctl_power_sustained_limit_t
                    {
                        enabled = true,
                        power = 250000,
                        interval = 1000
                    }
                },
                ScalingSettings = new ctl_scaling_settings_t
                {
                    Enable = true,
                    ScalingType = (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX
                }
            };

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(config, Formatting.Indented);
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedConfig = JsonConvert.DeserializeObject<dynamic>(json);
            Assert.NotNull(deserializedConfig);

            // Verify some key properties
            Assert.Equal(config.AdapterIndex, (int)deserializedConfig.AdapterIndex);
            Assert.Equal((int)config.DisplaySettings.LowLatency, (int)deserializedConfig.DisplaySettings.LowLatency);
            Assert.Equal(config.PowerSettings.sustainedPowerLimit.power, (int)deserializedConfig.PowerSettings.sustainedPowerLimit.power);
        }
    }
}



