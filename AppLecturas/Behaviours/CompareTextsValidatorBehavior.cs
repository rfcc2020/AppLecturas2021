using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppLecturas.Behaviours
{
    public class CompareTextsValidatorBehavior : Behavior<Entry>
    {
        //clase para comparar el texto entre controles
        public static BindableProperty TextProperty = BindableProperty.Create<CompareTextsValidatorBehavior, string>(tc => tc.Text, string.Empty, BindingMode.TwoWay);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static BindableProperty EntProperty = BindableProperty.Create<CompareTextsValidatorBehavior, Entry>(tc => tc.Ent, null, BindingMode.TwoWay);

        public Entry Ent
        {
            get { return (Entry)GetValue(EntProperty); }
            set { SetValue(EntProperty, value); }
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += TextChanged;
            base.OnAttachedTo(entry);
        }

        // Compara que el texto de este control sea el mismo que el de otro control
        void TextChanged(object sender, TextChangedEventArgs e)
        {
            bool valido = (e.NewTextValue == Text);
            ((Ent == null) ? (Entry)sender : Ent).TextColor = valido ? Color.Green : Color.Red;
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= TextChanged;
            base.OnDetachingFrom(entry);
        }
    }
}
