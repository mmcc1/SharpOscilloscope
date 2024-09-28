using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpOscilloscopeScope
{
    public enum TriggerType { Edge, Level, Pulse, Slope }
    public enum TriggerMode { None, Auto, Normal, Single }

    internal class TriggerSystem
    {
        public TriggerType TriggerType { get; set; } = TriggerType.Edge;
        public TriggerMode TriggerMode { get; set; } = TriggerMode.None;
        public float TriggerLevelChannel1 { get; set; } = 0.3f;
        public float TriggerLevelChannel2 { get; set; } = 0.3f;
        public bool TriggerOnRisingEdge { get; set; } = true;

        public bool IsTriggeredChannel1 { get; set; } = false;
        public bool IsTriggeredChannel2 { get; set; } = false;
        private float preTriggerBufferChannel1;
        private float preTriggerBufferChannel2;
        private bool waitingForTriggerChannel1 = true;
        private bool waitingForTriggerChannel2 = true;

        public bool CheckTriggerChannel1(float currentSample, float previousSample)
        {
            if (IsTriggeredChannel1) return false; // Avoid re-triggering while already triggered

            switch (TriggerType)
            {
                case TriggerType.Edge:
                    if (TriggerOnRisingEdge)
                    {
                        // Trigger on a rising edge crossing the trigger level
                        if (previousSample < TriggerLevelChannel1 && currentSample >= TriggerLevelChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }
                    }
                    else
                    {
                        // Trigger on a falling edge crossing the trigger level
                        if (previousSample > TriggerLevelChannel1 && currentSample <= TriggerLevelChannel1)
                        {
                            IsTriggeredChannel1 = true;
                        }
                    }
                    break;

                case TriggerType.Level:
                    if (currentSample >= TriggerLevelChannel1)
                    {
                        IsTriggeredChannel1 = true;
                    }
                    break;

                    // Additional logic for Pulse and Slope triggering can be added here
            }

            return IsTriggeredChannel1;
        }

        public bool CheckTriggerChannel2(float currentSample, float previousSample)
        {
            if (IsTriggeredChannel2) return false; // Avoid re-triggering while already triggered

            switch (TriggerType)
            {
                case TriggerType.Edge:
                    if (TriggerOnRisingEdge)
                    {
                        // Trigger on a rising edge crossing the trigger level
                        if (previousSample < TriggerLevelChannel2 && currentSample >= TriggerLevelChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }
                    }
                    else
                    {
                        // Trigger on a falling edge crossing the trigger level
                        if (previousSample > TriggerLevelChannel2 && currentSample <= TriggerLevelChannel2)
                        {
                            IsTriggeredChannel2 = true;
                        }
                    }
                    break;

                case TriggerType.Level:
                    if (currentSample >= TriggerLevelChannel2)
                    {
                        IsTriggeredChannel2 = true;
                    }
                    break;

                    // Additional logic for Pulse and Slope triggering can be added here
            }

            return IsTriggeredChannel2;
        }

        public void ResetTriggerChannel1()
        {
            IsTriggeredChannel1 = false;
            waitingForTriggerChannel1 = true;
        }

        public void ResetTriggerChannel2()
        {
            IsTriggeredChannel2 = false;
            waitingForTriggerChannel2 = true;
        }
    }
}
