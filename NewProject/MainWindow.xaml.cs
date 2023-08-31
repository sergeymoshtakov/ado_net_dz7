using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Data.Data data;
        public ObservableCollection<Pair> Pairs { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            data = new Data.Data();
            Pairs = new ObservableCollection<Pair>();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DepartmentCountLabel.Content = data.Departments.Count().ToString();
            ManagerCountLabel.Content = data.Managers.Count().ToString();
            // predicate -> sql query
            TopChiefsCountLabel.Content = data.Managers.Where(manager => manager.IdChief == null).Count().ToString();
            SubordinatesCountLabel.Content = data.Managers.Where(manager => manager.IdChief != null).Count().ToString();
            ITDepartmentCountLabel.Content = data.Managers.Where(manager => manager.IdMainDep == new Guid("D3C376E4-BCE3-4D85-ABA4-E3CF49612C94")).Count().ToString();
            TwoDepartmentsCountLabel.Content = data.Managers.Where(manager => manager.IdMainDep != null && manager.IdSecDep != null).Count().ToString();


        }

        public class Pair
        {
            public String Key { get; set; } = null!;
            public String? Value { get; set; }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Pairs.Clear();
            var Query = data.Managers.Where(m => m.IdMainDep == Guid.Parse("131ef84b-f06e-494b-848f-bb4bc0604266")).Select(m => new Pair() {Key = m.Surname, Value = $"{m.Name[0]}. {m.Secname[0]}"});
            foreach(var pair in Query) 
            {
                Pairs.Add(pair); // zvernenya do bd
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Pairs.Clear();
            var Query = data.Managers.Join(data.Departments, m => m.IdMainDep, d => d.Id, (m, d) => new Pair() { Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.", Value = d.Name }).Skip(3).Take(10);
            foreach(var pair in Query)
            {
                Pairs.Add(pair);
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Pairs.Clear();
            var Query = data.Managers.Join(data.Managers, m1 => m1.IdChief, m2 => m2.Id, (m1, m2) => new Pair() { Key = $"{m1.Surname} {m1.Name[0]}. {m1.Secname[0]}.", Value = $"{m2.Surname} {m2.Name[0]}. {m2.Secname[0]}." }).Take(10).ToList().OrderBy(pair => pair.Key); // zapuskaet zapros i vydaet kolekciyu
            foreach(var pair in Query)
            {
                Pairs.Add(pair);
            }
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            Pairs.Clear();
            var Query = data.Managers.Join(data.Managers, m1 => m1.Id, m2 => m2.Id, (m1, m2) => new Pair() { Key = $"{m1.Surname} {m1.Name[0]}. {m1.Secname[0]}.", Value = m2.CreateDt.ToString() }).Take(7).ToList().OrderByDescending(pair => DateTime.Parse(pair.Value));
            foreach (var pair in Query)
            {
                Pairs.Add(pair);
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            Pairs.Clear();
            var Query = data.Managers.Join(data.Departments, m => m.IdSecDep, d => d.Id, (m, d) => new Pair() { Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.", Value = $"{d.Name}" }).Take(10).ToList().OrderBy(pair => pair.Value);
            foreach (var pair in Query)
            {
                Pairs.Add(pair);
            }
        }
    }
}
