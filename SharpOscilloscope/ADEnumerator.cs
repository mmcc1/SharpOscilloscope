using NAudio.Wave;

namespace SharpOscilloscope
{
    public class ADInfo
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public int SampleRate { get; set; }
        public int BitDepth { get; set; }
    }

    public class ADEnumerator
    {
        public List<ADInfo> GetOutputDevices()
        {
            var devices = new List<ADInfo>();

            for (int i = 0; i < WaveOut.DeviceCount; i++)
            {
                var capabilities = WaveOut.GetCapabilities(i);

                // Create a new AudioDeviceInfo object
                var deviceInfo = new ADInfo
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
