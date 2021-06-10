using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turing.Models
{
    public class Caracter : INotifyPropertyChanged
    {
        private char miChar;
        private int idChar;

        public char MiChar { get { return miChar; } set { miChar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MiChar")); } }
        public int IdChar { get { return idChar;} set { idChar = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IdChar")); } }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
