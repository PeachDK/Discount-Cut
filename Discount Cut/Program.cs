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

            Hardresser Hairdresser_A = new Hardresser(Hardresser.State.Waiting);
            Hardresser Hairdresser_B = new Hardresser(Hardresser.State.Waiting);
            Hardresser Hairdresser_C = new Hardresser(Hardresser.State.Waiting);
            Hardresser Hairdresser_D = new Hardresser(Hardresser.State.Waiting);
            Hardresser Hairdresser_E = new Hardresser(Hardresser.State.Waiting);
            Hardresser Hairdresser_F = new Hardresser(Hardresser.State.Waiting);


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

            int lastsum = 0;
            while (true)
            {
                
                int sum = 0;
                

                sum = sum + Hairdresser_A.Customer_Serviced + Hairdresser_B.Customer_Serviced + Hairdresser_C.Customer_Serviced +
                      Hairdresser_D.Customer_Serviced + Hairdresser_E.Customer_Serviced + Hairdresser_F.Customer_Serviced;



                if (sum == lastsum)
                {
                    lastsum = sum + 1;
                    Console.WriteLine("                                                            Customers Serviced = " + sum);
                    
                }
                


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
        public enum State
        {
            Waiting,
            Break,
            Work
        }

        private object Locker = new object();
        public int Customer_Serviced = 0;
        public State state { get; set; }


        public Hardresser(State staterino)
        {
            state = staterino;
        }


        public void ServiceCustomer(Scissor Scissor1, Scissor Scissor2)
        {
            Random random = new Random();
            int unitwork = 10;
            int process = 0;

            while (true)
            {
                

                if (state == State.Work)
                {
                    Monitor.Enter(Scissor1);
                    Monitor.Enter(Scissor2);

                    Console.WriteLine("Using: Scissors " + Scissor1.Label + " and " + Scissor2.Label + " Start Process: " + process);

                    Scissor1.used++; Scissor2.used++;

                    Console.WriteLine("      10-" + unitwork + "  Process: " + process);
                    Console.WriteLine("      " + Scissor1.Label + "   " + Scissor1.used);
                    Console.WriteLine("      " + Scissor2.Label + "   " + Scissor2.used);

                    Monitor.Pulse(Scissor1);
                    Monitor.Pulse(Scissor2);
                    unitwork = unitwork -1;
                    state = State.Break;
                    if (unitwork <= 0)
                    {
                        Customer_Serviced++;
                        unitwork = 10;
                        state = State.Waiting;
                    }
                    
                }


                if (state == State.Waiting)
                {
                    Thread.Sleep(random.Next(2000, 6000));
                    state = State.Work;
                    process = Processes.StaticInstance.nextid();
                }

                if (state == State.Break)
                {
                    Thread.Sleep(random.Next(4000, 10000));
                    state = State.Work;
                }

            }


        }

    }
}





