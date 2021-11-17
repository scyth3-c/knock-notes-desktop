using System;
using System.Diagnostics;
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
using MongoDB.Driver;
using MongoDB.Bson;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
    

        public MainWindow()
        {
            InitializeComponent();
            CHARGE();


        }

        private void Nombre_w(object sender, TextChangedEventArgs e)
        {
            escibiendo.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(253, 254, 254));
        }
        private void Contenido_w(object sender, TextChangedEventArgs e)
        {
            escibiendo.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(20, 143, 119));
        }

        private void Enviar(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(nombre.Text) && !string.IsNullOrEmpty(contenido.Text))
            {
                if (nombre.Text.Length > 4 && contenido.Text.Length > 8)
                {
                    DBAPI Enviar_api = new(nombre.Text, contenido.Text);
                    Enviar_api.Enviar();
                    CHARGE();
                    escibiendo.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(241, 148, 138));
                    _ = MessageBox.Show("Enviado");
                }
                else {
                    _ = MessageBox.Show("el nombre debe ser mayor a 4 y el contenido a 10");
                }
            } else
            {
                _ = MessageBox.Show("Datos incompletos!");
            }
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            CHARGE();
            escibiendo.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(241, 148, 138));
        }

        public async void CHARGE()
        {
            DBAPI lister = new();
            var list = await lister.Charge();

            for (int i = 0; i < list.Count; i++)
            {
                _ = Notas.Items.Add(list[i]);
              
            }
        }

        private void Notas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            escibiendo.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(41, 128, 185));
            if (Notas.SelectedItem != null)
            {

                Nota aux = Notas.SelectedItem as Nota;
                show_conten.Text = aux.conten;
                _ = MessageBox.Show(aux.conten);
                
              
            }
        }
   
    }
    public class DBAPI
    {
        private readonly string nombre;
        private readonly string contenido;
        private IMongoDatabase database;
        private IMongoClient client;
        private IMongoCollection<Nota> Tasks;


        public DBAPI(string nombre, string contenido)
        {
            this.nombre = nombre;
            this.contenido = contenido;
            Verify();
        }
        public DBAPI()
        {
            Verify();
        }
        public async Task<List<Nota>> Charge()
        {
            Verify();
            List<Nota> list = await Tasks.Find(_=>true).ToListAsync();
            return list;
        }
        public void Enviar()
        {
            Nota documento = new(nombre, contenido);
            Tasks.InsertOne(documento);
        }
        public void Verify(){
           if(Tasks != null) {
                return;
            } else
            {
                client = new MongoClient("DATABASE TOKEN!!!!");
                database = client.GetDatabase("Tasks");
                Tasks = database.GetCollection<Nota>("tasks");

                
            }
        }

        
   
    }


    public class Nota
    {
        public ObjectId Id { get; set; }
        public string nombre { get; set; }
        public string conten { get; set; }
        public Int32 __v { get; set; } 


        public Nota(string nombre, string conten) 
        {
            this.nombre = nombre;
            this.conten = conten;
        }

        public Nota()
        {
           
        }
    }
    



}
