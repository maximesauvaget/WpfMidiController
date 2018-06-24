using System;
using System.Windows.Controls;

namespace MidiInput
{
    public class SliderLinkToControlChangeEventBehavior : LinkToControlChangeEventBehavior<Slider>
    {
        protected override bool SubscribeToMidiEvent()
        {
            if(base.SubscribeToMidiEvent())
            {
                var midiIn = MidiInDeviceManager.Instance[DeviceNumber];
                var midiOut = MidiOutDeviceManager.Instance.GetProductName(midiIn.ProductName);
                if(midiOut != null)
                {
                    var multiplier = (AssociatedObject.Maximum - AssociatedObject.Minimum) / 127;

                    midiOut.SendChangeControl(Controller, (int)(AssociatedObject.Value / multiplier), Channel);
                }

                return true;
            }

            return false;
        }

        protected override void SetValue(int value)
        {
            //0 <= value <= 127

            var multiplier = (AssociatedObject.Maximum - AssociatedObject.Minimum) / 127;
            var finalValue = multiplier * value;

            AssociatedObject.SetCurrentValue(Slider.ValueProperty, (double)finalValue);
        }
    }
}
