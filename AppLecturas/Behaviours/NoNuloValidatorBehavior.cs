using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppLecturas.Behaviours
{
    //clase para validar que un control no contenga un valor nulo
    class NoNuloValidatorBehavior:Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Solo texto
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido;
            if (e.NewTextValue == null)
                valido = false;
            else
            {
                var str = e.NewTextValue as string;
                valido =  !string.IsNullOrWhiteSpace(str);
            }            
            ((Entry)sender).BackgroundColor = valido ? Color.Green : Color.Red;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }
}
