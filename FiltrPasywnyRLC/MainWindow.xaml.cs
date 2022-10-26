using System;
using System.IO;
using System.Collections.Generic;
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
//---
using Microsoft.Win32;
using System.Windows.Forms.DataVisualization.Charting;
using System.Numerics;
using System.Data;

namespace FiltrPasywnyRLC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Chart chart01;
        //---
        private double U1;      //Amplituda napięcia wejściowego filtru
        private double fmin;    //Częstotliwość minimalna napięcia zasilającego
        private double fmax;    //Częstotliwość maksymalna napięcia zasilającego
        private double R1;      //Rezystancja
        private double R2;      //Rezystancja
        private double L1;      //Indukcyjność
        private double C1;      //Pojemność
        private double[,] Results;  //Tablica wyników transmitancji
        private int size;       //Liczba wierszy w tablicy Results
        //---Deklaracja menu kontekstowego w pliku logiki programowalnej
        private ContextMenu circuitContextMenu = null;
        //---
        public MainWindow()
        {
            InitializeComponent();
            this.U1 = 10;
            //this.u2 = 0;
            this.fmin = 0;
            this.fmax = 1000;
            this.R1 = 0.5;
            this.R2 = 0.5;
            this.L1 = 0.001;
            this.C1 = 0.0005;
            this.size = 2000;
            //---
            this.txtMagnitude.Text = U1.ToString();
            this.txtFreqMin.Text = fmin.ToString();
            this.txtFreqMax.Text = fmax.ToString();
            this.txtResistance.Text = R1.ToString();
            this.txtResistance2.Text = R2.ToString();
            this.txtInductance.Text = L1.ToString();
            this.txtCapacitance.Text = C1.ToString();

            //---Definiowanie opcji menu kontekstowego
            MenuItem paramMenuItem = new MenuItem();
            paramMenuItem.Header = "Parameters...";
            paramMenuItem.Click += new RoutedEventHandler(filterParams_Click);

            MenuItem clearMenuItem = new MenuItem();
            clearMenuItem.Header = "Clear Waveform";
            clearMenuItem.Click += new RoutedEventHandler(clearWaveforms_Click);
            
            //Inicjalzacja menu kontekstowego
            circuitContextMenu = new ContextMenu();
            circuitContextMenu.Items.Add(paramMenuItem);
            circuitContextMenu.Items.Add(clearMenuItem);
            //---
            this.circuitImage.ContextMenu = circuitContextMenu;
        }
        //---

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //---
        private void filterParams_Click(object sender, RoutedEventArgs e)
        {
            ParamWindow paramDialog = new ParamWindow();
            //Ustawienie wartosci txtPoints.Text w oknie dialogowym
            paramDialog.txtPoints.Text = size.ToString();
            //Ustawienie wartosci rezystancji w oknie dialogowym
            paramDialog.txtResistance.Text = this.txtResistance.Text;
            paramDialog.txtResistance2.Text = this.txtResistance2.Text;
            paramDialog.txtCapacitance.Text = this.txtCapacitance.Text;
            paramDialog.txtInductance.Text = this.txtInductance.Text;
            paramDialog.txtMagnitude.Text = this.txtMagnitude.Text;
            paramDialog.txtFreqMin.Text = this.txtFreqMin.Text;
            paramDialog.txtFreqMax.Text = this.txtFreqMax.Text;

            //Otwieramy okno dialogowe - metoda ShowDialog()
            bool? dialogResult = paramDialog.ShowDialog();
            //Sprawdzamy który przycisk okna dialogowego zamnknął to okno
            //Jesli to był button OK to dialogResult jest true
            //Jesli to był button Cancel dialogResult jest na false
            if(dialogResult==true)
            {
                if (!Int32.TryParse(paramDialog.txtPoints.Text, out size))
                {
                    MessageBox.Show("Błędna wartość rozmiaru tablicy", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!Double.TryParse(paramDialog.txtResistance.Text, out R1))
                {
                    MessageBox.Show("Błędny format rezystancji R1", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtResistance.Text = R1.ToString();

                if (!Double.TryParse(paramDialog.txtResistance2.Text, out R2))
                {
                    MessageBox.Show("Błędny format rezystancji R2", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtResistance2.Text = R2.ToString();

                if (!Double.TryParse(paramDialog.txtCapacitance.Text, out C1))
                {
                    MessageBox.Show("Błędny format pojemnosci C", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtCapacitance.Text = C1.ToString();

                if (!Double.TryParse(paramDialog.txtInductance.Text, out L1))
                {
                    MessageBox.Show("Błędny format indukcyjności L", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtInductance.Text = L1.ToString();

                if (!Double.TryParse(paramDialog.txtMagnitude.Text, out U1))
                {
                    MessageBox.Show("Błędny format amplitudy napięcia U", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtMagnitude.Text = U1.ToString();

                if (!Double.TryParse(paramDialog.txtFreqMin.Text, out fmin))
                {
                    MessageBox.Show("Błędny format czestotliwosci fmin", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtFreqMin.Text = fmin.ToString();

                if (!Double.TryParse(paramDialog.txtFreqMax.Text, out fmax))
                {
                    MessageBox.Show("Błędny format czestotliwosci fmax", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.txtFreqMax.Text = fmax.ToString();
            }
            else
            {
                MessageBox.Show("Do zobaczenia", "Parametry", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        //---
        private void about_Click(object sender, RoutedEventArgs e)
        {
            About aboutWindow = new About();
            aboutWindow.ShowDialog();
        }
        //---
        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (!Double.TryParse(txtMagnitude.Text, out U1))
            {
                MessageBox.Show("Błedna wartość amplitudy", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtFreqMin.Text, out fmin))
            {
                MessageBox.Show("Błedna wartość częstotliwości min.", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtFreqMax.Text, out fmax))
            {
                MessageBox.Show("Błedna wartość częstotliwości max.", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtResistance.Text, out R1))
            {
                MessageBox.Show("Błedna wartość rezystancji R1", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtResistance.Text, out R2))
            {
                MessageBox.Show("Błedna wartość rezystancji R2", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtInductance.Text, out L1))
            {
                MessageBox.Show("Błedna wartość indukcyjności L", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Double.TryParse(txtCapacitance.Text, out C1))
            {
                MessageBox.Show("Błedna wartość pojemności C", "Parametry",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //---
            Transmittance();
            DrawWaveforms();
        }
        //---
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            chart01 = new Chart();
            host.Child = chart01;

            //Making 3 drawing areas
            chart01.ChartAreas.Add(new ChartArea("Magnitude"));
            chart01.ChartAreas.Add(new ChartArea("Phase"));
            chart01.ChartAreas.Add(new ChartArea("Current_Magnitude"));

            //Getting the position of mouse
            //Automatic interruption after getting += and then TAB
            chart01.MouseDown += Chart01_MouseDown;
            chart01.MouseMove += Chart01_MouseMove;
        }
        private void Chart01_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
            System.Drawing.PointF mousePt = new System.Drawing.PointF(e.X, e.Y);
            chart01.ChartAreas["Magnitude"].CursorX.SetCursorPixelPosition(mousePt, true);
            chart01.ChartAreas["Magnitude"].CursorY.SetCursorPixelPosition(mousePt, true);

            //Copy
            chart01.ChartAreas["Phase"].CursorX.SetCursorPixelPosition(mousePt, true);
            chart01.ChartAreas["Phase"].CursorY.SetCursorPixelPosition(mousePt, true);

            //Copy
            chart01.ChartAreas["Current_Magnitude"].CursorX.SetCursorPixelPosition(mousePt, true);
            chart01.ChartAreas["Current_Magnitude"].CursorY.SetCursorPixelPosition(mousePt, true);
        }

        private void Chart01_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //throw new NotImplementedException();
            double ptx = chart01.ChartAreas["Magnitude"].AxisX.PixelPositionToValue(e.X);
            double pty = chart01.ChartAreas["Magnitude"].AxisY.PixelPositionToValue(e.Y);

            double ptxi = chart01.ChartAreas["Phase"].AxisX.PixelPositionToValue(e.X);
            double ptyi = chart01.ChartAreas["Phase"].AxisY.PixelPositionToValue(e.Y);

            double ptxj = chart01.ChartAreas["Current_Magnitude"].AxisX.PixelPositionToValue(e.X);
            double ptyj = chart01.ChartAreas["Current_Magnitude"].AxisY.PixelPositionToValue(e.Y);

            // dodać wrunek by tylko 1 współrzędne były
            //if (pty<0) txtStatus.Text = "X=" + ptxi.ToString("#.0") + ";  " + "Y=" + ptyi.ToString("#.0");
            //if (pty>0) txtStatus.Text = "X=" + ptx.ToString("#.0") + ";  " + "Y=" + pty.ToString("#.0");

            txtStatus.Text = "For Magnitude: X=" + ptx.ToString("#.0") + ";" + "Y=" + pty.ToString("#.0") + "  For Phase X=" + ptxi.ToString("#.0") + ";" + "Y=" + ptyi.ToString("#.0") 
                + "  For Current Magnitude X=" + ptxj.ToString("#.0") + ";" + "Y=" + ptyj.ToString("#.0");
        }
        //---
        private void Transmittance()
        {
            this.Results = new double[size, 5];
            Complex Z1; //Licznik transmitancji
            Complex Z2; //Mianownik transmitancji
            Complex Z3; //Transmitancja zespolona Z1/Z2
            //Current
            Complex Z4; 
            Complex Z5; 
            Complex Z6; 
            double f = fmin;
            double df = (fmax - fmin) / (size - 1);
            double omega = 0;   //Częstość kątowa
            //---
            for (int i = 0; i < size; i++)
            {
                omega = 2 * Math.PI * f;
                Z1 = new Complex(0, omega*L1);
                Z2 = new Complex(R1 + R2 - omega * omega * R2 * L1 * C1, omega * (L1 + C1 * R2 * R1));
                Z3 = Z1 / Z2;
                //Current 
                Z4 = new Complex(R1 + R2 - omega * omega * R2 * L1 * C1, omega * (L1 + R1 * R2 * C1));
                Z5 = new Complex(1 - omega * omega * L1 * C1, omega * C1 * R1);
                Z6 = U1 * Z5 / Z4;

                Results[i, 0] = f;
                Results[i, 1] = Z3.Magnitude;
                Results[i, 2] = Z3.Phase;
                Results[i, 3] = Z6.Magnitude;
                Results[i, 4] = -Z6.Magnitude;
                f += df;
            }
        }
        //------
        public void DrawWaveforms()
        {
            DataTable dTable;   //Reprezentacja danych z Results
            DataView dView;     //Reprezentacja DataTable na Chart
            dTable = new DataTable();
            DataColumn column;
            DataRow row;
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Frequency";
            dTable.Columns.Add(column);
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Transmittance";
            dTable.Columns.Add(column);
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "PhaseSpectrum";
            dTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Envelope+";
            dTable.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Envelope-";
            dTable.Columns.Add(column);

            //---
            for (int i = 0; i < size; i++)
            {
                row = dTable.NewRow();
                row["Frequency"] = Results[i, 0];
                row["Transmittance"] = Results[i, 1];
                row["PhaseSpectrum"] = Results[i, 2];
                row["Envelope+"] = Results[i, 3];
                row["Envelope-"] = Results[i, 4];
                dTable.Rows.Add(row);
            }
            //---
            dView = new DataView(dTable);
            //---
            chart01.Series.Clear();
            chart01.Titles.Clear();
            
            chart01.ChartAreas["Magnitude"].AxisY.Title = "Amplitude spectrum";
            chart01.ChartAreas["Phase"].AxisY.Title = "Phase spectrum";
            chart01.ChartAreas["Current_Magnitude"].AxisY.Title = "Current Magnitude";
            //---
            chart01.DataBindTable(dView, "Frequency");
            //---
            chart01.Series["Transmittance"].ChartType = SeriesChartType.Line;
            chart01.Series["PhaseSpectrum"].ChartType = SeriesChartType.Line;
            chart01.Series["Envelope+"].ChartType = SeriesChartType.Line;
            chart01.Series["Envelope-"].ChartType = SeriesChartType.Line;
            //---
            chart01.Series["Transmittance"].ChartArea = "Magnitude";
            chart01.Series["PhaseSpectrum"].ChartArea = "Phase";
            chart01.Series["Envelope+"].ChartArea = "Current_Magnitude";
            chart01.Series["Envelope-"].ChartArea = "Current_Magnitude";
            //---
            chart01.Titles.Add("Filter Transmittance: U2/U1");
            //
            chart01.ChartAreas[0].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[0].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[0].AxisX.Minimum = 0;

            chart01.ChartAreas[1].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[1].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[1].AxisX.Minimum = 0;

            chart01.ChartAreas[2].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[2].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[2].AxisX.Minimum = 0;

            
            // Nowy fragment 
            // Cursory i przybliżenia
            chart01.ChartAreas["Magnitude"].CursorX.IsUserEnabled = true;
            chart01.ChartAreas["Magnitude"].CursorY.IsUserEnabled = true;
            chart01.ChartAreas["Magnitude"].CursorX.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Magnitude"].CursorY.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Magnitude"].CursorX.Interval = 1;
            chart01.ChartAreas["Magnitude"].CursorY.Interval = 0.1;
            chart01.ChartAreas["Magnitude"].CursorX.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Magnitude"].CursorY.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Magnitude"].CursorX.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Magnitude"].CursorY.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Magnitude"].AxisX.ScaleView.Zoomable = true;
            chart01.ChartAreas["Magnitude"].AxisY.ScaleView.Zoomable = true;
            chart01.ChartAreas["Magnitude"].AxisX.Interval = 100;
            chart01.ChartAreas["Magnitude"].AxisY.Interval = 0.5;
            chart01.ChartAreas["Magnitude"].AxisY.ScaleView.SmallScrollMinSize = 0.1;

            chart01.ChartAreas["Phase"].CursorX.IsUserEnabled = true;
            chart01.ChartAreas["Phase"].CursorY.IsUserEnabled = true;
            chart01.ChartAreas["Phase"].CursorX.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Phase"].CursorY.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Phase"].CursorX.Interval = 1;
            chart01.ChartAreas["Phase"].CursorY.Interval = 0.1;
            chart01.ChartAreas["Phase"].CursorX.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Phase"].CursorY.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Phase"].CursorX.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Phase"].CursorY.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Phase"].AxisX.ScaleView.Zoomable = true;
            chart01.ChartAreas["Phase"].AxisY.ScaleView.Zoomable = true;
            chart01.ChartAreas["Phase"].AxisX.Interval = 100;
            chart01.ChartAreas["Phase"].AxisY.Interval = 1;
            chart01.ChartAreas["Phase"].AxisY.ScaleView.SmallScrollMinSize = 0.1;

            chart01.ChartAreas["Current_Magnitude"].CursorX.IsUserEnabled = true;
            chart01.ChartAreas["Current_Magnitude"].CursorY.IsUserEnabled = true;
            chart01.ChartAreas["Current_Magnitude"].CursorX.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Current_Magnitude"].CursorY.IsUserSelectionEnabled = true;
            chart01.ChartAreas["Current_Magnitude"].CursorX.Interval = 1;
            chart01.ChartAreas["Current_Magnitude"].CursorY.Interval = 1;
            chart01.ChartAreas["Current_Magnitude"].CursorX.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Current_Magnitude"].CursorY.LineColor = System.Drawing.Color.BlueViolet;
            chart01.ChartAreas["Current_Magnitude"].CursorX.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Current_Magnitude"].CursorY.LineDashStyle = ChartDashStyle.Dash;
            chart01.ChartAreas["Current_Magnitude"].AxisX.ScaleView.Zoomable = true;
            chart01.ChartAreas["Current_Magnitude"].AxisY.ScaleView.Zoomable = true;
            chart01.ChartAreas["Current_Magnitude"].AxisX.Interval = 100;
            chart01.ChartAreas["Current_Magnitude"].AxisY.Interval = 10;
            chart01.ChartAreas["Current_Magnitude"].AxisY.ScaleView.SmallScrollMinSize = 0.1;
            

            /// Kolory wykresów
            chart01.Series["Transmittance"].Color = System.Drawing.Color.Red;
            chart01.Series["PhaseSpectrum"].Color = System.Drawing.Color.Blue;
            chart01.Series["Envelope+"].Color = System.Drawing.Color.MediumPurple;
            chart01.Series["Envelope-"].Color = System.Drawing.Color.OrangeRed;
            //---
            chart01.Titles[0].Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            chart01.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);
            chart01.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);
            chart01.ChartAreas[1].AxisX.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);
            chart01.ChartAreas[1].AxisY.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);
            chart01.ChartAreas[2].AxisX.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);
            chart01.ChartAreas[2].AxisY.TitleFont = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular);

            /// Osie wykresów
        }

        private void resetValue_Click(object sender, RoutedEventArgs e)
        {
            this.txtMagnitude.Text = "10";
            this.U1 = 10;
        }

        private void clearWaveforms_Click(object sender, RoutedEventArgs e)
        {
            chart01.Series.Clear();
            chart01.Titles.Clear();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            chart01.Series.Clear();
            chart01.Titles.Clear();
        }

        private void save_txt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text file(*.txt)|*.txt|c# file (*cs)|*.cs";
            if (dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName, tx1.Text);
            }
        }

        private void image_Click(object sender, RoutedEventArgs e)
        {
            //ExportToImage(scrollViewer);
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = "png";
            dlg.FileName = "FilterPlots.png";
            dlg.RestoreDirectory = true;
            Nullable<bool> result = dlg.ShowDialog();
            string path = dlg.FileName;
            chart01.SaveImage(dlg.FileName, ChartImageFormat.Png);
        }

    }
}
