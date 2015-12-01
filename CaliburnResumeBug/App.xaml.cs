using Caliburn.Micro;
using CaliburnResumeBug.Messages;
using CaliburnResumeBug.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CaliburnResumeBug
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : CaliburnApplication
    {
        private WinRTContainer _container;
        private IEventAggregator _eventAggregator;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void Configure()
        {
            LogManager.GetLog = type => new DebugLog(type);
            _container = new WinRTContainer();
            _container.RegisterWinRTServices();

            _container
                .PerRequest<ShellViewModel>()
                .PerRequest<TestViewModel>();
            _eventAggregator = _container.GetInstance<IEventAggregator>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            DisplayRootViewFor<ShellViewModel>();

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                _eventAggregator.PublishOnUIThread(new ResumeStateMessage());
        }

        protected override void OnSuspending(object sender, SuspendingEventArgs e)
        {
            _eventAggregator.PublishOnUIThread(new SuspendStateMessage(e.SuspendingOperation));
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}
