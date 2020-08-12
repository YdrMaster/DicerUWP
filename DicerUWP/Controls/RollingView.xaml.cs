using DicerWinUI.Common;
using System;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DicerUWP.Controls {
    public sealed partial class RollingView : UserControl {
        public event EventHandler<string> Play;
        public event EventHandler Delete;

        private readonly RollingViewModel _viewModel = new RollingViewModel();
        private readonly string _title;

        public RollingView(string title) {
            _title = title; 
            InitializeComponent();
            _viewModel.Expression = RollingConfig.Expression;
        }

        private void Play_Click(object sender, RoutedEventArgs e) {
            var builder = new StringBuilder();
            var list = RollingConfig.Roll().ToArray();
            if (list.Length == 1)
                Play(this, $"{_title} = {_viewModel.Expression} = {list[0]}");
            else {
                foreach (var num in list.SkipLast(1))
                    builder.Append($"+{num}");
                if (list.Last() > 0)
                    builder.Append($"+{list.Last()}");
                else
                    builder.Append(list.Last());
                Play(this, $"{_title} = {_viewModel.Expression} = {builder.ToString().Substring(1)} = {list.Sum()}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
            => Delete(this, null);

        private void RollingChanged(object sender, EventArgs e)
            => _viewModel.Expression = RollingConfig.Expression;

        private class RollingViewModel : Bindable {
            private string _expression;

            public string Expression {
                get => _expression;
                set => SetProperty(ref _expression, value);
            }
        }
    }
}
