using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace DrawPokerBasic
{
    public static class Utils
    {
        //
        // refer to msdn to get to work correctly: http://msdn.microsoft.com/en-us/library/microsoft.xna.framework.audio.soundeffect.fromstream.aspx
        //
        public static void Beep()
        {
            //using (var stream = TitleContainer.OpenStream("Sounds/beep-07.wav"))
            var MyStream = Application.GetResourceStream(new Uri("Sounds/beep-07.wav", UriKind.RelativeOrAbsolute));
            var effect = SoundEffect.FromStream(MyStream.Stream);
            FrameworkDispatcher.Update();
            effect.Play();
        }
    }
}
