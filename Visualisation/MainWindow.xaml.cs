using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using SystemCore;

namespace Visualisation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CampingPitchesOverviewPage CampingPitchesOverviewFrame { get; set; }
        public MainWindow()
        {
            Query insertQuery = new Query("INSERT INTO Inventory VALUES (@id, @name, @quantity)");
            insertQuery.AddParameter("id", 5);
            insertQuery.AddParameter("name", "butter");
            insertQuery.AddParameter("quantity", 50);
            insertQuery.Execute();

            Query query = new Query("SELECT * FROM Inventory");
            var result = query.Select();

            Query queryFirst = new Query("SELECT * FROM Inventory WHERE id = @id");
            queryFirst.AddParameter("id", 3);
            var first = query.SelectFirst();

            Query updateQuery = new Query("UPDATE Inventory SET name = @name WHERE id = @id");
            updateQuery.AddParameter("name", "peanuts");
            updateQuery.AddParameter("id", 4);
            updateQuery.Execute();

            Query updateSecond = new Query("SELECT * FROM Inventory WHERE id = @id");
            updateSecond.AddParameter("id", 4);
            var second = updateSecond.SelectFirst();

            Query deleteQuery = new Query("DELETE FROM Inventory WHERE id = @id");
            deleteQuery.AddParameter("id", 5);
            deleteQuery.Execute();

            Query selectQuery = new Query("SELECT * FROM Inventory");
            var resultSecond = selectQuery.Select();

            this.InitializeComponent();
            this.CampingPitchesOverviewFrame = new CampingPitchesOverviewPage();
        }

        private void ReserveButtonClick(object sender, RoutedEventArgs e)
        {
            this.ReserveButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#006837");
            this.ReserveButton.Foreground = Brushes.White;
            this.MainFrame.Content = this.CampingPitchesOverviewFrame.Content;
        }

    }
}
