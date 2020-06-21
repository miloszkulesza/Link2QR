using Link2QR.Events;
using Link2QR.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using Unity;

namespace Link2QR.ViewModels
{
    public class FileNameDialogViewModel : BindableBase, IDialogAware
    {
        private readonly IEventAggregator eventAggregator;

        private string filename;
        public string Filename
        {
            get { return filename; }
            set 
            { 
                SetProperty(ref filename, value);
                UpdateButtonEnableState();
            }
        }

        private bool canCloseDialog;

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog
        {
            get { return canCloseDialog; }
            set { SetProperty(ref canCloseDialog, value); }
        }

        public DelegateCommand CloseDialogCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public FileNameDialogViewModel(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            CloseDialogCommand = new DelegateCommand(CloseDialog).ObservesCanExecute(() => CanCloseDialog);
            CancelCommand = new DelegateCommand(OnCancel);
        }

        private void UpdateButtonEnableState()
        {
            CanCloseDialog = !string.IsNullOrEmpty(Filename);
        }

        bool IDialogAware.CanCloseDialog() => CanCloseDialog;

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }

        private void CloseDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("filename", Filename);
            RequestClose(parameters);
        }

        private void OnCancel()
        {
            CanCloseDialog = true;
            RequestClose(null);
        }
    }
}
