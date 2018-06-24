using NAudio.Midi;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MidiInput
{
    public class MidiInDeviceManager : PropertyChangedBase, IDisposable
    {
        private MidiInDeviceManager()
        {
            Devices = new ObservableCollection<MidiInViewModel>();

            for (var i = 0; i < MidiIn.NumberOfDevices; i++)
            {
                Devices.Add(new MidiInViewModel(i));
            }
        }

        public MidiInViewModel this[int i]
        {
            get { return Devices.SingleOrDefault(d => d.DeviceNumber == i); }
        }

        public static MidiInDeviceManager Instance { get; } = new MidiInDeviceManager();

        public ObservableCollection<MidiInViewModel> Devices { get; }

        public void Dispose()
        {
            foreach (var device in Devices)
                device.Dispose();
        }
    }
}
