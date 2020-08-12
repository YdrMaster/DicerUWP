using DicerWinUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DicerUWP.Controls {
    public sealed partial class RollingView : UserControl {
        public event EventHandler<string> Play;
        public event EventHandler Delete;

        private readonly RollingViewModel _viewModel;

        public RollingView(string title) {
            _viewModel = new RollingViewModel(title);
            InitializeComponent();
        }

        private void Play_Click(object sender, RoutedEventArgs e) {
            var builder = new StringBuilder();
            var list = _viewModel.Roll().ToArray();
            if (list.Length == 1)
                Play(this, $"{_viewModel.Title} = {_viewModel.Expression} = {list[0]}");
            else {
                foreach (var num in list.SkipLast(1))
                    builder.Append($"+{num}");
                if (list.Last() > 0)
                    builder.Append($"+{list.Last()}");
                else
                    builder.Append(list.Last());
                Play(this, $"{_viewModel.Title} = {_viewModel.Expression} = {builder.ToString().Substring(1)} = {list.Sum()}");
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
            => Delete(this, null);

        private static string UnitToString(int value) => value == 1 ? "" : $"d{value}";

        private class RollingViewModel : Bindable {
            private static readonly int[] Units = { 100, 20, 12, 10, 8, 6, 4, 3, 2, 1 };

            private readonly Random _random = new Random();
            private readonly int[] _values = new int[Units.Length];

            public RollingViewModel(string title) {
                Title = title;
                _values[1] = 1;
            }

            public string Title { get; }

            public string Expression {
                get {
                    var builder = new List<string>();
                    for (var i = 0; i < Units.Length; ++i)
                        if (_values[i] != 0)
                            builder.Add($"{(_values[i] > 0 ? "+" : "")}{_values[i]}{UnitToString(Units[i])}");
                    switch (builder.Count) {
                        case 0:
                            return "0";
                        case 1:
                            return builder.First().Substring(1);
                        default: {
                                var result = new StringBuilder();
                                foreach (var item in builder)
                                    result.Append(item);
                                return result.ToString().Substring(1);
                            }
                    }
                }
            }

            public int D1 {
                get => _values[9];
                set {
                    if (SetProperty(ref _values[9], value))
                        Notify(nameof(Expression));
                }
            }

            public int D2 {
                get => _values[8];
                set {
                    if (SetProperty(ref _values[8], value))
                        Notify(nameof(Expression));
                }
            }

            public int D3 {
                get => _values[7];
                set {
                    if (SetProperty(ref _values[7], value))
                        Notify(nameof(Expression));
                }
            }

            public int D4 {
                get => _values[6];
                set {
                    if (SetProperty(ref _values[6], value))
                        Notify(nameof(Expression));
                }
            }

            public int D6 {
                get => _values[5];
                set {
                    if (SetProperty(ref _values[5], value))
                        Notify(nameof(Expression));
                }
            }

            public int D8 {
                get => _values[4];
                set {
                    if (SetProperty(ref _values[4], value))
                        Notify(nameof(Expression));
                }
            }

            public int D10 {
                get => _values[3];
                set {
                    if (SetProperty(ref _values[3], value))
                        Notify(nameof(Expression));
                }
            }

            public int D12 {
                get => _values[2];
                set {
                    if (SetProperty(ref _values[2], value))
                        Notify(nameof(Expression));
                }
            }

            public int D20 {
                get => _values[1];
                set {
                    if (SetProperty(ref _values[1], value))
                        Notify(nameof(Expression));
                }
            }

            public int D100 {
                get => _values[0];
                set {
                    if (SetProperty(ref _values[0], value))
                        Notify(nameof(Expression));
                }
            }

            public IEnumerable<int> Roll() {
                for (var i = 0; i < Units.Length - 1; ++i) {
                    if (_values[i] == 0) continue;
                    var sum = 0;
                    for (var j = 0; j < _values[i]; ++j)
                        sum += _random.Next(1, Units[i] + 1);
                    yield return sum;
                }
                if (_values.Last() != 0)
                    yield return _values.Last();
            }
        }
    }
}
