using SharpOscilloscopeLib;
using System.Diagnostics;

namespace SharpOscilloscope
{
    public partial class Form1 : Form
    {
        private int channel1DeviceIndex;
        private int channel2DeviceIndex;
        private bool running;
        private AudioCapture audioCapture;
        private AudioCapture audioCapture2;

        public Form1()
        {
            InitializeComponent();
            checkBox1.Checked = true;
            checkBox1_CheckedChanged(null, null);
            checkBox2.Checked = false;
            checkBox2_CheckedChanged(null, null);
            comboBox1.SelectedIndex = 3;  //Time
            comboBox1_SelectedIndexChanged(null, null);
            comboBox2.SelectedIndex = 4; //Amplitude
            comboBox2_SelectedIndexChanged(null, null);
            comboBox3.SelectedIndex = 0; //Trigger mode
            comboBox3_SelectedIndexChanged(null, null);
            comboBox4.SelectedIndex = 0; //Trigger type
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                SetupAudioCapture();
                running = true;
                button1.Text = "&Stop";
            }
            else
            {
                audioCapture.StopRecordingChannel1();
                audioCapture2.StopRecordingChannel2();
                signalDisplayControl1.ResetSingle();
                running = false;
                button1.Text = "&Run";
            }
        }

        private void SetupAudioCapture()
        {
            // Initialize audio capture with a callback to process audio data
            audioCapture = new AudioCapture();
            audioCapture.AudioCaptureChannel1(channel1DeviceIndex, 48000, 16, ProcessAudioDataChannel1);
            audioCapture.StartRecordingChannel1();

            audioCapture2 = new AudioCapture();
            audioCapture2.AudioCaptureChannel2(channel2DeviceIndex, 48000, 16, ProcessAudioDataChannel2);
            audioCapture2.StartRecordingChannel2();
        }

        // This will be called when new audio data is available
        private void ProcessAudioDataChannel1(float[] channel1Data, float[] channel1DummyData)
        {
            signalDisplayControl1.UpdateLeftChannelData(channel1Data);
        }

        private void ProcessAudioDataChannel2(float[] channel2Data, float[] channel2DummyData)
        {
            signalDisplayControl1.UpdateRightChannelData(channel2Data);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Settings settings = new Settings();
                settings.channel1Device = channel1DeviceIndex;
                settings.channel2Device = channel2DeviceIndex;

                if (settings.ShowDialog() == DialogResult.OK)
                {
                    channel1DeviceIndex = settings.channel1Device;
                    channel2DeviceIndex = settings.channel2Device;
                }
            }
            catch
            {

            }
        }

        //Time
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: //1ms
                    {
                        signalDisplayControl1.SetTimeScale(0.001f);
                        signalDisplayControl1.SetInterval(3000);
                        break;
                    }
                case 1: //10ms
                    {
                        signalDisplayControl1.SetTimeScale(0.010f);
                        signalDisplayControl1.SetInterval(3000);
                        break;
                    }
                case 2: //100ms
                    {
                        signalDisplayControl1.SetTimeScale(0.100f);
                        signalDisplayControl1.SetInterval(3000);
                        break;
                    }
                case 3: //1sec
                    {
                        signalDisplayControl1.SetTimeScale(1.000f);
                        signalDisplayControl1.SetInterval(3000);
                        break;
                    }
                case 4: //10sec
                    {
                        signalDisplayControl1.SetTimeScale(10.000f);
                        signalDisplayControl1.SetInterval(15000);
                        break;
                    }
                default:
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0: //1mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.001f);
                        break;
                    }
                case 1: //10mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.010f);
                        break;
                    }
                case 2: //100mV
                    {
                        signalDisplayControl1.SetAmplitudeScale(0.100f);
                        break;
                    }
                case 3: //1V
                    {
                        signalDisplayControl1.SetAmplitudeScale(1.000f);
                        break;
                    }
                case 4: //2V
                    {
                        signalDisplayControl1.SetAmplitudeScale(2.000f);
                        break;
                    }
                case 5: //5V
                    {
                        signalDisplayControl1.SetAmplitudeScale(5.000f);
                        break;
                    }
                case 6: //10V
                    {
                        signalDisplayControl1.SetAmplitudeScale(10.000f);
                        break;
                    }
                default:
                    break;
            }
        }

        //Mode
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0: //None
                    {
                        signalDisplayControl1.SetMode(0);
                        break;
                    }
                case 1: //Auto
                    {
                        signalDisplayControl1.SetMode(1);
                        break;
                    }
                case 2: //Normal
                    {
                        signalDisplayControl1.SetMode(2);
                        break;
                    }
                case 3: //Single
                    {
                        signalDisplayControl1.SetMode(3);
                        break;
                    }
                default:
                    break;
            }
        }

        //Type
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            //signalDisplayControl1.tr
        }

        //Trigger level
        private void button2_Click(object sender, EventArgs e)
        {
            float level = 0.3f;
            if (float.TryParse(textBox1.Text, out level))
                signalDisplayControl1.SetLevel(level);
            else
                MessageBox.Show("Numeric values only", "Error");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                signalDisplayControl1.ToggleLeftChannelDisplay(true);
            else
                signalDisplayControl1.ToggleLeftChannelDisplay(false);
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
                signalDisplayControl1.ToggleRightChannelDisplay(true);
            else
                signalDisplayControl1.ToggleRightChannelDisplay(false);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (audioCapture != null)
                {
                    audioCapture.StopRecordingChannel1();
                    audioCapture2.StopRecordingChannel2();
                }
            }
            catch(Exception ex)
            {

            }

            Application.Exit();
        }
    }
}
