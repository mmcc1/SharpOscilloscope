using NAudio.Wave;

namespace SharpOscilloscopeLib
{
    public class AudioDeviceEnumerator
    {
        public class AudioDeviceInfo
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public int SampleRate { get; set; }
            public int BitDepth { get; set; }
        }

        public List<AudioDeviceInfo> GetInputDevices()
        {
            var devices = new List<AudioDeviceInfo>();
            
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var capabilities = WaveIn.GetCapabilities(i);

                // Create a new AudioDeviceInfo object
                var deviceInfo = new AudioDeviceInfo
                {
                    Index = i,
                    Name = capabilities.ProductName,
                    SampleRate = capabilities.SupportsWaveFormat(SupportedWaveFormat.WAVE_FORMAT_48M16) ? 48000 : 0, // Assuming 48kHz as standard
                    BitDepth = 16
                };

                devices.Add(deviceInfo);
            }

            return devices;
        }
    }

}
