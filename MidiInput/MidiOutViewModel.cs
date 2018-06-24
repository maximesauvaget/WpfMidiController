using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidiInput
{
    public class MidiOutViewModel : PropertyChangedBase, IDisposable
    {
        private readonly int deviceNo;
        private readonly MidiOut device;
        private readonly MidiOutCapabilities capabilities;

        public MidiOutViewModel(int deviceNo)
        {
            this.deviceNo = deviceNo;
            device = new MidiOut(deviceNo);
            capabilities = MidiOut.DeviceInfo(deviceNo);
        }

        public int DeviceNumber => deviceNo;

        public string ProductName => capabilities.ProductName;

        public int ProductId => capabilities.ProductId;

        public string Manufacturer => capabilities.Manufacturer.ToString();

        public void SendChangeControl(int controller, int value, int channel)
        {
            device.Send(MidiMessage.ChangeControl(controller, value, channel).RawData);
        }

        public void Dispose()
        {
            device?.Dispose();
        }
    }
}