using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace xeen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private CCFileLoader fl;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);
                var ccFile = new byte[buffer.Length];
                buffer.CopyTo(ccFile);
                fl = new CCFileLoader();
                fl.LoadFile(ccFile);
            }
        }

        public class CCFileLoader
        {
            public CCFileLoader()
            {

            }

            private const int InitialShift = 0xac;
            private const int ShiftStep = 0x67;

            public void LoadFile(byte[] rawToc)
            {
                for (int i = 0; i < rawToc.Length; i++)
                {
                    var currentByte = rawToc[i];
                    rawToc[i] = DecryptByte(i, currentByte);
                }



                OnFileLoaded(EventArgs.Empty);
            }

            protected virtual void OnFileLoaded(EventArgs e)
            {
               if (FileLoaded != null)
                { 
                    FileLoaded(this, e);
                }
            }

            public event EventHandler FileLoaded;

            private static byte DecryptByte(int i, byte currentByte)
            {
                int ah = InitialShift + i * 8 * ShiftStep & 0xff;
                return (byte)(((currentByte << 2 | currentByte >> 6) + ah) & 0xff);
            }
        }
    }
}
