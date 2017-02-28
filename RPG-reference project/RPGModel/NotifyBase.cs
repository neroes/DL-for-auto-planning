using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RPGModel
{
    // Abstract base class. Bruges til at samle INotifyPropertyChanged funktionaliteten, så den ikke behøves implementeres i alle de almindelige model klasser.
    // Formålet med INotifyPropertyChanged er at når en attribut for en klasse ændres, så smides en event der giver GUI'en besked om ændringen.
    public class NotifyBase : INotifyPropertyChanged
    {
        // Her defineres den event der skal smides for at GUI'en ved at en attribut er blevet ændret.
        public event PropertyChangedEventHandler PropertyChanged;

        // Denne metode bruges til at smide PropertyChanged eventen og skal kaldes i alle set metoderne, for alle model klassernes attributer.
        // [CallerMemberName] = Annotation attribute. Bruges til at sætte parameteren propertyName til navnet på den kaldende metode. 
        // propertyName bliver dog kun sat når den ikke har fået en værdi, dette kan lade sig gøre fordi den er frivillig (se '= ""' delen).
        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
