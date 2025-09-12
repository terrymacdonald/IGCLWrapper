using Xunit;
using System.Text.Json;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void DisplayProperties_Serialization_ShouldWork()
        {
            // Create a display properties structure
            ctl_display_properties_t originalProps = new ctl_display_properties_t();
            originalProps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalProps);
            originalProps.Version = 1;
            originalProps.Type = ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_DISPLAYPORT;
            originalProps.Display_Timing_Info.PixelClock = 533250000; // 533.25 MHz
            originalProps.Display_Timing_Info.HActive = 1920;
            originalProps.Display_Timing_Info.VActive = 1080;
            originalProps.Display_Timing_Info.HTotal = 2200;
            originalProps.Display_Timing_Info.VTotal = 1125;
            originalProps.Display_Timing_Info.RefreshRate = 60.0f;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalProps, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_display_properties_t deserializedProps = JsonSerializer.Deserialize<ctl_display_properties_t>(json);
            Assert.NotNull(deserializedProps);

            // Verify key properties
            Assert.Equal(originalProps.Type, deserializedProps.Type);
            Assert.Equal(originalProps.Display_Timing_Info.PixelClock, deserializedProps.Display_Timing_Info.PixelClock);
            Assert.Equal(originalProps.Display_Timing_Info.HActive, deserializedProps.Display_Timing_Info.HActive);
            Assert.Equal(originalProps.Display_Timing_Info.VActive, deserializedProps.Display_Timing_Info.VActive);
            Assert.Equal(originalProps.Display_Timing_Info.RefreshRate, deserializedProps.Display_Timing_Info.RefreshRate);
        }

        [Fact]
        public void AdapterProperties_Serialization_ShouldWork()
        {
            // Create an adapter properties structure
            ctl_device_adapter_properties_t originalProps = new ctl_device_adapter_properties_t();
            originalProps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalProps);
            originalProps.Version = 1;
            originalProps.device_type = ctl_device_type_t.CTL_DEVICE_TYPE_GRAPHICS;
            originalProps.pci_vendor_id = 0x8086; // Intel vendor ID
            originalProps.pci_device_id = 0x1234;
            originalProps.Frequency = 1800; // 1.8 GHz

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalProps, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_device_adapter_properties_t deserializedProps = JsonSerializer.Deserialize<ctl_device_adapter_properties_t>(json);
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
            ctl_sharpness_settings_t originalSettings = new ctl_sharpness_settings_t();
            originalSettings.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalSettings);
            originalSettings.Version = 1;
            originalSettings.Enable = true;
            originalSettings.FilterType = ctl_sharpness_filter_type_flags_t.CTL_SHARPNESS_FILTER_TYPE_FLAG_ADAPTIVE;
            originalSettings.Intensity = 0.75f;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalSettings, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_sharpness_settings_t deserializedSettings = JsonSerializer.Deserialize<ctl_sharpness_settings_t>(json);
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
            ctl_power_optimization_settings_t originalSettings = new ctl_power_optimization_settings_t();
            originalSettings.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalSettings);
            originalSettings.Version = 1;
            originalSettings.PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED;
            originalSettings.PowerOptimizationFeature = ctl_power_optimization_flags_t.CTL_POWER_OPTIMIZATION_FLAG_PSR;
            originalSettings.Enable = true;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalSettings, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_power_optimization_settings_t deserializedSettings = JsonSerializer.Deserialize<ctl_power_optimization_settings_t>(json);
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
            ctl_scaling_settings_t originalSettings = new ctl_scaling_settings_t();
            originalSettings.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalSettings);
            originalSettings.Version = 1;
            originalSettings.Enable = true;
            originalSettings.ScalingType = ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX;
            originalSettings.CustomScalingX = 95;
            originalSettings.CustomScalingY = 95;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalSettings, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_scaling_settings_t deserializedSettings = JsonSerializer.Deserialize<ctl_scaling_settings_t>(json);
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
            ctl_display_settings_t originalSettings = new ctl_display_settings_t();
            originalSettings.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalSettings);
            originalSettings.Version = 1;
            originalSettings.Set = false; // Get operation
            originalSettings.LowLatency = ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED;
            originalSettings.ContentType = ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING;
            originalSettings.QuantizationRange = ctl_display_setting_quantization_range_t.CTL_DISPLAY_SETTING_QUANTIZATION_RANGE_FULL_RANGE;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalSettings, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_display_settings_t deserializedSettings = JsonSerializer.Deserialize<ctl_display_settings_t>(json);
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
            ctl_freq_state_t originalState = new ctl_freq_state_t();
            originalState.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalState);
            originalState.Version = 1;
            originalState.request = 1800.0; // 1.8 GHz
            originalState.actual = 1750.0;  // 1.75 GHz
            originalState.tdp = 2000.0;     // 2.0 GHz
            originalState.efficient = 300.0; // 300 MHz
            originalState.throttleReasons = ctl_freq_throttle_reason_flags_t.CTL_FREQ_THROTTLE_REASON_FLAG_THERMAL_LIMIT;

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalState, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_freq_state_t deserializedState = JsonSerializer.Deserialize<ctl_freq_state_t>(json);
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
            ctl_mem_state_t originalState = new ctl_mem_state_t();
            originalState.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalState);
            originalState.Version = 1;
            originalState.free = 8L * 1024 * 1024 * 1024; // 8 GB free
            originalState.size = 16L * 1024 * 1024 * 1024; // 16 GB total

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalState, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_mem_state_t deserializedState = JsonSerializer.Deserialize<ctl_mem_state_t>(json);
            Assert.NotNull(deserializedState);

            // Verify properties
            Assert.Equal(originalState.free, deserializedState.free);
            Assert.Equal(originalState.size, deserializedState.size);
        }

        [Fact]
        public void PowerLimits_Serialization_ShouldWork()
        {
            // Create power limits
            ctl_power_limits_t originalLimits = new ctl_power_limits_t();
            originalLimits.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalLimits);
            originalLimits.Version = 1;
            originalLimits.sustainedPowerLimit.enabled = true;
            originalLimits.sustainedPowerLimit.power = 250000; // 250W
            originalLimits.sustainedPowerLimit.interval = 1000; // 1 second
            originalLimits.burstPowerLimit.enabled = true;
            originalLimits.burstPowerLimit.power = 300000; // 300W

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalLimits, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_power_limits_t deserializedLimits = JsonSerializer.Deserialize<ctl_power_limits_t>(json);
            Assert.NotNull(deserializedLimits);

            // Verify properties
            Assert.Equal(originalLimits.sustainedPowerLimit.enabled, deserializedLimits.sustainedPowerLimit.enabled);
            Assert.Equal(originalLimits.sustainedPowerLimit.power, deserializedLimits.sustainedPowerLimit.power);
            Assert.Equal(originalLimits.sustainedPowerLimit.interval, deserializedLimits.sustainedPowerLimit.interval);
            Assert.Equal(originalLimits.burstPowerLimit.enabled, deserializedLimits.burstPowerLimit.enabled);
            Assert.Equal(originalLimits.burstPowerLimit.power, deserializedLimits.burstPowerLimit.power);
        }

        [Fact]
        public void OverclockProperties_Serialization_ShouldWork()
        {
            // Create overclock properties
            ctl_oc_properties_t originalProps = new ctl_oc_properties_t();
            originalProps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(originalProps);
            originalProps.Version = 1;
            originalProps.bSupported = true;
            originalProps.gpuFrequencyOffset.min = -500.0; // -500 MHz
            originalProps.gpuFrequencyOffset.max = 200.0;  // +200 MHz
            originalProps.gpuFrequencyOffset.Default = 0.0; // 0 MHz offset
            originalProps.powerLimit.min = 100000; // 100W
            originalProps.powerLimit.max = 350000; // 350W

            // Serialize to JSON
            string json = JsonSerializer.Serialize(originalProps, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            ctl_oc_properties_t deserializedProps = JsonSerializer.Deserialize<ctl_oc_properties_t>(json);
            Assert.NotNull(deserializedProps);

            // Verify properties
            Assert.Equal(originalProps.bSupported, deserializedProps.bSupported);
            Assert.Equal(originalProps.gpuFrequencyOffset.min, deserializedProps.gpuFrequencyOffset.min);
            Assert.Equal(originalProps.gpuFrequencyOffset.max, deserializedProps.gpuFrequencyOffset.max);
            Assert.Equal(originalProps.gpuFrequencyOffset.Default, deserializedProps.gpuFrequencyOffset.Default);
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
                    Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(new ctl_display_settings_t()),
                    Version = 1,
                    LowLatency = ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED,
                    ContentType = ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING
                },
                PowerSettings = new ctl_power_limits_t
                {
                    Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(new ctl_power_limits_t()),
                    Version = 1,
                    sustainedPowerLimit = new ctl_power_sustained_limit_t
                    {
                        enabled = true,
                        power = 250000,
                        interval = 1000
                    }
                },
                ScalingSettings = new ctl_scaling_settings_t
                {
                    Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(new ctl_scaling_settings_t()),
                    Version = 1,
                    Enable = true,
                    ScalingType = ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX
                }
            };

            // Serialize to JSON
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            Assert.False(string.IsNullOrEmpty(json));

            // Deserialize from JSON
            var deserializedConfig = JsonSerializer.Deserialize<dynamic>(json);
            Assert.NotNull(deserializedConfig);

            // Verify some key properties
            Assert.Equal(config.AdapterIndex, deserializedConfig.GetProperty("AdapterIndex").GetInt32());
            Assert.Equal((int)config.DisplaySettings.LowLatency, deserializedConfig.GetProperty("DisplaySettings").GetProperty("LowLatency").GetInt32());
            Assert.Equal(config.PowerSettings.sustainedPowerLimit.power, deserializedConfig.GetProperty("PowerSettings").GetProperty("sustainedPowerLimit").GetProperty("power").GetInt32());
        }
    }
}
