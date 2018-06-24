using NAudio.Midi;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace MidiInput
{
    public abstract class LinkToControlChangeEventBehavior<T> : Behavior<T>, ISubscriberToMidiEvent<ControlChangeEvent> 
        where T : Control
    {
        private bool IsInitialized;
        
        public int DeviceNumber
        {
            get { return (int)GetValue(DeviceNumberProperty); }
            set { SetValue(DeviceNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeviceNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeviceNumberProperty =
            DependencyProperty.Register("DeviceNumber", typeof(int), typeof(LinkToControlChangeEventBehavior<T>), new PropertyMetadata(-1));
                        
        public int Channel
        {
            get { return (int)GetValue(ChannelProperty); }
            set { SetValue(ChannelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Channel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ChannelProperty =
            DependencyProperty.Register("Channel", typeof(int), typeof(LinkToControlChangeEventBehavior<T>), new PropertyMetadata(-1));

        public int Controller
        {
            get { return (int)GetValue(ControllerProperty); }
            set { SetValue(ControllerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Controller.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ControllerProperty =
            DependencyProperty.Register("Controller", typeof(int), typeof(LinkToControlChangeEventBehavior<T>), new PropertyMetadata(-1));
        
        public int MidiValue
        {
            get { return (int)GetValue(MidiValueProperty); }
            set { SetValue(MidiValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MidiValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MidiValueProperty =
            DependencyProperty.Register("MidiValue", typeof(int), typeof(LinkToControlChangeEventBehavior<T>), new PropertyMetadata(-1, OnMidiPropertyChanged));

        private static void OnMidiPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((LinkToControlChangeEventBehavior<T>)d).SetValue((int)e.NewValue);
        }

        protected override void OnAttached()
        {
            SubscribeToMidiEvent();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            SubscribeToMidiEvent();
        }

        protected virtual bool SubscribeToMidiEvent()
        {
            if (IsInitialized)
                return false;

            if(DeviceNumber > -1 && Channel > -1)
            {
                MidiInDeviceManager.Instance[DeviceNumber].Subscribe(this);
                IsInitialized = true;

                return true;
            }

            return false;
        }

        private void OnControlChangeEvent(ControlChangeEvent obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (obj.Channel == (int)ReadLocalValue(ChannelProperty)
                    && (byte)obj.Controller == (int)ReadLocalValue(ControllerProperty))
                {
                    SetValue(obj.ControllerValue);
                }
            });
        }

        protected abstract void SetValue(int value);

        public void OnEventReceived(ControlChangeEvent midiEvent)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (midiEvent.Channel == (int)ReadLocalValue(ChannelProperty)
                    && (byte)midiEvent.Controller == (int)ReadLocalValue(ControllerProperty))
                {
                    SetValue(midiEvent.ControllerValue);
                }
            });
        }
    }
}
