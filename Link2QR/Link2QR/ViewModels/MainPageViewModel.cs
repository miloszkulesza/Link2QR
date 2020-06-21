using Link2QR.Events;
using Link2QR.Services;
using Link2QR.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using QRCoder;
using System;
using System.IO;
using Unity;
using Xamarin.Forms;

namespace Link2QR.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;

        private bool codeVisibility;
        public bool CodeVisibility
        {
            get { return codeVisibility; }
            set { SetProperty(ref codeVisibility, value); }
        }

        private bool canCodeLink;
        public bool CanCodeLink
        {
            get { return canCodeLink; }
            set { SetProperty(ref canCodeLink, value); }
        }

        private string link;
        public string Link
        {
            get { return link; }
            set
            {
                SetProperty(ref link, value);
                UpdateCodeButtonEnableState();
            }
        }

        private Image barcode;
        public Image Barcode
        {
            get { return barcode; }
            set { SetProperty(ref barcode, value); }
        }

        public byte[] BarcodeBytes { get; set; }

        public DelegateCommand CodeLinkCommand { get; set; }
        public DelegateCommand NewLinkCommand { get; set; }
        public DelegateCommand SaveImageCommand { get; set; }

        public MainPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IEventAggregator eventAggregator) : base(navigationService)
        {
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;
            RegisterCommands();
            Title = "Code link to QR";
            CodeVisibility = false;
        }

        private void RegisterCommands()
        {
            CodeLinkCommand = new DelegateCommand(OnCodeLink).ObservesCanExecute(() => CanCodeLink);
            NewLinkCommand = new DelegateCommand(OnNewLink);
            SaveImageCommand = new DelegateCommand(OnSaveImage);
        }

        private void OnCodeLink()
        {
            Barcode = new Image();
            BarcodeBytes = GetQrImageAsBytes(Link);
            Barcode.Source = ImageSource.FromStream(() => new MemoryStream(BarcodeBytes)) as StreamImageSource;
            CodeVisibility = true;
        }

        private void OnNewLink()
        {
            CodeVisibility = false;
            Link = string.Empty;
            eventAggregator.GetEvent<UnfocusLinkEntryEvent>().Publish();
        }

        private void UpdateCodeButtonEnableState()
        {
            CanCodeLink = !string.IsNullOrEmpty(Link);
        }

        private byte[] GetQrImageAsBytes(string text)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.L);
            var qRCode = new PngByteQRCode(qrCodeData);
            var qrCodeBytes = qRCode.GetGraphic(20);
            return qrCodeBytes;
        }

        private void OnSaveImage()
        {
            dialogService.ShowDialog("FileNameDialog", SaveImage);
        }

        private async void SaveImage(IDialogResult result)
        {
            string filename = result.Parameters.GetValue<string>("filename");
            if (filename != null)
            {
                var saveFileService = Xamarin.Forms.DependencyService.Get<ISaveFile>();
                var snackbarService = Xamarin.Forms.DependencyService.Get<ISnackbarService>();
                bool saveResult = false;
                try
                {
                    saveResult = await saveFileService.SaveFile(BarcodeBytes, filename);
                }
                catch(Exception e)
                {
                    snackbarService.ShowSnackbar(e.Message);
                }
                if (saveResult)
                {
                    snackbarService.ShowSnackbar("Image successfully saved to phone storage");
                    NewLinkCommand.Execute();
                }
                else
                    snackbarService.ShowSnackbar("Image save failed");
            }
        }
    }
}
