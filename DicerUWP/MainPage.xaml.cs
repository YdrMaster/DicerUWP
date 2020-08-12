using DicerUWP.Controls;
using DicerWinUI.Common;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace DicerUWP {
    public sealed partial class MainPage : Page {
        private MainPageModel _viewModel = new MainPageModel();

        public MainPage() => InitializeComponent();

        private void RollingBuilderView_NewRollingChecked(object _, string e) {
            var rollingView = new RollingView(e);
            rollingView.Roll += (object sender, IEnumerable<(string, int)> result) => {
                if (!(sender is RollingView v)) return;
                var builder = new StringBuilder();
                foreach (var (text, num) in result)
                    builder.Append($"/{text}");
                _viewModel.Result = $"{v.Title} = {builder.ToString().Substring(1)}";
            };
            rollingView.Delete += (object rolling, System.EventArgs ___) => RollingList.Items.Remove(rolling);
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
