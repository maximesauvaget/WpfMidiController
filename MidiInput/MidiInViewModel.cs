using NAudio.Midi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MidiInput
{
    public interface ISubscriberToMidiEvent { }

    public interface ISubscriberToMidiEvent<T> : ISubscriberToMidiEvent where T : MidiEvent
    {
        void OnEventReceived(T midiEvent);
    }

    public class MidiInViewModel : PropertyChangedBase, IDisposable
    {
        private readonly int deviceNo;
        private readonly MidiIn device;
        private readonly MidiInCapabilities capabilities;
        
        private List<Subscriber> subscribers;

        public MidiInViewModel(int deviceNo)
        {
            this.deviceNo = deviceNo;
            device = new MidiIn(deviceNo);
            capabilities = MidiIn.DeviceInfo(deviceNo);

            subscribers = new List<Subscriber>();

            device.MessageReceived += Device_MessageReceived;
            device.ErrorReceived += Device_ErrorReceived;
            device.Start();
        }

        public int DeviceNumber => deviceNo;

        public string ProductName => capabilities.ProductName;

        public int ProductId => capabilities.ProductId;

        public string Manufacturer => capabilities.Manufacturer.ToString();

        public void Unsubscribe<T>(ISubscriberToMidiEvent<T> subscriber)
            where T : MidiEvent
        {
            var subscription = subscribers.SingleOrDefault(s => s.subscriber == subscriber);
            
            subscribers.Remove(subscription);
        }

        public void Subscribe<T>(ISubscriberToMidiEvent<T> subscriber)
            where T : MidiEvent
        {
            subscribers.Add(new Subscriber<T>(subscriber));
        }

        private void Device_ErrorReceived(object sender, MidiInMessageEventArgs e)
        {
            ;
        }

        private void Device_MessageReceived(object sender, MidiInMessageEventArgs e)
        {
            foreach(var subscriber in subscribers.Where(s => s.EventType == e.MidiEvent.GetType()))
            {
                subscriber.OnEventReceived(e.MidiEvent);
            }
        }

        public void Dispose()
        {
            subscribers.Clear();
            device?.Dispose();
        }

        private abstract class Subscriber
        {
            public readonly ISubscriberToMidiEvent subscriber;

            public Subscriber(ISubscriberToMidiEvent subscriber)
            {
                this.subscriber = subscriber;
            }

            public Type EventType { get; set; }

            public abstract void OnEventReceived(MidiEvent midiEvent);
        }

        private class Subscriber<T> : Subscriber where T : MidiEvent
        {
            public new readonly ISubscriberToMidiEvent<T> subscriber;

            public Subscriber(ISubscriberToMidiEvent<T> subscriber) : base(subscriber)
            {
                EventType = typeof(T);
                this.subscriber = subscriber;
            }

            public override void OnEventReceived(MidiEvent midiEvent)
            {
                subscriber.OnEventReceived((T)midiEvent);
            }
        }
    }
}
