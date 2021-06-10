using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Turing.Models;

namespace Turing.ViewModels
{
    class Context : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer TextTimer = new System.Windows.Threading.DispatcherTimer();
        private ObservableCollection<Caracter> cadenaOriginal {get; set;}
        private ObservableCollection<Caracter> cadenaBien { get; set; }
        private ObservableCollection<Caracter> cadenaResultado { get; set; }
        private string texto { get; set; }
        private string error { get; set; }
        private string estado { get; set; }
        private float speed { get; set; }
        private int idText { get; set; }
        public ObservableCollection<Caracter> CadenaOriginal
        {
            get { return cadenaOriginal; }
            set { cadenaOriginal = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CadenaOriginal")); }
        }

        public ObservableCollection<Caracter> CadenaBien
        {
            get { return cadenaBien; }
            set { cadenaBien = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CadenaBien")); }
        }

        public ObservableCollection<Caracter> CadenaResultado
        {
            get { return cadenaResultado; }
            set { cadenaResultado = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CadenaResultado")); }
        }
        public string Texto
        {
            get { return texto; }
            set { texto = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Texto")); }
        }

        public string Error
        {
            get { return error; }
            set { error = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Error")); }
        }

        public string Estado
        {
            get { return estado; }
            set { estado = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Estado"));  }
        }

        public int IdText
        {
            get { return idText; }
            set { idText = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IdText")); }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Speed")); Change(); }
        }
        public bool textopar { get; set; }
        public bool TextPar
        {
            get { return textopar; }
            set { textopar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextPar")); }
        }
        public bool ejecucion { get; set; }
        public bool Ejecucion
        {
            get { return ejecucion; }
            set { ejecucion = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ejecucion")); }
        }

        public ICommand ComenzarCommand { get; set; }
        public ICommand ChangeCommand { get; set; }
        public ICommand CancelarCommand { get; set; }
        int idFor = 0;
        private Caracter empty;


        public Context()
        {
            Speed = 2;
            ComenzarCommand = new RelayCommand(Inicio);
            ChangeCommand = new RelayCommand(Change);
            CancelarCommand = new RelayCommand(Cancelar);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(Speed*1000));
            TextTimer.Interval = new TimeSpan(0,0,0,0,500);
            empty = new Caracter { MiChar = ' ', IdChar = 0 };
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            TextTimer.Tick += TextTimer_Tick;
            IdText = 1;
            Ejecucion = true;
        }

        private void TextTimer_Tick(object sender, EventArgs e)
        {
            TextPar = !TextPar;
        }

        public void Change()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, (int)(Speed * 1000));
        }

        public void Inicio()
        {
            try
            {
                if(Texto!=null)
                {
                    string res = "";
                    foreach (char item in Texto)
                    {
                        if(item!=' ')
                        {
                            res += item;
                        }
                    }
                    if(res.Length>0)
                    {
                        idFor = 0;
                        CadenaOriginal = new ObservableCollection<Caracter>();
                        Random r = new Random();
                        foreach (char item in Texto)
                        {
                            Caracter car = new Caracter
                            {
                                MiChar = item,
                                IdChar = r.Next(1, 3)
                            };
                            CadenaOriginal.Add(car);
                        }
                        TextTimer.Start();
                        CadenaCorrecto();
                        Estado = "Procesando...";
                        IdText = 1;
                        Ejecucion = false;
                    }
                    else
                    {
                        throw new ArgumentException("Por favor introduce un texto o una cadena de caracteres");
                    }
                }
                else
                {
                    throw new ArgumentException("Por favor ingrese un texto");
                }
            }
            catch (Exception ex)
            {
                Error = ex.Message;
            }
            
        }
        public void CadenaCorrecto()
        {
            CadenaBien = new ObservableCollection<Caracter>();
            CadenaResultado = new ObservableCollection<Caracter>();
            foreach (Caracter item in CadenaOriginal)
            {
                if(item.MiChar!=' ')
                {
                    string a = item.MiChar.ToString();
                    char[] b = a.ToUpper().ToCharArray();
                    Caracter ca = new Caracter
                    {
                        MiChar = b[0],
                        IdChar = 0
                    };
                    CadenaBien.Add(ca);
                }
            }
            for (int i = 0; i < CadenaBien.Count; i++)
            {
                Caracter c = new Caracter
                {
                    IdChar = 0,
                    MiChar = ' '
                };
                CadenaResultado.Add(c);
            }
            dispatcherTimer.Start();
        }

        public void Cancelar()
        {
            Ejecucion = true;
            dispatcherTimer.Stop();
            TextTimer.Stop();
            Estado = "Cancelado";
            TextPar = true;
            IdText = 0;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if(CadenaResultado[idFor].MiChar==' ')
            {
                if(CadenaBien[idFor].MiChar==CadenaBien[CadenaBien.Count-1-idFor].MiChar)
                {
                    Caracter t = new Caracter
                    {
                        MiChar = CadenaBien[idFor].MiChar,
                        IdChar = 1
                    };
                    CadenaResultado[idFor] = t;
                    CadenaResultado[CadenaResultado.Count - 1 - idFor] = t;
                }
                else
                {
                    Caracter t1 = new Caracter
                    {
                        MiChar = CadenaBien[idFor].MiChar,
                        IdChar = 2
                    };
                    Caracter t2 = new Caracter
                    {
                        MiChar = CadenaBien[CadenaBien.Count - 1 - idFor].MiChar,
                        IdChar = 2
                    };
                    CadenaResultado[idFor] = t1;
                    CadenaResultado[CadenaResultado.Count - 1 - idFor] = t2;
                    Estado = "¡No es Palindromo!";
                    IdText = 0;
                    TextTimer.Stop();
                    TextPar = true;
                }
                idFor++;
            }
            else
            {
                TextTimer.Stop();
                TextPar = true;
                Ejecucion = true;
                dispatcherTimer.Stop();
                if(Estado!= "¡No es Palindromo!")
                {
                    Estado = "¡Es Palindromo!";
                    IdText = 2;
                    
                }
            }
        }
    }
}
