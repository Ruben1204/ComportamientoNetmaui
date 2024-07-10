using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComportamientoMaui
{
    public class NumericValidationStyleBehavior : Behavior<Entry>
    {
        //lo primero es definir una propiedad adjunta la cual nos servira para controlar la adicion o eliminacion del comportamoento del control
        //definimos la propiedad
        public static readonly BindableProperty AttachBehaviorProperty = BindableProperty.CreateAttached("AttachBehavior", typeof(bool), typeof(NumericValidationStyleBehavior), false, propertyChanged: OnAttachedBehaviorChanged);//aqui ponemos el nombre de la propiedad el tipo que retorna el tipo declarante, valor por defecto y accion cuando la propiedad cambie

        //declaramos metodos para obtener la pripiedad adjunta
        public static bool GetAttachBehavior(BindableObject view)
        {
            return (bool)view.GetValue(AttachBehaviorProperty);
        }

        //metodo establecedor para la propiedad adjunta
        public static void SetAttachBehavior(BindableObject view, bool value)
        {
            view.SetValue(AttachBehaviorProperty, value);
        }

        //ahora definiremos el metodo que se ejecutara cuando la propiedad adjunta cambie

        static void OnAttachedBehaviorChanged(BindableObject view, object oldValue, object newValue)
        {
            //aqui convertimos la vista a un entry es decir normalizamos la vista
            Entry entry = view as Entry;
            if (entry == null)
            {
                return; // Salimos si la vista no es un entry
            }

            //aqui capturamos el nuevo valor del comportamiento adjunto
            bool attachBehavior = (bool)newValue;
            if (attachBehavior)
            {
                //Agregamos el comportamiento si aplica o se aprueba
                entry.Behaviors.Add(new NumericValidationStyleBehavior());
            }
            else
            {
                //De lo contrario quitamos el comportamiento si no aplica o esta deshabilitado
                Behavior toRemove = entry.Behaviors.FirstOrDefault(b => b is  NumericValidationStyleBehavior);
                if (toRemove != null)
                {
                    entry.Behaviors.Remove(toRemove);
                }
            }
        }

        //Este es el metodo que se llama cuando el comportamiento se agrega o adjunta a un entry
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;//Aqui lo que estamos haciendo es suscribiendonos al evento que posteriormente configuraremos.
            base.OnAttachedTo(entry);//aqui aseguramos que cualquier logica de clase base tambien se ejecute.
        }

        //Este es el metodo que se llama cuando el comportamiento se desagrega o desadjunta de un entry
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -=OnEntryTextChanged;//aqui cancelamos la suscripción al evento Textchanged del Entry
            base.OnDetachingFrom(entry);//aqui aseguramos que cualquier logica de clase base tambien se ejecute.
        }

        //Este es el controlador de eventos para el evento TextChanged del Entry en este caso
        void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            double result;
            bool isValid = double.TryParse(e.NewTextValue, out result);//aqui lo que hacemos es intentar convertir el nuevo valor de texto a un double usando el tryparse
            ((Entry)sender).TextColor = isValid ? Colors.Black : Colors.Purple;//Y aqui evaluamos si la conversion fue exitosa el color del texto seera negra y si no sera purpura
        }
    }

}
