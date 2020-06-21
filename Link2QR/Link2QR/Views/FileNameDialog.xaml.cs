using Link2QR.Events;
using Prism.Events;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Link2QR.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FileNameDialog
    {
        private readonly IEventAggregator eventAggregator;

        public FileNameDialog(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            InitializeComponent();
            this.eventAggregator.GetEvent<FocusFilenameEntryEvent>().Subscribe(FocusFilename);
            FocusFilename();
        }

        private void FocusFilename()
        {
            filename.Focus();
        }
    }
}