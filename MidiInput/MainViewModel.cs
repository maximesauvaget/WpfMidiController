using NAudio.Midi;
using System.Collections.ObjectModel;

namespace MidiInput
{
    public class MainViewModel : PropertyChangedBase, ISubscriberToMidiEvent<ControlChangeEvent>
    {
        private MidiInDeviceManager midiInManager;
        private MidiOutDeviceManager midiOutManager;
        private MidiInViewModel selectedDevice;

        public MainViewModel()
        {
            midiInManager = MidiInDeviceManager.Instance;
            midiOutManager = MidiOutDeviceManager.Instance;
        }

        public ObservableCollection<MidiInViewModel> DevicesIn => midiInManager.Devices;

        public ObservableCollection<MidiOutViewModel> DevicesOut => midiOutManager.Devices;

        public MidiInViewModel SelectedDevice
        {
            get => selectedDevice;
            set
            {
                selectedDevice = value;

                selectedDevice.Subscribe<ControlChangeEvent>(this);

                RaisePropertyChanged();
            }
        }

        public ControlChangeEvent ControlChangeEvent { get; set; }

        public void OnEventReceived(ControlChangeEvent midiEvent)
        {
            ControlChangeEvent = midiEvent;
            RaisePropertyChanged(nameof(ControlChangeEvent));
        }
        
    }
}
