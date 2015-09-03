using PlaylistConverter.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PlaylistConverter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string WPL = ".wpl";
        private const string M3U = ".m3u";

        private readonly IConversionService ConversionService;

        public MainPage()
            : this(new XmlConversionService())
        {

        }

        public MainPage(IConversionService conversionService)
        {
            ConversionService = conversionService;

            this.InitializeComponent();
            btnFind.Click += BtnFind_Click;
            btnConvert.Click += BtnConvert_Click;
            btnSaveResults.Click += BtnSaveResults_Click;

            btnAllInOne.Click += BtnAllInOne_Click;
        }

        private async void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            await FindAndReadFile();
        }

        private void BtnConvert_Click(object sender, RoutedEventArgs e)
        {
            ConvertFile();
        }

        private async void BtnSaveResults_Click(object sender, RoutedEventArgs e)
        {
            await SaveFile();
        }

        private async void BtnAllInOne_Click(object sender, RoutedEventArgs e)
        {
            await FindAndReadFile();
            ConvertFile();
            await SaveFile();
        }

        private async System.Threading.Tasks.Task FindAndReadFile()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(WPL);
            var file = await filePicker.PickSingleFileAsync();
            if (file != null)
            {
                tbxFileName.Text = file.Name;
                using (var fread = await file.OpenReadAsync())
                {
                    var fileSize = (uint)fread.Size;
                    var buffer = new Windows.Storage.Streams.Buffer(fileSize);
                    var results = await fread.ReadAsync(buffer, fileSize, Windows.Storage.Streams.InputStreamOptions.None);

                    var data = System.Text.Encoding.UTF8.GetString(results.ToArray());
                    tbxOriginalPlayList.Text = data;
                }
            }
        }

        private void ConvertFile()
        {
            var newData = ConversionService.Convert(tbxOriginalPlayList.Text);
            tbxNewPlayList.Text = newData;
        }

        private async System.Threading.Tasks.Task SaveFile()
        {
            var filePicker = new FileSavePicker();
            filePicker.FileTypeChoices.Add(M3U, (new string[] { M3U }).ToList());
            filePicker.DefaultFileExtension = M3U;
            filePicker.SuggestedFileName = tbxFileName.Text.Replace(WPL, M3U);
            var file = await filePicker.PickSaveFileAsync();
            if (file != null)
            {
                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    var bytes = new byte[tbxNewPlayList.Text.Length * sizeof(char)];
                    System.Buffer.BlockCopy(tbxNewPlayList.Text.ToCharArray(), 0, bytes, 0, bytes.Length);
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }

                tbxSavedDestination.Text = file.Path;
            }
        }
    }
}
