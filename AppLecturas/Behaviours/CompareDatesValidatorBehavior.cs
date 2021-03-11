using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppLecturas.Behaviours
{
    //clase para realizar comparación de controles que reciben valores tipo fecha
    public class CompareDatesValidatorBehavior : Behavior<DatePicker>
    {
        public static BindableProperty DateProperty = BindableProperty.Create<CompareDatesValidatorBehavior, DateTime>(dt => dt.Date, DateTime.Now, BindingMode.TwoWay);

        public DateTime Date
        {
            get { return (DateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static BindableProperty OrderProperty = BindableProperty.Create<CompareDatesValidatorBehavior, int>(dt => dt.Order, 0, BindingMode.TwoWay);

        public int Order
        {
            get { return (int)GetValue(OrderProperty); }
            set { SetValue(OrderProperty, value); }
        }

        public static BindableProperty DatePickProperty = BindableProperty.Create<CompareDatesValidatorBehavior, DatePicker>(dt => dt.DatePick, null, BindingMode.TwoWay);

        public DatePicker DatePick
        {
            get { return (DatePicker)GetValue(DatePickProperty); }
            set { SetValue(DatePickProperty, value); }
        }

        protected override void OnAttachedTo(DatePicker dp)
        {
            dp.DateSelected += DateSelected;
            base.OnAttachedTo(dp);
        }

        // Compara que la fecha seleccionada sea mayor a la de otro control
        private void DateSelected(object sender, DateChangedEventArgs e)
        {
            bool valido = (Order == 1) ? Date > e.NewDate : (e.NewDate > Date);
            ((DatePicker)sender).BackgroundColor = valido ? Color.Green : Color.Red;
            DatePick.BackgroundColor = valido ? Color.Green : Color.Red;
        }

        protected override void OnDetachingFrom(DatePicker dp)
        {
            dp.DateSelected -= DateSelected;
            base.OnDetachingFrom(dp);
        }
    }
}
