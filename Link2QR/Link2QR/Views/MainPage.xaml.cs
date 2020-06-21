using Link2QR.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Link2QR.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IEventAggregator eventAggregator;

        public MainPage(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            InitializeComponent();
            this.eventAggregator.GetEvent<UnfocusLinkEntryEvent>().Subscribe(UnfocusEntry);
        }

        public void UnfocusEntry()
        {
            linkEntry.Unfocus();
        }
    }
}