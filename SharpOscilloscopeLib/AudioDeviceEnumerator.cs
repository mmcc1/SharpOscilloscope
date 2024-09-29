/*
 * Sharp Oscilloscope - A windows 2 channel oscilloscope which uses
 * audio inputs.
 * 
 * Copyright (C) 2024  Mark McCarron
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, see
 * <https://www.gnu.org/licenses/>.
 */

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
