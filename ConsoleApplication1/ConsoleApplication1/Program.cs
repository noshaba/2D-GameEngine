using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace ConsoleApplication1 {
    class Program {

        static ContextSettings context = new ContextSettings();
        static RenderWindow window;

        static void Main(string[] args) {
            Console.WriteLine("Hello World! :D");
            window = new RenderWindow(new VideoMode(1024,768),"SMFL Test!", Styles.Default, context);
            window.Closed += window_closed;
            while (window.IsOpen()) {
                window.Clear();
                window.Display();
            }
        }

        static void window_closed(object sender, EventArgs e) {
            window.Close();
        }
    }
}
