using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Discount_Cut
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hairdressers working");

            Hardresser Hairdresser_A = new Hardresser();
            Hardresser Hairdresser_B = new Hardresser();
            Hardresser Hairdresser_C = new Hardresser();
            Hardresser Hairdresser_D = new Hardresser();
            Hardresser Hairdresser_E = new Hardresser();
            Hardresser Hairdresser_F = new Hardresser();


            Scissor Scissor_AB = new Scissor("AB");
            Scissor Scissor_BC = new Scissor("BC");
            Scissor Scissor_CD = new Scissor("CD");
            Scissor Scissor_DE = new Scissor("DE");
            Scissor Scissor_EF = new Scissor("EF");
            Scissor Scissor_FA = new Scissor("FA");

            Thread A = new Thread(() => Hairdresser_A.ServiceCustomer(Scissor_AB, Scissor_BC));
            Thread B = new Thread(() => Hairdresser_B.ServiceCustomer(Scissor_BC, Scissor_CD));
            Thread C = new Thread(() => Hairdresser_C.ServiceCustomer(Scissor_CD, Scissor_DE));
            Thread D = new Thread(() => Hairdresser_D.ServiceCustomer(Scissor_DE, Scissor_EF));
            Thread E = new Thread(() => Hairdresser_E.ServiceCustomer(Scissor_EF, Scissor_FA));
            Thread F = new Thread(() => Hairdresser_F.ServiceCustomer(Scissor_FA, Scissor_AB));


            A.Start();
            B.Start();
            C.Start();
            D.Start();
            E.Start();
            F.Start();

            while (true)
            {
                int sum = 0;


                sum = sum + Hairdresser_A.Customer_Serviced + Hairdresser_B.Customer_Serviced + Hairdresser_C.Customer_Serviced + 
                      Hairdresser_D.Customer_Serviced + Hairdresser_E.Customer_Serviced + Hairdresser_F.Customer_Serviced;

                Console.WriteLine("                                                            Customers Serviced = " + sum);
                Thread.Sleep(3000);

            }

        }



    }

    class Scissor
    {
        public string Label { get; set; }
        public int used { get; set; }

        public Scissor(string label)
        {
            Label = label;
        }

    }

    class Processes
    {
        private static readonly Processes _staticinstance = new Processes();
        public static Processes StaticInstance
        {
            get
            {
                return _staticinstance;
            }
        }
        public int ID { get; set; }

        private Processes()
        {
            ID = 1;

        }

        public int nextid()
        {
            return ID++;
        }

    }

    class Hardresser
    {
        private object Locker = new object();
        public int Customer_Serviced = 0;

        public void ServiceCustomer(Scissor Scissor1, Scissor Scissor2)
        {
            while (true)
            {
                for (int i = 10; i > 0; i--)
                {

                    Monitor.Enter(Locker);

                    int p = Processes.StaticInstance.nextid();
                    Random random = new Random();
                    Console.WriteLine("Using: Scissors " + Scissor1.Label + " and " + Scissor2.Label);
                    Console.WriteLine("Start" + p);
                    Thread.Sleep(random.Next(6000, 20000));
                    Scissor1.used++;
                    Scissor2.used++;
                    Console.WriteLine("Done" + p);
                    Console.WriteLine("                                           " + Scissor1.Label + "   " + Scissor1.used);
                    Console.WriteLine("                                           " + Scissor2.Label + "   " + Scissor2.used);

                    Monitor.Exit(Locker);
                }

                Customer_Serviced++;

            }


        }

    }
}


    


