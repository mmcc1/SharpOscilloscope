using SharpOscilloscopeLib;
using System;
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
