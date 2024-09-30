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

using SharpOscilloscopeLib;
using static SharpOscilloscopeLib.AudioDeviceEnumerator;

namespace SharpOscilloscope
{
    public partial class Settings : Form
    {
        private List<AudioDeviceInfo> audioDeviceInfo;
        public int channel1Device;
        public int channel2Device;

        public Settings()
        {
            InitializeComponent();
            audioDeviceInfo = new List<AudioDeviceInfo>();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            try
            {
                AudioDeviceEnumerator audioDeviceEnumerator = new AudioDeviceEnumerator();
                audioDeviceInfo = audioDeviceEnumerator.GetInputDevices();

                foreach (var device in audioDeviceInfo)
                {
                    comboBox1.Items.Add($"{device.Index}: {device.Name} - {device.SampleRate} Hz, {device.BitDepth}-bit");
                    comboBox2.Items.Add($"{device.Index}: {device.Name} - {device.SampleRate} Hz, {device.BitDepth}-bit");
                }

                comboBox1.SelectedIndex = channel1Device;
                comboBox2.SelectedIndex = channel2Device;
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            channel1Device = comboBox1.SelectedIndex;
            channel2Device = comboBox2.SelectedIndex;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
