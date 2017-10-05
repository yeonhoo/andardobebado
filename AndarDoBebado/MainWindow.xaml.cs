using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media; // Pen

using System.IO;
using Microsoft.Research.DynamicDataDisplay; // Core functionality
using Microsoft.Research.DynamicDataDisplay.DataSources; // EnumerableDataSource
using Microsoft.Research.DynamicDataDisplay.PointMarkers; // CirclePointMarker


namespace AndarDoBebado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        private void Window1_Loaded(object sender, RoutedEventArgs e)
        {

            PlotGraphic(1000);

            plotter.Viewport.FitToView();
            grafico2.Viewport.FitToView();

        } // Window1_Loaded()

        private void PlotGraphic(int passos)
        {
            Double L = 1; //longitude

            double XCoordenada = 0;
            double YCoordenada = 0;

            List<double> XCoordenadas = new List<double>();
            List<double> YCoordenadas = new List<double>();
            List<double> Distancias = new List<double>();
            List<double> DistanciasRelativa = new List<double>();
            List<int> NumeroPassos = new List<int>();

            Random rand = new Random();

            for (var i = 0; i < passos; i++)
            {


                int degree = rand.Next(0, 360); // pega um grau aleatorio

                double X1Coordenada = L * Math.Cos(Math.PI / 180 * degree) + XCoordenada; // gera a coord X
                double Y1Coordenada = L * Math.Sin(Math.PI / 180 * degree) + YCoordenada;

                double Distancia = Math.Sqrt(Math.Pow(X1Coordenada, 2) + Math.Pow(Y1Coordenada, 2)); // calcula a distancia
                Distancias.Add(Distancia);

                NumeroPassos.Add(i + 1);

                double DistanciaR = L * Math.Sqrt(i);
                DistanciasRelativa.Add(DistanciaR);

                XCoordenadas.Add(X1Coordenada);
                YCoordenadas.Add(Y1Coordenada);

                XCoordenada = X1Coordenada;
                YCoordenada = Y1Coordenada;
            }



            var coordXDataSource = new EnumerableDataSource<double>(XCoordenadas); // coordenada x do caminho percorrido
            coordXDataSource.SetXMapping(x => x);

            var coordYDataSource = new EnumerableDataSource<double>(YCoordenadas);// coordenada y do caminho percorrido
            coordYDataSource.SetYMapping(y => y);

            var passosXDataSource = new EnumerableDataSource<int>(NumeroPassos); // numero de passos gerados para representar a coordenada x
            passosXDataSource.SetXMapping(x => x);

            var distanciasYDataSource = new EnumerableDataSource<double>(Distancias); // distancias para o eixo y
            distanciasYDataSource.SetYMapping(y => y);

            var distanciasRelativaYDataSource = new EnumerableDataSource<double>(DistanciasRelativa); // Distancias Relativa para o eixo y
            distanciasRelativaYDataSource.SetYMapping(y => y);

            CompositeDataSource compositeDataSource = new CompositeDataSource(coordXDataSource, coordYDataSource); // dados p/ grafico do caminho percorrido

            CompositeDataSource compositeDataSource2 = new CompositeDataSource(passosXDataSource, distanciasYDataSource); // dados p/ grafico distancia percorrida - distancia simulada

            CompositeDataSource compositeDataSource3 = new CompositeDataSource(passosXDataSource, distanciasRelativaYDataSource); // dados p/ grafico distancia percorrida - distancia relativa

            // inicio - deleta o grafico atual da tela
            plotter.Children.RemoveAll(typeof(LineGraph));
            plotter.Children.RemoveAll(typeof(ElementMarkerPointsGraph));
            plotter.Children.RemoveAll(typeof(MarkerPointsGraph));

            grafico2.Children.RemoveAll(typeof(LineGraph));
            grafico2.Children.RemoveAll(typeof(ElementMarkerPointsGraph));
            grafico2.Children.RemoveAll(typeof(MarkerPointsGraph));

            // fim - deleta o grafico atual da tela

            plotter.AddLineGraph(compositeDataSource,
                new Pen(Brushes.Blue, 5),
              new TrianglePointMarker
              {
                  Size = 10.0,
                  Pen = new Pen(Brushes.Black, 2.0),
                  Fill = Brushes.GreenYellow
              },
              new PenDescription("Andar do Bêbado"));

            grafico2.AddLineGraph(compositeDataSource2,
              new Pen(Brushes.Blue, 2),
                //new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
              new PenDescription("Distancia Simulada"));

            grafico2.AddLineGraph(compositeDataSource3,
              new Pen(Brushes.Red, 2),
                //new CirclePointMarker { Size = 10.0, Fill = Brushes.Red },
              new PenDescription("Distancia Estimada"));

            Passos.Text = passos.ToString();
            DistanciaPercorrida.Text = Distancias[Distancias.Count - 1].ToString(); // seta o campo com o ultimo elemento 
            Diferenca.Text = (Distancias[Distancias.Count - 1] - DistanciasRelativa[DistanciasRelativa.Count - 1]).ToString();
        }

        // Evento do botao OK
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            String input = this.Passos.Text;
            int passos = Convert.ToInt32(input);

            PlotGraphic(passos);
            
        }


    } // class Window1
}