using Caliburn.Micro;
using CaliburnResumeBug.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CaliburnResumeBug.ViewModels
{
    public class ShellViewModel : Screen, IHandle<ResumeStateMessage>, IHandle<SuspendStateMessage>
    {
        private bool resume;
        private readonly WinRTContainer container;
        private readonly IEventAggregator eventAggregator;
        private INavigationService navigationService;

        public ShellViewModel(WinRTContainer container, IEventAggregator eventAggregator)
        {
            this.container = container;
            this.eventAggregator = eventAggregator;
        }

        protected override void OnActivate()
        {
            eventAggregator.Subscribe(this);
        }

        protected override void OnDeactivate(bool close)
        {
            eventAggregator.Unsubscribe(this);
        }

        public void SetupNavigationService(Frame frame)
        {
            navigationService = container.RegisterNavigationService(frame);

            if (resume)
                navigationService.ResumeState();
            else
                navigationService.For<TestViewModel>().Navigate();
        }

        public void Handle(SuspendStateMessage message)
        {
            navigationService.SuspendState();
        }

        public void Handle(ResumeStateMessage message)
        {
            resume = true;
        }
    }
}
