using DicerWinUI.Common;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DicerUWP.Controls {
    public sealed partial class RollingBuilderView : UserControl {
        public event EventHandler<string> NewRollingChecked;
        private readonly RollingViewModel _viewModel = new RollingViewModel();

        public RollingBuilderView() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs _) {
            if (string.IsNullOrWhiteSpace(_viewModel.Title)) return;
            NewRollingChecked.Invoke(this, _viewModel.Title.Trim());
            _viewModel.Title = "";
        }

        private class RollingViewModel : Bindable {
            private string _title = "";

            public string Title {
                get => _title;
                set => SetProperty(ref _title, value);
            }
        }
    }
}
