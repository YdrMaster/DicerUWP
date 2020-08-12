using DicerUWP.Controls;
using DicerWinUI.Common;
using Windows.UI.Xaml.Controls;

namespace DicerUWP {
    public sealed partial class MainPage : Page {
        private MainPageModel _viewModel = new MainPageModel();

        public MainPage() => InitializeComponent();

        private void RollingBuilderView_NewRollingChecked(object sender, string e) {
            var rollingView = new RollingView(e);
            rollingView.Play += (object _, string title) => _viewModel.Result = title;
            rollingView.Delete += (object rolling, System.EventArgs _) => RollingList.Items.Remove(rolling);
            RollingList.Items.Insert(RollingList.Items.Count - 1, rollingView);
        }

        private class MainPageModel : Bindable {
            private string _result = "0";

            public string Result {
                get => _result;
                set => SetProperty(ref _result, value);
            }
        }
    }
}
