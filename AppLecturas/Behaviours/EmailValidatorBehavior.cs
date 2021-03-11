using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppLecturas.Behaviours
{
    //clase para validar controles que reciban email válido
    public class EmailValidatorBehavior : Behavior<Entry>
    {
        const string emailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Valida si el texto introducido es un correo electrónico
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
            ((Entry)sender).TextColor = valido ? Color.Green : Color.Red;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }
}
