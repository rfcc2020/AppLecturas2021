using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace AppLecturas.Behaviours
{
    //clase para validar que el password tenga un caracter mayúscula, un dígito y que sea de 6 caracteres
    public class PasswordValidatorBehavior : Behavior<Entry>
    {
        const string passwordRegex = @"^(?=.*[a-z])(?=.*\d)[a-z\d$@$!%*#?&]{8}$";

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        //10 caracteres, con al menos: 1 digito, 1 minúscula, 1 mayúscula, 1 caracter especial
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (Regex.IsMatch(e.NewTextValue, passwordRegex));
            ((Entry)sender).TextColor = valido ? Color.Green : Color.Red;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }
}
