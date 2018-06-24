using NAudio.Midi;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MidiInput
{
    public class MidiOutDeviceManager : PropertyChangedBase, IDisposable
    {
        private MidiOutDeviceManager()
        {
            Devices = new ObservableCollection<MidiOutViewModel>();

            for (var i = 0; i < MidiOut.NumberOfDevices; i++)
            {
                Devices.Add(new MidiOutViewModel(i));
            }
        }

        public MidiOutViewModel this[int i]
        {
            get { return Devices.SingleOrDefault(d => d.DeviceNumber == i); }
        }

        public static MidiOutDeviceManager Instance { get; } = new MidiOutDeviceManager();

        public ObservableCollection<MidiOutViewModel> Devices { get; }

        public MidiOutViewModel GetProductId(int productId)
        {
            return Devices.FirstOrDefault(d => d.ProductId == productId);
        }

        public MidiOutViewModel GetProductName(string productName)
        {
            return Devices.FirstOrDefault(d => d.ProductName == productName);
        }

        public void Dispose()
        {
            foreach (var device in Devices)
                device.Dispose();
        }
    }
}
