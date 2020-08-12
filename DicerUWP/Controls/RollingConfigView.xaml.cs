using DicerWinUI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml.Controls;

namespace DicerUWP.Controls {
    public sealed partial class RollingConfigView : UserControl {
        public event EventHandler Changed;

        private readonly RollingViewModel _viewModel = new RollingViewModel();

        public RollingConfigView() {
            InitializeComponent();
            _viewModel.PropertyChanged += (_, e) => {
                if (e.PropertyName == "Expression")
                    Changed(this, null);
            };
        }

        public IEnumerable<int> Roll() => _viewModel.Roll();
        public string Expression => _viewModel.Expression;

        private class RollingViewModel : Bindable {
            private static readonly int[] Units = { 100, 20, 12, 10, 8, 6, 4, 3, 2, 1 };
            private readonly int[] _values = Units.Select(x => x == 20 ? 1 : 0).ToArray();

            private readonly Random _random = new Random();

            private void UpdateExpression() {
                var builder = new List<string>();
                for (var i = 0; i < Units.Length; ++i)
                    if (_values[i] != 0)
                        builder.Add($"{(_values[i] > 0 ? "+" : "")}{_values[i]}{UnitToString(Units[i])}");
                switch (builder.Count) {
                    case 0:
                        Expression = "0";
                        break;
                    case 1:
                        builder.First().Substring(1);
                        break;
                    default: {
                            var result = new StringBuilder();
                            foreach (var item in builder)
                                result.Append(item);
                            Expression = result.ToString().Substring(1);
                            break;
                        }
                }
                Notify(nameof(Expression));
            }

            public string Expression { get; private set; } = "1d20";

            public int D1 {
                get => _values[9];
                set {
                    if (SetProperty(ref _values[9], value))
                        UpdateExpression();
                }
            }

            public int D2 {
                get => _values[8];
                set {
                    if (SetProperty(ref _values[8], value))
                        UpdateExpression();
                }
            }

            public int D3 {
                get => _values[7];
                set {
                    if (SetProperty(ref _values[7], value))
                        UpdateExpression();
                }
            }

            public int D4 {
                get => _values[6];
                set {
                    if (SetProperty(ref _values[6], value))
                        UpdateExpression();
                }
            }

            public int D6 {
                get => _values[5];
                set {
                    if (SetProperty(ref _values[5], value))
                        UpdateExpression();
                }
            }

            public int D8 {
                get => _values[4];
                set {
                    if (SetProperty(ref _values[4], value))
                        UpdateExpression();
                }
            }

            public int D10 {
                get => _values[3];
                set {
                    if (SetProperty(ref _values[3], value))
                        UpdateExpression();
                }
            }

            public int D12 {
                get => _values[2];
                set {
                    if (SetProperty(ref _values[2], value))
                        UpdateExpression();
                }
            }

            public int D20 {
                get => _values[1];
                set {
                    if (SetProperty(ref _values[1], value))
                        UpdateExpression();
                }
            }

            public int D100 {
                get => _values[0];
                set {
                    if (SetProperty(ref _values[0], value))
                        UpdateExpression();
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

            private static string UnitToString(int value) => value == 1 ? "" : $"d{value}";
        }
    }
}
