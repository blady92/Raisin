using System;

namespace Cyber2O
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (AudioTest game = new AudioTest())
            {
                game.Run();
            }
        }
    }
#endif
}

