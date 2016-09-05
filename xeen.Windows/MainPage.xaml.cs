using System;
using System.Diagnostics.Contracts;
using System.IO;
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
            picker.FileTypeFilter.Add("*");
            var file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                var buffer = await Windows.Storage.FileIO.ReadBufferAsync(file);
                fl = new CCFileLoader();
                fl.LoadFile(buffer.AsStream());
                fl.FileLoaded += OnFileLoaded;
            }
        }

        protected async void OnFileLoaded(object obj, EventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add("*");
            await picker.PickSingleFileAsync();
        }

        public class CCFileLoader
        {
            public CCFileLoader()
            {
                
            }

            private const int InitialShift = 0xac;
            private const int ShiftStep = 0x67;

            public void LoadFile(Stream stream)
            {
                BinaryReader reader = new BinaryReader(stream);
                int fileCount = reader.ReadInt16(); 

                for (int i = 0; i < fileCount; i++)
                {
                    //reader.ReadByte()
                    byte lastByte = 0;
                    Contract.Requires(lastByte == 0);

                }
                FileLoaded?.Invoke(this, EventArgs.Empty);
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
