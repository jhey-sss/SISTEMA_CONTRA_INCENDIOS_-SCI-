using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCR
{
    internal class Program

    {
        static void Main(string[] args)
        {
            int op;//
            PanelCentral Pcentral = new PanelCentral();

            SensorHumo SH1 = new SensorHumo("SH1", "A1", true, "SENSOR");
            SensorTemp ST1 = new SensorTemp("ST1", "B1", true, "SENSOR");

            Console.WriteLine((SH1.Id,SH1.Ubicacion,SH1.Estado,SH1.NHumoActual));
            Console.WriteLine((ST1.Id, ST1.Ubicacion, ST1.Estado, ST1.TempActual));
            Console.WriteLine(Pcentral);

            Pcentral.AgregarDispositivo(SH1);
            Pcentral.AgregarDispositivo(ST1);


            while (true)
            {
                Console.WriteLine("\n--- MONITOREO DEL SISTEMA ---");
                Console.WriteLine("1. Hacer un escaneo de seguridad");
                Console.WriteLine("2. humo en el Turbogenerador");
                Console.WriteLine("3. temperatura en el Turbogenerador");
                Console.WriteLine("4. Salir");
                Console.Write("Elige una opción: ");
                op = int.Parse(Console.ReadLine());

                if (op == 1)
                {
                    Pcentral.EscaneoSistema();
                }
                else if ( op == 2)
                {
                    Console.WriteLine("Valor de humo actualizado a "+ SH1.NHumoActual);
                }
                else if (op == 3)
                {
                    Console.WriteLine("Valor de temperatura actualizado a " + ST1.TempActual);
                }
                else if (op == 4)
                {
                    Console.WriteLine("Saliendo del programa...");
                    break;
                }
                else
                {
                    Console.WriteLine("Opcion no valida");
                }

            }
        }

    }
    public enum Edispocitivo
    {
        normal, 
        alerta,
        fallo,
    }
    public class PanelCentral
    {
        private List<Dispocitivos> lisdis;
        public bool LucesdeAlerta{ get; set; }

        public PanelCentral()
        {
            lisdis = new List<Dispocitivos>();
            LucesdeAlerta = false;

        }
        public void AgregarDispositivo(Dispocitivos nuevodispositivo)
        {
            lisdis.Add(nuevodispositivo);
            Console.WriteLine($"Dispositivo {nuevodispositivo.Id} agregado al panel central.");
        }
        public void EscaneoSistema()
        {
            bool Incendio= false;
            foreach (var dispositivo in lisdis)
            {
                dispositivo.VEstado();
                if (dispositivo.Estado == Edispocitivo.alerta)
                {
                    Incendio = true;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"incendio en el sensor: " + (dispositivo.Id, dispositivo.Ubicacion));
                    Console.ResetColor();
                }
            }
            if (Incendio)
            {
                LucesdeAlerta = true;
                Console.WriteLine("papu, hay fuego");

            }
            else
            {
                LucesdeAlerta = false;
            }
        }


    }

    public abstract class Dispocitivos
    {
        public string Id { get; set; }
        public string Ubicacion { get; set; }
        public Edispocitivo Estado { get; set; }
        public string Tipo { get; set; }

        public Dispocitivos(string Id ,string Ubicacion, bool Estado, string Tipo)
        {
            this.Id = Id;
            this.Ubicacion = Ubicacion;
            this.Estado = Edispocitivo.normal;
            this.Tipo = Tipo;
        }
        public abstract void VEstado();
        
    }
    public class SensorHumo : Dispocitivos
    {
        public double NHumoActual { get; set; }

        public double HumbralHumo = 30.0;
        public SensorHumo(string Id, string Ubicacion, bool Estado, string Tipo) : base(Id, Ubicacion, Estado, Tipo)
        {
            NHumoActual = 43.0;
        }
        public override void VEstado()
        {
            if (NHumoActual > HumbralHumo)
            {
                Estado = Edispocitivo.alerta;
            }
            else
            {
                Estado = Edispocitivo.normal;
            }
        }
    }


    public class SensorTemp : Dispocitivos
    {
        public double TempActual { get; set; }
        public double HumbralTemp = 50.0;
        public SensorTemp(string Id, string Ubicacion, bool Estado, string Tipo) : base(Id, Ubicacion, Estado, Tipo)
        {
            TempActual = 90.0;
        }
        public override void VEstado()
        {
            if (TempActual > HumbralTemp)
            {
                Estado = Edispocitivo.alerta;
            }
            else
            {
                Estado = Edispocitivo.normal;
            }
        }
    }


    public class VEstado
    {

    }

}
