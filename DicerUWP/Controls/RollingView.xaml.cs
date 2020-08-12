using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DicerUWP.Controls {
    public sealed partial class RollingView : UserControl, INotifyPropertyChanged {
        public event EventHandler<IEnumerable<(string, int)>> Roll;
        public event EventHandler Delete;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; }
        public string Expression { get; private set; } = "";
        private bool AllowDeleteRolling => RollingFlip.Items.Count > 1;

        public RollingView(string title) {
            Title = title;
            InitializeComponent();
            AddRolling();
        }

        private void Roll_Click(object sender, RoutedEventArgs e) {
            Roll(this, RollEach());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
            => Delete(this, null);

        private void NewRolling_Click(object sender, RoutedEventArgs e) 
            => AddRolling();

        private void DeleteRolling_Click(object sender, RoutedEventArgs e) {
            RollingFlip.Items.RemoveAt(RollingFlip.SelectedIndex);
            Notify(nameof(AllowDeleteRolling));
            UpdateExpression();
        }

        private IEnumerable<(string, int)> RollEach() {
            foreach (var item in RollingFlip.Items) {
                if (!(item is RollingConfigView v)) continue;

                var builder = new StringBuilder();
                var list = v.Roll().ToArray();
                if (list.Length == 1)
                    yield return ($"{v.Expression} = {list[0]}", list[0]);
                else {
                    foreach (var num in list.SkipLast(1)) builder.Append($"+{num}");
                    var last = list.Last();
                    builder.Append(last > 0 ? $"+{last}" : $"{last}");
                    var sum = list.Sum();
                    yield return ($"{v.Expression} = {builder.ToString().Substring(1)} = {sum}", sum);
                }
            }
        }

        private void UpdateExpression() {
            var builder = new StringBuilder();
            foreach (var item in RollingFlip.Items)
                if (item is RollingConfigView v)
                    builder.Append($"/{v.Expression}");
            Expression = builder.ToString().Substring(1);
            Notify(nameof(Expression));
        }

        private void AddRolling() {
            var newRolling = new RollingConfigView();
            newRolling.Changed += (_, __) => UpdateExpression();
            RollingFlip.Items.Add(newRolling);
            RollingFlip.SelectedIndex = RollingFlip.Items.Count - 1;
            Notify(nameof(AllowDeleteRolling));
            UpdateExpression();
        }

        private void Notify(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
